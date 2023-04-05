using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFXComponent : MonoBehaviour
{
    // probando singleton
    private static SFXComponent _instance;
    public static SFXComponent Instance { get { return _instance; } }


    [SerializeField]
    private AudioSource[] _sfx;
    [SerializeField]
    private AudioSource[] _objectsSFX;

    /// <summary>
    /// ---- TUTO DE ARRAY DE SFX---
    /// de momento no hay nada oop
    /// 
    /// </summary>


    void Awake()
    {
        _instance = this;
    }

    #region metodos especificos

    public void SoltarParteSFX()
    {
        if (_sfx[0] != null)
        {
            _sfx[0].Play();
        }
        
    }

    public void RecogerParteSFX()
    {
        if (_sfx[1] != null)
        {
            _sfx[1].Play();
        }

    }

    public void PalancaSFX()
    {
        if (_sfx[2] != null)
        {
            _sfx[2].Play();
        }
    }

    #endregion

   
    // metodo general
    public void SFXPlayer(int i)
    {
        if (_sfx[i] != null)
        {
            _sfx[i].Play();
        }
    }

    public void SFXObjects(int i)
    {
        if (_objectsSFX[i] != null)
        {
            Debug.Log(_objectsSFX[i] != null);
            _objectsSFX[i].Play();
        }
        else
        {

        }
    }
}
