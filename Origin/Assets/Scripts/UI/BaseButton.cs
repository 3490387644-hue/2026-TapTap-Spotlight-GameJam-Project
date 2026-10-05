using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class BaseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 鼠标悬停时按钮相对于初始位置的偏移量。
    [SerializeField] private Vector2 hoverOffset = new Vector2(-20f, 20f);
    // 鼠标悬停时按钮相对于初始大小的缩放倍数。
    [SerializeField, Min(1f)] private float hoverScale = 1.1f;
    // 按钮位置和大小完成过渡动画所需的时间。
    [SerializeField, Min(0f)] private float animationDuration = 0.15f;

    // 当前按钮的矩形变换组件。
    private RectTransform rectTransform;
    // 按钮未悬停时的初始锚点位置。
    private Vector2 normalPosition;
    // 按钮未悬停时的初始缩放值。
    private Vector3 normalScale;
    // 当前过渡动画开始时按钮的锚点位置。
    private Vector2 animationStartPosition;
    // 当前过渡动画开始时按钮的缩放值。
    private Vector3 animationStartScale;
    // 当前过渡动画已经播放的时间。
    private float animationTime;
    // 当前鼠标是否悬停在按钮上。
    private bool isHovered;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        normalPosition = rectTransform.anchoredPosition;
        normalScale = rectTransform.localScale;
        animationStartPosition = normalPosition;
        animationStartScale = normalScale;
    }

    private void Update()
    {
        if (animationDuration <= 0f)
        {
            ApplyTargetTransform(1f);
            return;
        }

        animationTime = Mathf.Min(animationTime + Time.unscaledDeltaTime, animationDuration);
        // 当前动画的平滑插值进度，取值范围为 0 到 1。
        float progress = Mathf.SmoothStep(0f, 1f, animationTime / animationDuration);
        ApplyTargetTransform(progress);
    }

    /// <summary>
    /// 鼠标指针进入按钮区域时触发。
    /// </summary>
    /// <param name="eventData">鼠标指针事件数据。</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetHoverState(true);
    }

    /// <summary>
    /// 鼠标指针离开按钮区域时触发。
    /// </summary>
    /// <param name="eventData">鼠标指针事件数据。</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        SetHoverState(false);
    }

    // 根据鼠标是否悬停，重新设置过渡动画的起点。
    /// <param name="hovered">按钮当前是否处于悬停状态。</param>
    private void SetHoverState(bool hovered)
    {
        if (isHovered == hovered)
        {
            return;
        }

        isHovered = hovered;
        animationStartPosition = rectTransform.anchoredPosition;
        animationStartScale = rectTransform.localScale;
        animationTime = 0f;
    }

    /// <param name="progress">当前过渡动画的插值进度，取值范围为 0 到 1。</param>
    private void ApplyTargetTransform(float progress)
    {
        // 当前悬停状态对应的目标位置。
        Vector2 targetPosition = isHovered ? normalPosition + hoverOffset : normalPosition;
        // 当前悬停状态对应的目标缩放值。
        Vector3 targetScale = isHovered ? normalScale * hoverScale : normalScale;

        rectTransform.anchoredPosition = Vector2.Lerp(animationStartPosition, targetPosition, progress);
        rectTransform.localScale = Vector3.Lerp(animationStartScale, targetScale, progress);
    }

    private void OnDisable()
    {
        isHovered = false;
        animationTime = animationDuration;

        if (rectTransform == null)
        {
            return;
        }

        rectTransform.anchoredPosition = normalPosition;
        rectTransform.localScale = normalScale;
    }
}
