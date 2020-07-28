using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public struct AudioObject
    {
        public string identifier;
        public AudioClip clip;
    }

    [SerializeField]
    private AudioSource source = null;
    [SerializeField]
    private AudioClip backgroundMusic = null;
    [SerializeField]
    private bool muteBackground;

    [Range(0.0f, 1.0f)]
    public float masterVolume = 1.0f;
    [Range(0.0f, 1.0f)]
    public float backgroundVolume = 1.0f;
    [Range(0.0f, 1.0f)]
    public float effectsVolume = 1.0f;
    public AudioObject[] audioClips;

    private Dictionary<string, AudioClip> idToClip;

    private void Start()
    {
        //source = this.GetComponent<AudioSource>();
        OnValidate();
        PlayBackgroundMusic();
    }

    public void OnValidate()
    {
        if (masterVolume > 1.0f) masterVolume = 1.0f;
        if (masterVolume < 0.0f) masterVolume = 0.0f;
        if (backgroundVolume > 1.0f) backgroundVolume = 1.0f;
        if (backgroundVolume < 0.0f) backgroundVolume = 0.0f;
        if (effectsVolume > 1.0f) effectsVolume = 1.0f;
        if (effectsVolume < 0.0f) effectsVolume = 0.0f;

        idToClip = new Dictionary<string, AudioClip>();
        foreach (AudioObject obj in audioClips)
        {
            idToClip.Add(obj.identifier, obj.clip);
        }
    }

    public void setMasterVolume(float volume)
    {
        masterVolume = volume;
        PlayBackgroundMusic();
    }
    public void setEffectsVolume(float volume)
    {
        effectsVolume = volume;
    }
    public void setBackgroundVolume(float volume)
    {
        backgroundVolume = volume;
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        source.clip = backgroundMusic;
        source.volume = masterVolume;
        if (!muteBackground) source.PlayOneShot(backgroundMusic, backgroundVolume);
        else source.Stop();
    }
    public void MuteBackgroundMusic()
    {
        muteBackground = true;
        PlayBackgroundMusic();
    }
    public void UnMuteBackgroundMusic()
    {
        muteBackground = false;
        PlayBackgroundMusic();
    }
    public void PlayEffect(string id)
    {
        AudioClip thisClip = idToClip[id];
        if (null == thisClip)
        {
            Debug.LogError("AUDIO MANAGER: No clip with id: " + id);
            return;
        }
        source.PlayOneShot(thisClip, effectsVolume);
    }

    public void PlayEffectFromSource(string id, AudioSource aSource)
    {
        AudioClip thisClip = idToClip[id];
        if (null == thisClip)
        {
            Debug.LogError("AUDIO MANAGER: No clip with id: " + id);
            return;
        }
        aSource.PlayOneShot(thisClip, effectsVolume);
    }
}