using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TextMeshProUGUI volumeText = null;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        LoadValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolumeSlider(float volume)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        volumeText.text = volume.ToString("0.0");
    }

    public void SaveVolumeButton()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        float volumeVal = volumeSlider.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeVal);
        LoadValues();
    }

    private void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }
}
