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
    [SerializeField]
    private float _sliderValueBGM = 0.5f;
    [SerializeField]
    private float _sliderValueSFX = 0.5f; 
    [SerializeField]
    private float _sliderValueAmbience = 0.5f;
    int _whichAudioMixer;

    #endregion

    // probando singleton
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    /// <summary>
    /// Guarda el valor del slider del menu de opciones de cada uno de los sliders, siendo
    /// _value el valor del slider e i el mixer correspondiente:
    /// 0 --> BGM
    /// 1 --> SFX
    /// 2 --> Ambience
    /// </summary>
    /// <param name="_value"></param>
    /// <param name="i"></param>
    private void SaveSliderValue(float _value, int i)
    {
        switch (i)
        {
            case 0:
                _sliderValueBGM = _value;
                break;
            case 1:
                _sliderValueSFX = _value;
                break ;
            case 2:
                _sliderValueAmbience = _value;
                break;
        }
    }

    /// <summary>
    /// Setea el valor del slider guardado dependiendo del indice:
    /// 0 --> BGM
    /// 1 --> SFX
    /// 2 --> Ambience
    /// </summary>
    /// <param name="i"></param>
    public float SetSliderValue(int i)
    {
        float auxValue = 0.5f;
        switch (i)
        {
            case 0:
                auxValue = _sliderValueBGM;
                break;
            case 1:
                auxValue = _sliderValueSFX;
                break;
            case 2:
                auxValue = _sliderValueAmbience;
                break;
        }

        return auxValue;
    }
    public void SetBGMVolume(float _sliderValue)
    {
        // representa el valor del slider de manera logaritmica para que se haga bien la conversion; 
        // lo convierte en valores entre [-80, 0] pero en escala logaritmica
        _bgmMixer.SetFloat("BGMVolume", Mathf.Log10(_sliderValue) * 20);

        // guarda el valor del slider
        SaveSliderValue(_sliderValue, 0);
    }

    public void SetSFXVolume(float _sliderValue)
    {
        _sfxMixer.SetFloat("SFXVolume", Mathf.Log10(_sliderValue) * 20);

        // guarda el valor del slider
        SaveSliderValue(_sliderValue, 1);
    }
    public void SetAmbienceVolume(float _sliderValue)
    {
        _ambienceMixer.SetFloat("AmbienceMixer", Mathf.Log10(_sliderValue) * 20);

        // guarda el valor del slider
        SaveSliderValue(_sliderValue, 2);
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
            // añade al tiempo
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
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _beforeFadeVolume = GetVolume();
    }

    private void Update()
    {
    }
}
