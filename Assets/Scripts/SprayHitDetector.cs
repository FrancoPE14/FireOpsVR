using UnityEngine;

public class SprayHitDetector : MonoBehaviour
{
    public SprayController sprayController;
    public float hitAmountPerCollision = 0.08f;

    private void OnParticleCollision(GameObject other)
    {
        ExtinguishableFire fire = other.GetComponent<ExtinguishableFire>();

        if (fire == null)
            fire = other.GetComponentInParent<ExtinguishableFire>();

        if (fire == null)
            return;

        fire.RegisterSprayHit(hitAmountPerCollision);
    }
}