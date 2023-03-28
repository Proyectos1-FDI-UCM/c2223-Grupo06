using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXComponent : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] _sfx;

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
}
