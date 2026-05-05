using UnityEngine;

public class FindAudioSources : MonoBehaviour
{
    void Start()
    {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in sources)
        {
            Debug.Log(
                "Audio Source found on: " + source.gameObject.name +
                " | Clip: " + (source.clip != null ? source.clip.name : "No clip") +
                " | PlayOnAwake: " + source.playOnAwake +
                " | Loop: " + source.loop +
                " | Active: " + source.gameObject.activeInHierarchy
            );
        }
    }
}