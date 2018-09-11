using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXPlayer : MonoBehaviour {
    public DisplayCaptions displayCaptions;
    public SoundProfile soundProfile;
    public AnimationCurve clipSelect = new AnimationCurve(new Keyframe(0f,0f), new Keyframe(1f, 1f));
    public IntRangeVariable Counter;
    public AnimationCurve clipPitch = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, -1f));
    public AnimationCurve clipVolume = new AnimationCurve(new Keyframe(0f, 0.25f), new Keyframe(1f, 1f));
    public FloatRangeVariable Size;
    public FloatRangeVariable IdleTime;
    private AudioSource audioSource;
    public enum VolumePitchMode { SizeCounter, CounterSize, CounterCounter, CounterIdle, IdleCounter, IdleIdle, IdleSize, SizeSize, SizeIdle }
    public VolumePitchMode PitchVolume;
    public bool autoplay;
    public IntRangeVariable cycle;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
    private float VolumePercent
    {
        get
        {
            switch (PitchVolume)
            {
                case VolumePitchMode.CounterCounter:
                case VolumePitchMode.IdleCounter:
                case VolumePitchMode.SizeCounter:
                    return Counter.Percent;
                case VolumePitchMode.CounterIdle:
                case VolumePitchMode.IdleIdle:
                case VolumePitchMode.SizeIdle:
                    return IdleTime.Percent;
                case VolumePitchMode.CounterSize:
                case VolumePitchMode.IdleSize:
                case VolumePitchMode.SizeSize:
                    return Size.Percent;
            }
            return 0f;
        }
    }

    public void PlaySound()
    {
        if (cycle != null && cycle.RuntimeValue < cycle.MaxValue)
        {
            Debug.Log("Skip sound: " + cycle.RuntimeValue);
            cycle.RuntimeValue++;
            return;
        }
        else if(cycle != null)
            cycle.RuntimeValue = cycle.MinValue;
        switch (PitchVolume)
        {
            case VolumePitchMode.CounterCounter:
                audioSource.pitch = clipPitch.Evaluate(Counter.Percent);
                audioSource.volume = clipVolume.Evaluate(Counter.Percent);
                break;
            case VolumePitchMode.CounterIdle:
                audioSource.pitch = clipPitch.Evaluate(Counter.Percent);
                audioSource.volume = clipVolume.Evaluate(IdleTime.Percent);
                break;
            case VolumePitchMode.CounterSize:
                audioSource.pitch = clipPitch.Evaluate(Counter.Percent);
                audioSource.volume = clipVolume.Evaluate(Size.Percent);
                break;
            case VolumePitchMode.IdleCounter:
                audioSource.pitch = clipPitch.Evaluate(IdleTime.Percent);
                audioSource.volume = clipVolume.Evaluate(Counter.Percent);
                break;
            case VolumePitchMode.IdleIdle:
                audioSource.pitch = clipPitch.Evaluate(IdleTime.Percent);
                audioSource.volume = clipVolume.Evaluate(IdleTime.Percent);
                break;
            case VolumePitchMode.IdleSize:
                audioSource.pitch = clipPitch.Evaluate(IdleTime.Percent);
                audioSource.volume = clipVolume.Evaluate(Size.Percent);
                break;
            case VolumePitchMode.SizeCounter:
                audioSource.pitch = clipPitch.Evaluate(Size.Percent);
                audioSource.volume = clipVolume.Evaluate(Counter.Percent);
                break;
            case VolumePitchMode.SizeIdle:
                audioSource.pitch = clipPitch.Evaluate(Size.Percent);
                audioSource.volume = clipVolume.Evaluate(IdleTime.Percent);
                break;
            case VolumePitchMode.SizeSize:
                audioSource.pitch = clipPitch.Evaluate(Size.Percent);
                audioSource.volume = clipVolume.Evaluate(Size.Percent);
                break;
        }
        SoundProfile.SoundEffect soundEffect = soundProfile.GetSound();
        //SoundProfile.SoundEffect soundEffect = soundProfile.GetSound(Size.Percent, Counter.Percent, IdleTime.Percent);
        if (soundEffect != null)
        {
            audioSource.PlayOneShot(soundEffect.clip);
            if (displayCaptions != null)
                displayCaptions.AddCaption(soundProfile.CaptionColor, VolumePercent, soundProfile.before + soundEffect.caption + soundProfile.after);
        }
        else
            Debug.LogWarning("sound effect not found " + name);
        //audioSource.PlayOneShot(soundProfile.Clip(Size.Percent, Counter.Percent, IdleTime.Percent));
            

    }
	// Update is called once per frame
	void Update () {
        if (autoplay && !audioSource.isPlaying)
            PlaySound();
	}
}
