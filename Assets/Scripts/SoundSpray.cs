using UnityEngine;

public class SpraySoundController : MonoBehaviour
{
    public AudioSource sprayAudio;

    public void StartSpraySound()
    {
        if (sprayAudio != null && !sprayAudio.isPlaying)
        {
            sprayAudio.Play();
        }
    }

    public void StopSpraySound()
    {
        if (sprayAudio != null && sprayAudio.isPlaying)
        {
            sprayAudio.Stop();
        }
    }
}