using Cinemachine;
using UnityEngine;

public class VirtualCameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private CinemachineVirtualCamera virtualCamera;
    private Transform followTarget;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera == null)
        {
            Debug.LogError(
                "VirtualCameraFollowPlayer",
                this
            );
        }
    }

    private void LateUpdate()
    {
        if (virtualCamera == null)
            return;

        if (followTarget == null)
        {
            GameObject player = FindPlayer();
            if (player == null)
                return;

            followTarget = player.transform;
            virtualCamera.Follow = followTarget;
        }
        else if (virtualCamera.Follow != followTarget)
        {
            virtualCamera.Follow = followTarget;
        }
    }

    private GameObject FindPlayer()
    {
        try
        {
            return GameObject.FindGameObjectWithTag(playerTag);
        }
        catch (System.Exception e)
        {
            Debug.LogError(
                "VirtualCameraFollowPlayer",
                this
            );
            Debug.LogException(e, this);
            return null;
        }
    }
}