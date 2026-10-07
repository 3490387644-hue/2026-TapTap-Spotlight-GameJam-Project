

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;

public static class ExcelToJson
{
    private const string OutputFileName = "DialogData.json";

    [MenuItem("TapTapGameTools/Excel2Json")]
    private static void ConvertExcelToJson()
    {
        string excelPath = EditorUtility.OpenFilePanel("选择 Excel 文件", @"D:\", "xlsx");
        if (string.IsNullOrEmpty(excelPath)) return;

        try
        {
            List<Dictionary<string, object>> rows = ReadWorkbook(excelPath);
            string outputPath = Path.Combine(Application.dataPath, "StreamingAssets", OutputFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            // 修复乱码：使用 UTF-8 BOM 编码写入，LitJson 读取时中文不会乱码
            File.WriteAllText(outputPath, SerializeRows(rows), new UTF8Encoding(true));
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Excel2Json",
                $"转换成功，共导出 {rows.Count} 行数据。\n{outputPath}", "确定");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            EditorUtility.DisplayDialog("Excel2Json 转换失败", e.Message, "确定");
        }
    }

    private static List<Dictionary<string, object>> ReadWorkbook(string path)
    {
        if (!path.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            throw new NotSupportedException("当前工具仅支持 .xlsx 文件，不支持旧版 .xls 文件。");

        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
        using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
        {
            List<string> sharedStrings = ReadSharedStrings(archive);
            XmlDocument sheet = LoadFirstWorksheet(archive);
            XmlNodeList cells = sheet.SelectNodes("//*[local-name()='sheetData']/*[local-name()='row']/*[local-name()='c']");
            if (cells == null || cells.Count == 0)
                throw new InvalidDataException("Excel 文件没有包含可读取的数据。");

            List<XmlNode> cellsList = cells.Cast<XmlNode>().ToList();
            Dictionary<int, string> headers = ReadRow(cellsList, 1, sharedStrings)
                .Where(p => !string.IsNullOrWhiteSpace(p.Value))
                .ToDictionary(p => p.Key, p => p.Value.Trim());

            Dictionary<int, string> types = ReadRow(cellsList, 2, sharedStrings);

            if (headers.Count == 0)
                throw new InvalidDataException("第一行没有找到字段名。");

            foreach (KeyValuePair<int, string> header in headers)
            {
                if (!types.TryGetValue(header.Key, out string type) || string.IsNullOrWhiteSpace(type))
                    throw new InvalidDataException($"字段 \"{header.Value}\" 没有在第二行声明数据类型。");
            }

            int lastRow = cellsList.Select(cell => GetRowNumber(cell.ParentNode)).DefaultIfEmpty(2).Max();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            for (int rowNumber = 3; rowNumber <= lastRow; rowNumber++)
            {
                Dictionary<int, string> values = ReadRow(cellsList, rowNumber, sharedStrings);
                if (values.Count == 0) continue;

                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (KeyValuePair<int, string> header in headers)
                {
                    string type = types[header.Key].Trim().ToLowerInvariant();
                    values.TryGetValue(header.Key, out string value);
                    row[header.Value] = ConvertValue(value ?? string.Empty, type, rowNumber, header.Value);
                }
                result.Add(row);
            }

            return result;
        }
    }

    private static Dictionary<int, string> ReadRow(List<XmlNode> cells, int rowNumber, List<string> sharedStrings)
    {
        Dictionary<int, string> values = new Dictionary<int, string>();
        foreach (XmlNode cell in cells)
        {
            if (GetRowNumber(cell.ParentNode) != rowNumber) continue;
            string reference = cell.Attributes?["r"]?.Value;
            if (string.IsNullOrEmpty(reference)) continue;
            values[GetColumnNumber(reference)] = ReadCellValue(cell, sharedStrings);
        }
        return values;
    }

    private static string ReadCellValue(XmlNode cell, List<string> sharedStrings)
    {
        string type = cell.Attributes?["t"]?.Value;
        if (type == "inlineStr")
            return string.Concat(cell.SelectNodes(".//*[local-name()='t']").Cast<XmlNode>().Select(n => n.InnerText));

        XmlNode valueNode = cell.SelectSingleNode("./*[local-name()='v']");
        string value = valueNode?.InnerText ?? string.Empty;
        if (type == "s" && int.TryParse(value, out int idx) && idx >= 0 && idx < sharedStrings.Count)
            return sharedStrings[idx];
        return value;
    }

    private static List<string> ReadSharedStrings(ZipArchive archive)
    {
        ZipArchiveEntry entry = archive.GetEntry("xl/sharedStrings.xml");
        if (entry == null) return new List<string>();

        XmlDocument doc = LoadXml(entry);
        return doc.SelectNodes("//*[local-name()='si']")
            .Cast<XmlNode>()
            .Select(node => string.Concat(node.SelectNodes(".//*[local-name()='t']").Cast<XmlNode>().Select(t => t.InnerText)))
            .ToList();
    }

    private static XmlDocument LoadFirstWorksheet(ZipArchive archive)
    {
        XmlDocument workbook = LoadXml(archive.GetEntry("xl/workbook.xml"));
        XmlNode sheet = workbook.SelectSingleNode("//*[local-name()='sheets']/*[local-name()='sheet']");
        string relId = sheet?.Attributes?["r:id"]?.Value;
        if (string.IsNullOrEmpty(relId))
            throw new InvalidDataException("Excel 未找到工作表。");

        XmlNode rel = LoadXml(archive.GetEntry("xl/_rels/workbook.xml.rels"))
            .SelectSingleNode($"//*[local-name()='Relationship'][@Id='{relId}']");
        string target = rel?.Attributes?["Target"]?.Value;
        if (string.IsNullOrEmpty(target))
            throw new InvalidDataException("无法定位第一个工作表。");

        return LoadXml(archive.GetEntry("xl/" + target.TrimStart('/')));
    }

    private static XmlDocument LoadXml(ZipArchiveEntry entry)
    {
        if (entry == null) throw new InvalidDataException("Excel 文件结构不完整。");
        XmlDocument doc = new XmlDocument();
        doc.Load(entry.Open());
        return doc;
    }

    private static int GetRowNumber(XmlNode row)
    {
        return int.Parse(row.Attributes?["r"]?.Value ?? "0", CultureInfo.InvariantCulture);
    }

    private static int GetColumnNumber(string cellReference)
    {
        int column = 0;
        foreach (char c in cellReference.TakeWhile(char.IsLetter))
            column = column * 26 + char.ToUpperInvariant(c) - 'A' + 1;
        return column;
    }

    private static object ConvertValue(string value, string type, int rowNumber, string fieldName)
    {
        if (type == "string") return value;
        if (string.IsNullOrEmpty(value))
        {
            if (type == "enum")
                throw new InvalidDataException($"第 {rowNumber} 行枚举字段 \"{fieldName}\" 不能为空。");
            return type == "bool" ? false : 0;
        }

        try
        {
            switch (type)
            {
                case "int": return int.Parse(value, CultureInfo.InvariantCulture);
                case "long": return long.Parse(value, CultureInfo.InvariantCulture);
                case "float": return float.Parse(value, CultureInfo.InvariantCulture);
                case "double": return double.Parse(value, CultureInfo.InvariantCulture);
                case "bool":
                    if (value == "1") return true;
                    if (value == "0") return false;
                    return bool.Parse(value);
                case "enum": return ParseEnum(value, rowNumber, fieldName);
                default:
                    throw new InvalidDataException($"不支持的数据类型 \"{type}\"。");
            }
        }
        catch (FormatException e)
        {
            throw new InvalidDataException(
                $"第 {rowNumber} 行字段 \"{fieldName}\" 的值 \"{value}\" 无法转换为 {type}。", e);
        }
    }

    private static E_DialogFuncType ParseEnum(string value, int rowNumber, string fieldName)
    {
        // 尝试按枚举名称解析（如 "Type1"）
        if (Enum.TryParse(value, true, out E_DialogFuncType result) && Enum.IsDefined(typeof(E_DialogFuncType), result))
            return result;
        // 尝试按数字解析（如 "0" -> Type1）
        if (int.TryParse(value, out int num))
        {
            if (Enum.IsDefined(typeof(E_DialogFuncType), num))
                return (E_DialogFuncType)num;
            throw new InvalidDataException(
                $"第 {rowNumber} 行枚举字段 \"{fieldName}\" 的值 {value} 不是有效的 E_DialogFuncType 枚举值。");
        }
        throw new InvalidDataException(
            $"第 {rowNumber} 行枚举字段 \"{fieldName}\" 的值 \"{value}\" 不是有效的 E_DialogFuncType。");
    }

    private static string SerializeRows(List<Dictionary<string, object>> rows)
    {
        StringBuilder json = new StringBuilder();
        json.AppendLine("[");
        for (int i = 0; i < rows.Count; i++)
        {
            Dictionary<string, object> row = rows[i];
            json.Append("  {");
            int j = 0;
            foreach (KeyValuePair<string, object> field in row)
            {
                if (j++ > 0) json.Append(",");
                json.AppendLine();
                json.Append("    ");
                AppendJsonString(json, field.Key);
                json.Append(": ");
                AppendJsonValue(json, field.Value);
            }
            json.AppendLine();
            json.Append("  }");
            if (i < rows.Count - 1) json.Append(",");
            json.AppendLine();
        }
        json.Append("]");
        return json.ToString();
    }

    private static void AppendJsonValue(StringBuilder json, object value)
    {
        if (value is string s) { AppendJsonString(json, s); return; }
        if (value is bool b) { json.Append(b ? "true" : "false"); return; }
        if (value is Enum e) { json.Append(Convert.ToInt32(e)); return; }
        if (value is IFormattable f) { json.Append(f.ToString(null, CultureInfo.InvariantCulture)); return; }
        throw new InvalidDataException("不支持输出的数据类型：" + (value?.GetType().Name ?? "null") + "。");
    }

    private static void AppendJsonString(StringBuilder json, string value)
    {
        json.Append('"');
        foreach (char c in value)
        {
            switch (c)
            {
                case '"': json.Append("\\\""); break;
                case '\\': json.Append("\\\\"); break;
                case '\b': json.Append("\\b"); break;
                case '\f': json.Append("\\f"); break;
                case '\n': json.Append("\\n"); break;
                case '\r': json.Append("\\r"); break;
                case '\t': json.Append("\\t"); break;
                default:
                    if (c < 32)
                        json.Append("\\u").Append(((int)c).ToString("x4"));
                    else
                        json.Append(c);
                    break;
            }
        }
        json.Append('"');
    }
}