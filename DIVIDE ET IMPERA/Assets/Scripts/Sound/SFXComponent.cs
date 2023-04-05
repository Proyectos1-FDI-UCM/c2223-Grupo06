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
    /// ---- TUTO DE ARRAY DE SFX--- (pone donde estan porque querré cambiarlo en el futuro para limpieza)
    /// de momento no hay nada oop
    /// 0 --> soltar parte
    ///     en el player manager
    /// 1 --> recuperar parte
    ///     en el player manager
    /// 2 --> andar
    ///     en el input controller
    /// 3 --> saltar
    ///     en el jump component
    /// 4 --> lanzar brazo
    ///     en el throw component
    /// 5 --> chutar
    ///     en el throw component
    /// 6 --> coger objeto
    ///     en el playermanager (mirar desde el input (x))
    /// 7 --> soltar objeto
    ///     en el playermanager (mirar desde el input (x))
    /// 8 --> caida con daño
    ///     en el fall damage
    /// 9 --> caida sin daño
    ///     en el fall damage
    /// </summary>


    void Awake()
    {
        _instance = this;
    }

    // metodos generales player
    public void SFXPlayer(int i)
    {
        if (_sfx[i] != null)
        {
            _sfx[i].Play();
        }
    }
    public void SFXPlayerStop(int i)
    {
        if (_sfx[i] != null)
        {
            _sfx[i].Stop();
        }
    }


    // metodos generales de objetos
    public void SFXObjects(int i)
    {
        if (_objectsSFX[i] != null)
        {
            _objectsSFX[i].Play();
        }
    }
    public void SFXObjectsStop(int i)
    {
        if (_objectsSFX[i] != null)
        {
            _objectsSFX[i].Stop();
        }
    }


    // te dice si el sonido esta sonando o no
    public bool isPlayingSFX (int i)
    {
        return _sfx[i].isPlaying;
    }
}
