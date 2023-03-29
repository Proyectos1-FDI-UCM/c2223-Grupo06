using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSFXManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] _sfx;

    // metodo general
    public void SFXPlayer(int i)
    {
        if (_sfx[i] != null)
        {
            _sfx[i].Play();
        }
    }
}
