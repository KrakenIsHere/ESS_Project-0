using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsController : MonoBehaviour
{
    private static AudioSettingsController controller;

    public AudioMixer mixer;

    // Use this for initialization
    void Awake()
    {
        if (controller == null)
        {
            controller = this;
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetMasterAudio(float masterLvl)
    {
        mixer.SetFloat("MasterVol", masterLvl);
    }

    public void SetSFXAudio(float sfxLvl)
    {
        mixer.SetFloat("SfxVol", sfxLvl);
    }

    public void SetMusicAudio(float musicLvl)
    {
        mixer.SetFloat("MusicVol", musicLvl);
    }

    public void SetVoiceAudio(float voiceLvl)
    {
        mixer.SetFloat("VoiceVol", voiceLvl);
    }

    public void SetAmbianceAudio(float ambianceLvl)
    {
        mixer.SetFloat("AmbianceVol", ambianceLvl);
    }
}
