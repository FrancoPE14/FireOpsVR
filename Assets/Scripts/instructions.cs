using UnityEngine;

public class FollowPlayerView : MonoBehaviour
{
    public Transform playerCamera;
    public float distance = 1.2f;
    public Vector3 offset = new Vector3(0f, -0.1f, 0f);
    public bool followEveryFrame = true;

    void LateUpdate()
    {
        if (!followEveryFrame) return;
        MoveInFrontOfPlayer();
    }

    public void MoveInFrontOfPlayer()
    {
        if (playerCamera == null) return;

        Vector3 forward = playerCamera.forward;
        forward.y = 0f;
        forward.Normalize();

        if (forward.sqrMagnitude < 0.001f)
            forward = playerCamera.forward;

        transform.position = playerCamera.position + forward * distance + offset;

        Vector3 lookDir = transform.position - playerCamera.position;

        if (lookDir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(-lookDir.normalized);
    }
}