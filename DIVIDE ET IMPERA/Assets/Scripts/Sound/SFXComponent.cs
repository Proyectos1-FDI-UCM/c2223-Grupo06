using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXComponent : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] _sfx;

    #region Soltar y Recoger partes

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

    #endregion
}
