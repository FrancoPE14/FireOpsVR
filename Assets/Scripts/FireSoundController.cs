using UnityEngine;

public class FireSoundController : MonoBehaviour
{
    private AudioSource fireAudio;

    private void Awake()
    {
        fireAudio = GetComponent<AudioSource>();

        if (fireAudio == null)
        {
            Debug.LogError("No AudioSource found on Fire object.");
        }
    }

    private void OnEnable()
    {
        if (fireAudio != null && !fireAudio.isPlaying)
        {
            fireAudio.Play();
        }
    }

    private void OnDisable()
    {
        if (fireAudio != null && fireAudio.isPlaying)
        {
            fireAudio.Stop();
        }
    }
}