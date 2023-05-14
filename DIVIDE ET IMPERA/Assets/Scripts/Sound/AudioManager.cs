using System.Collections;
//using UnityEditorInternal;
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

    [SerializeField] private float _beforeFadeVolume;
    private float _sliderValueBGM;
    public float SliderValueBGM { get { return _sliderValueBGM; } }
    private float _sliderValueSFX;
    private float _sliderValueAmbience;
    int _whichAudioMixer;

    #endregion

    // probando singleton
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }


    public float GetVolume()
    {
        float i;
        _bgmMixer.GetFloat("BGMVolume", out i);
        return i;
    }


    #region Fade
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
            _bgmMixer.SetFloat("BGMVolume", (GetVolume() + Mathf.Log10(k / timeToFade)));

            k -= Time.deltaTime;
            // añade al tiempo
            timeElapsed += Time.deltaTime;

            // cosas de corrutinas
            yield return null;
        }
    }
    #endregion


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
        //SetUpAllVolumes();
        _beforeFadeVolume = GetVolume();
    }
}
