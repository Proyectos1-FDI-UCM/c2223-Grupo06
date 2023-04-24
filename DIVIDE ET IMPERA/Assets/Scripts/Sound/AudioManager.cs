using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region References
    [SerializeField]
    private AudioMixer _bgmMixer;
    [SerializeField]
    private AudioMixer _sfxMixer;
    [SerializeField]
    private AudioMixer _ambienceMixer;
    #endregion

    #region Properties

    [SerializeField]
    private float _beforeFadeVolume;
    private float _sliderValue;

    #endregion

    // probando singleton
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public void SaveSliderValue()
    {

    }

    public void SetBGMVolume(float _sliderValue)
    {
        // representa el valor del slider de manera logaritmica para que se haga bien la conversion; 
        // lo convierte en valores entre [-80, 0] pero en escala logaritmica
        _bgmMixer.SetFloat("BGMVolume", Mathf.Log10(_sliderValue) * 20);
    }

    public void SetSFXVolume(float _sliderValue)
    {
        _sfxMixer.SetFloat("SFXVolume", Mathf.Log10(_sliderValue) * 20);
    }
    public void SetAmbienceVolume(float _sliderValue)
    {
        _ambienceMixer.SetFloat("AmbienceMixer", Mathf.Log10(_sliderValue) * 20);
    }

    public float GetVolume()
    {
        float i;
        _bgmMixer.GetFloat("BGMVolume", out i);
        return i;
    }

    public void SetVolumeAfterFade()
    {
        _bgmMixer.SetFloat("BGMVolume", _beforeFadeVolume);
    }

    public void FadeBGM(float timeToFade)
    {
        // para todas las corrutinas (no se si esta bien)
        StopAllCoroutines();

        // empieza la corrutina para el fade
        StartCoroutine(FadeTrackCoroutine(timeToFade));

        
    }

    private IEnumerator FadeTrackCoroutine(float timeToFade)
    {
        _beforeFadeVolume = GetVolume();

        float timeElapsed = 0f, k = timeToFade;

        // while para que lo haga respecto al tiempo
        while (timeElapsed < timeToFade)
        {
            // calcula y cambia el volumen (log10)
            _bgmMixer.SetFloat("BGMVolume",  (GetVolume() + Mathf.Log10(k/timeToFade)));

            k -= Time.deltaTime;
            // a�ade al tiempo
            timeElapsed += Time.deltaTime;

            // cosas de corrutinas
            yield return null;

        }
    }


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _instance = this;

        _beforeFadeVolume = GetVolume();

    }

    private void Update()
    {
    }


}
