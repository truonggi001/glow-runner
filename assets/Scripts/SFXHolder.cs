using UnityEngine;

/// <summary>
/// Holds SFX audio clips for PlayerController. Attached to child of Player.
/// </summary>
public class SFXHolder : MonoBehaviour
{
    public AudioClip jumpClip;
    public AudioClip dashClip;
    public AudioClip deathClip;
    public AudioClip shardClip;

    private AudioSource source;

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.volume = 0.6f;
        source.playOnAwake = false;
    }

    public void Play(AudioClip clip)
    {
        if (clip != null && source != null)
            source.PlayOneShot(clip);
    }
}