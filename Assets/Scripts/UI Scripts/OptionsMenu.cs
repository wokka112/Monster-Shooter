using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer effectsAudioMixer;
    public AudioMixer musicAudioMixer;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionStrings = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string resolutionStr = resolutions[i].width + " x " + resolutions[i].height;
            resolutionStrings.Add(resolutionStr);

            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionStrings);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetEffectsVolume(float volume)
    {
        effectsAudioMixer.SetFloat("volume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioMixer.SetFloat("volume", volume);
    }

    public void SetFullScreen(bool setFullscreen)
    {
        Screen.fullScreen = setFullscreen;
    }
}
