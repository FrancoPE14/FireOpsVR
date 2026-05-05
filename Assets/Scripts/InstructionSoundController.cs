using UnityEngine;
using System.Collections;

public class InstructionSoundController : MonoBehaviour
{
    public AudioSource instructionAudio;
    public float delay = 1f;

    private Coroutine playRoutine;

    private void OnEnable()
    {
        // Start delayed playback when instruction appears
        playRoutine = StartCoroutine(PlayWithDelay());
    }

    private void OnDisable()
    {
        // Optional: stop if instruction disappears early
        if (playRoutine != null)
            StopCoroutine(playRoutine);

        if (instructionAudio.isPlaying)
            instructionAudio.Stop();
    }

    IEnumerator PlayWithDelay()
    {
        yield return new WaitForSeconds(delay);

        if (instructionAudio != null)
        {
            instructionAudio.Play();
        }
    }
}