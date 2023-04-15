using UnityEngine;

public abstract class AudioEvent : ScriptableObject
{
    public abstract void Play(Vector3 position);

    public abstract void Preview(AudioSource previewer);
}