using UnityEngine;
using Services;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
    public AudioClip[] clips;

    public RangedFloat volume;

    [MinMaxRange(0, 2)]
    public RangedFloat pitch;

    [Range(0f,1f)]
    public float spatialBlend = 1f; 

    public override void Play(Vector3 position)
    {
        if (clips.Length == 0) return;

        
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        float volume_ = Random.Range(volume.minValue, volume.maxValue);
        float pitch_ = Random.Range(pitch.minValue, pitch.maxValue);
        ServiceLocator.Instance.Get<AudioManager>().PlaySfxAtLocation(clip, position, volume_, pitch_, spatialBlend);
    }
}