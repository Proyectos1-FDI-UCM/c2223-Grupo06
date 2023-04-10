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
    [SerializeField] 
    private AudioSource _yippie;

    /// <summary>
    /// ---- TUTO DE ARRAY DE SFX DE TIMMY--- 
    /// 0 --> soltar parte [en el player manager]
    /// 1 --> recuperar parte [en el player manager]
    /// 2 --> andar [en el input controller]
    /// 3 --> saltar [en el jump component]
    /// 4 --> lanzar brazo [en el throw component]
    /// 5 --> chutar [en el throw component]
    /// 6 --> coger objeto [en el playermanager (mirar desde el input (x))]
    /// 7 --> soltar objeto [en el playermanager (mirar desde el input (x))]
    /// 8 --> caida con daño [en el fall damage]
    /// 9 --> caida sin daño (?) [en el fall damage]
    /// 10 --> brazo contra el muro [arm component] (+ 3 de obj)
    /// 
    /// </summary>

    /// <summary>
    /// ---- TUTO DE ARRAY DE SFX DE OBJETOS--- 
    /// 0 --> palanca [en el palanca component Activar()]
    /// 1 --> muelle [en el spring component]
    /// 2 --> bola contra el muro [ball component]
    /// 3 --> bola contra el muro (+ 10 de player)
    /// 4 --> abrir puerta con llave [en el door component]
    /// 
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


    // te dice si el sonido esta sonando o no en el player
    public bool isPlayingSFX (int i)
    {
        return _sfx[i].isPlaying;
    }

    // te dice si el sonido esta sonando o no en el player
    public bool isPlayingSFXObjects(int i)
    {
        return _objectsSFX[i].isPlaying;
    }

    public void PlayYippie()
    {
        _yippie.Play();
    }
}
