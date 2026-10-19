using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCheck : MonoBehaviour
{
    public float addNum; //在上升气流中停止时每帧恢复的体力
    public bool isOnAir=false; //是否在上升气流中

    private void FixedUpdate()
    {
        if(isOnAir&&PlayerMove3.Instance.isStop)
        {
            StaminaChange.Instance.UpdateStaminaChange(addNum);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            isOnAir = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isOnAir = false;
        }
    }

    public void OnAirSound()
    {
        GameSceneManager.Instance.PlaySoundByTriggerType(E_SoundTriggerType.Chapter3AirflowSound, false, true);
    }
}
