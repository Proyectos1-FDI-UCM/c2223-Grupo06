using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public AudioMixer _bgmMixer;

    public void SetVolume(float _sliderValue)
    {
        // representa el valor del slider de manera logaritmica para que se haga bien la conversion; 
        // lo convierte en valores entre [-80, 0] pero en escala logaritmica
        _bgmMixer.SetFloat("BGMVolume", Mathf.Log10(_sliderValue) * 20);
    }

}
