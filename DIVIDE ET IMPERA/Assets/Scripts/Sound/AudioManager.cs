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



    private void FadeBGM(float timeToFade)
    {
        StopAllCoroutines();

        StartCoroutine(FadeTrackCoroutine(timeToFade));

    }


    public void FadeBGM2(float timeToFade)
    {
        float timeElapsed = 0;

        while (timeElapsed < timeToFade)
        {
            _bgmMixer.SetFloat("BGMVolume", Mathf.Log10(timeToFade - timeElapsed) * 20);

            timeElapsed += Time.deltaTime;

        }

    }

    private IEnumerator FadeTrackCoroutine(float timeToFade)
    {
        float timeElapsed = 0;

        while(timeElapsed < timeToFade)
        {
            _bgmMixer.SetFloat("BGMVolume", Mathf.Log10(timeToFade-timeElapsed) * 20);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

    }


    private void Start()
    {
        _instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {

            FadeBGM(5);

        }
    }


}
