using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMComponent : MonoBehaviour
{

    [SerializeField]
    private AudioSource[] _bgm;
    [SerializeField]
    private AudioSource _ambience;

    #region properties
    private int _currentBGM;
    [SerializeField]
    private bool _canPlay;
    public bool CanPlay { get { return _canPlay; } set { _canPlay = value; } }

    #endregion

    /// <summary>
    /// TUTORIAL/CHULETA DE TEMAZOS
    /// 
    /// 0 --> "Game Over"
    /// 1 --> "Major Loss"
    /// 2 --> "Never Suerender"
    /// 3 --> "Skeleton Waltz"
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
        if(_ambience.isPlaying)
        _ambience.Stop();
    }

    public void StopAllBGM()
    {
        // para todo lo que este sonando
    }
    #endregion


    private void BGMManager()
    {
        // pretendo pasarlo a un switch cuando aprenda como hacerlo xd
        if (GameManager.Instance.CurrentState == GameManager.GameStates.START)
        {
            _currentBGM = 3;
        }
        else if(GameManager.Instance.CurrentState == GameManager.GameStates.GAME)
        {
            _currentBGM = 0;
        }
        else if(GameManager.Instance.CurrentState == GameManager.GameStates.PAUSE)
        {
            _currentBGM = 3;
        }
        else if(GameManager.Instance.CurrentState == GameManager.GameStates.GAMEOVER)
        {
            _currentBGM = 0;
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameStates.SCORE)
        {
            _currentBGM = 2;
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameStates.LEVELSELECTOR)
        {
            _currentBGM = 2;
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameStates.CONTROLES)
        {
            _currentBGM = 2;
        }

    }

    private void BGMPlayer (int currentBGM)
    {
        PlayBGM(_currentBGM);
        _canPlay = false;
    }

    private void Start()
    {
        _canPlay = true;
    }

    private void Update()
    {
        BGMManager();

        if (_canPlay)
        {
            BGMPlayer(_currentBGM);
        }
    }
}
