using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PinFollowSocket : MonoBehaviour
{
    public Transform socket;
    public XRGrabInteractable pinGrab;

    private bool isRemoved = false;

    private void Start()
    {
        if (pinGrab == null)
            pinGrab = GetComponent<XRGrabInteractable>();

        if (pinGrab != null)
            pinGrab.selectEntered.AddListener(OnPinGrabbed);
    }

    private void LateUpdate()
    {
        if (!isRemoved && socket != null)
        {
            transform.position = socket.position;
            transform.rotation = socket.rotation;
        }
    }

    private void OnPinGrabbed(SelectEnterEventArgs args)
    {
        isRemoved = true;
    }
}