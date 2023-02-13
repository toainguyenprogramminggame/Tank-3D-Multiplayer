using Tank3DMultiplayer;
using Unity.Netcode;
using UnityEngine;

public class CameraFollow : NetworkBehaviour
{
    GameObject cameraHolder;

    private void Start()
    {
        if(!IsOwner) return;
        cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder");

    }
    private void LateUpdate()
    {
        if (!IsOwner) return;

        if (cameraHolder != null)
        {
            // Update position for camera holder
            cameraHolder.transform.position = transform.position + new Vector3(0f, ConstValue.OFFSET_CAMERA_Y, 0f);
        }
        else
        {
            cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder");
        }
    }

}
