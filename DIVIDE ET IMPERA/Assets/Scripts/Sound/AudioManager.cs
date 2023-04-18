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

    // probando singleton
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    #region parameters

    float _startingVolume;

    #endregion

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

    public void FadeBGM(float timeToFade)
    {
        StopAllCoroutines();

        StartCoroutine(FadeTrackCoroutine(timeToFade));

    }


    public void FadeBGM2(float timeToFade)
    {
        float timeElapsed = 0, i;

        while (timeElapsed < timeToFade)
        {
            _bgmMixer.SetFloat("BGMVolume", 4*(_startingVolume - Mathf.Log10(timeElapsed / timeToFade) * 20)); //

            Debug.Log("uwu " + (Mathf.Log10(timeToFade / timeElapsed) * 20));
            // timeElapsed/timeToFade

            timeElapsed += Time.deltaTime;

        }

    }

    private IEnumerator FadeTrackCoroutine(float timeToFade)
    {

        float timeElapsed = 0.05f, i;

        while (timeElapsed < timeToFade)
        {
            _bgmMixer.SetFloat("BGMVolume",  (_startingVolume - 20 - Mathf.Log10(timeElapsed / timeToFade) * 20)); //

            Debug.Log("uwu " + (_startingVolume - 20 - Mathf.Log10(timeElapsed / timeToFade) * 20));
            // timeElapsed/timeToFade

            timeElapsed += Time.deltaTime;

            yield return null;

        }


        /*
        float timeElapsed = 0;

        while(timeElapsed < timeToFade)
        {
            _bgmMixer.SetFloat("BGMVolume", Mathf.Log10(timeToFade-timeElapsed) * 20);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
        */
    }


    private void Start()
    {
        _instance = this;
        _startingVolume = GetVolume();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {

            FadeBGM(5);

        }
    }


}
