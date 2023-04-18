using UnityEngine;
using static GameManager;

public class BGMComponent : MonoBehaviour
{


    [SerializeField]
    private AudioSource[] _bgm;
    [SerializeField]
    private AudioSource _ambience;

    #region properties
    private int _currentBGM;
    private int _nextBGM;

    [SerializeField]
    private bool _playAmbience;
    [SerializeField]
    private bool _canPlay;
    public bool CanPlay { get { return _canPlay; } set { _canPlay = value; } }

    #endregion

    /// <summary>
    /// TUTORIAL/CHULETA DE TEMAZOS
    /// 
    /// 0 --> no bgm
    /// 1 --> skeleton waltz
    /// 2 --> jazz perhaps
    /// 3 --> dolphin
    /// </summary>

    #region methods
    public void PlayBGM(int i)
    {
        // si existe en el array lo pone
        if (_bgm[i] != null)
        {
            _bgm[i].Play();
        }
    }
    public void StopBGM(int i)
    {
        // si esta sonando lo para
        if (_bgm[i].isPlaying)
        {
            _bgm[i].Stop();
        }
    }
    public void PlayAmbience()
    {
        if (_ambience != null)
            _ambience.Play();
    }
    public void StopAmbience()
    {
        if (_ambience.isPlaying)
            _ambience.Stop();
    }

    #endregion


    private void BGMManager()
    {
        if (Instance != null)
        {
            switch (Instance.CurrentState) // Diferentes comportamientos según estado al que se entra
            {
                case GameStates.START:                       //     *MENÚ INICIAL*
                    _nextBGM = 2;
                    _playAmbience = false;
                    break;
                case GameStates.INTRO:
                    _nextBGM = 4;
                    _playAmbience = true;
                    break;
                case GameStates.GAME:                        //     *JUEGO*
                    _nextBGM = 1;
                    _playAmbience = true;
                    break;
                case GameStates.PAUSE:                       //     *PAUSA*
                    _nextBGM = 3;
                    _playAmbience = false;
                    break;
                case GameStates.GAMEOVER:                    //     *FIN DEL JUEGO*
                    _nextBGM = 3;
                    _playAmbience = false;
                    break;
                case GameStates.SCORE:
                    _nextBGM = 4;
                    _playAmbience = false;
                    break;
                case GameStates.LEVELSELECTOR:
                    _nextBGM = 3;
                    _playAmbience = false;
                    break;
                case GameStates.CONTROLES:                   //     *CONTROLES*
                    _nextBGM = 3;
                    _playAmbience = false;
                    break;
                case GameStates.OPCIONES:                   //      *OPCIONES*
                    _nextBGM = 3;
                    _playAmbience = false;
                    break;
            }
        }
    }

    private void BGMPlayer(int currentBGM)
    {
        PlayBGM(_currentBGM);
        _canPlay = false;
    }

    private void Start()
    {
        _currentBGM = 0;
        _playAmbience = false;
    }

    private void Update()
    {
        BGMManager();
        if (_currentBGM != _nextBGM)
        {
            StopBGM(_currentBGM);
            _currentBGM = _nextBGM;
            if(_currentBGM >= 0)
            {
                
                Debug.Log("heh");
                PlayBGM(_currentBGM);
            }
            
            if (_playAmbience && !_ambience.isPlaying)
            {
                PlayAmbience();
            }
            else if (!_playAmbience)
            {
                _ambience.Stop();
            }
        }

    }

   
}
