using UnityEngine;

public class PalancaComponent : MonoBehaviour
{

    #region Referencias
    private InputController _inputController;
    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _puerta;
    #endregion

    #region Properties
    private bool _palanca;
    private bool _brazoConectado = false;
    public bool _validPalancaHitbox; //no tengo ni idea de por que se llama asi, he copiado el nombre que utilizaba antes porque no se me ocurre otro xd
    #endregion

    #region Métodos

    // activa o desactiva la palanca dependiendo de su estado anterior
    private bool Activar()
    {
        bool _lvr = !_palanca;
        _puerta.SetActive(_palanca);
        _palanca= !_palanca;
        return _lvr;
    }
    #endregion



    private void Start()
    {
        _inputController = _player.GetComponent<InputController>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        // si se ha pulsado la E y el brazo está conectado
        if (_inputController.Interactuar && _brazoConectado)
        {
            _palanca = Activar();
        }

        //-------CONECTAR BRAZO-------------------
        // se pulsa R y se esta cerca de la palanca
        if (_inputController.ConectarParte && _validPalancaHitbox)
        {
            _brazoConectado = true;
            _mySpriteRenderer.color = Color.blue;
        }
        // se pulsa T y se esta lejos de la palanca
        else if (_inputController.RecuperarParte && _validPalancaHitbox)
        {
            _brazoConectado = false;
            _mySpriteRenderer.color = Color.white;
        }
    }

}
