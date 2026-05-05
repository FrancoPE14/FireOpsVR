using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoseAttachConstraint : MonoBehaviour
{
    public Rigidbody extinguisherBody;
    public Transform hoseAttachPoint;

    [Header("Reach Limits")]
    public float maxLinearReach = 0.0f;

    private Rigidbody rb;
    private ConfigurableJoint joint;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (extinguisherBody == null)
        {
            Debug.LogError("HoseAttachConstraint: extinguisherBody is not assigned.");
            return;
        }

        SetupJoint();
    }

    void SetupJoint()
    {
        joint = GetComponent<ConfigurableJoint>();
        if (joint == null)
            joint = gameObject.AddComponent<ConfigurableJoint>();

        joint.connectedBody = extinguisherBody;
        joint.autoConfigureConnectedAnchor = false;

        if (hoseAttachPoint != null)
            joint.connectedAnchor = extinguisherBody.transform.InverseTransformPoint(hoseAttachPoint.position);
        else
            joint.connectedAnchor = Vector3.zero;

        joint.anchor = Vector3.zero;

        // Keep position tightly constrained
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit linearLimit = new SoftJointLimit();
        linearLimit.limit = maxLinearReach;
        joint.linearLimit = linearLimit;

        // Do NOT leave all angular motion fully free
        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit lowX = new SoftJointLimit();
        lowX.limit = 35f;
        joint.lowAngularXLimit = lowX;

        SoftJointLimit highX = new SoftJointLimit();
        highX.limit = 35f;
        joint.highAngularXLimit = highX;

        SoftJointLimit yLimit = new SoftJointLimit();
        yLimit.limit = 45f;
        joint.angularYLimit = yLimit;

        SoftJointLimit zLimit = new SoftJointLimit();
        zLimit.limit = 45f;
        joint.angularZLimit = zLimit;

        joint.breakForce = Mathf.Infinity;
        joint.breakTorque = Mathf.Infinity;

        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }
}