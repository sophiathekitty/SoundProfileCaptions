using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoundProfile : ScriptableObject {

    public Color CaptionColor;
    public string before, after;
    public SoundScale[] sounds;
    public AnimationCurve clipSelect = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

    public enum SelectMode { CounterSize, SizeCounter, IdleRandom, CounterRandom, SizeRandom  }
    public SelectMode mode;

    public AudioClip Clip(float size, float counter, float idle)
    {
        switch (mode)
        {
            case SelectMode.CounterSize:
                return Sound(counter).Clip(size);
            case SelectMode.SizeCounter:
                return Sound(size).Clip(counter);
            case SelectMode.IdleRandom:
                return Sound(idle).RandomClip();
            case SelectMode.CounterRandom:
                return Sound(counter).RandomClip();
            case SelectMode.SizeRandom:
                return Sound(size).RandomClip();
        }
        return null;
    }
    public SoundEffect GetSound(float size, float counter, float idle)
    {
        switch (mode)
        {
            case SelectMode.CounterSize:
                return Sound(counter).Sound(size);
            case SelectMode.SizeCounter:
                return Sound(size).Sound(counter);
            case SelectMode.IdleRandom:
                return Sound(idle).RandomSound();
            case SelectMode.CounterRandom:
                return Sound(counter).RandomSound();
            case SelectMode.SizeRandom:
                return Sound(size).RandomSound();
        }
        return null;
    }

    private SoundScale Sound(float percent)
    {
        int i = Mathf.RoundToInt(Mathf.Lerp(0, sounds.Length-1, clipSelect.Evaluate(percent)));
        return sounds[i];
    }

    [System.Serializable]
	public class SoundScale
    {
        public AudioClip[] clips;
        public SoundEffect[] soundEffects;
        public AnimationCurve clipSelect = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        public AudioClip Clip(float percent)
        {
            return clips[Mathf.RoundToInt(Mathf.Lerp(0, clips.Length - 1, clipSelect.Evaluate(percent)))];
        }
        public AudioClip RandomClip()
        {
            return clips[Mathf.RoundToInt(Mathf.Lerp(0, clips.Length - 1, clipSelect.Evaluate(Random.Range(0f, 1f))))];
        }
        public SoundEffect Sound(float percent)
        {
            return soundEffects[Mathf.RoundToInt(Mathf.Lerp(0, soundEffects.Length - 1, clipSelect.Evaluate(percent)))];
        }
        public SoundEffect RandomSound()
        {
            return soundEffects[Mathf.RoundToInt(Mathf.Lerp(0, soundEffects.Length - 1, clipSelect.Evaluate(Random.Range(0f, 1f))))];
        }
    }
    [System.Serializable]
    public class SoundEffect
    {
        public string caption;
        public AudioClip clip;
    }
}
