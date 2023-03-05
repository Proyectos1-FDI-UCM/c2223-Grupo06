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
    // palanca activada o no
    private bool _palanca;
    // brazo conectado a la palanca 
    private bool _brazoConectado = false;
    // esta en el área de una palanca
    public bool _validPalancaHitbox; //no tengo ni idea de por que se llama asi,
                                     //he copiado el nombre que utilizaba antes
                                     //porque no se me ocurre otro xd
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
        // si se ha pulsado la E, el brazo está conectado y está en el estado correcto
        if (_inputController.Interactuar && _brazoConectado 
            && (PlayerManager.State == PlayerManager.TimmyStates.S1 
            || PlayerManager.State == PlayerManager.TimmyStates.S4))
        {
            _palanca = Activar();
        }

        //-------CONECTAR BRAZO-------------------
        // se pulsa R y se esta cerca de la palanca
        if (_inputController.ConectarParte && _validPalancaHitbox 
            && (PlayerManager.State == PlayerManager.TimmyStates.S0
            || PlayerManager.State == PlayerManager.TimmyStates.S3))
        {
            // conecta el brazo
            _brazoConectado = true;

            // SI TIENE PIERNAS
            if (PlayerManager.State == PlayerManager.TimmyStates.S0)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S1);
                Debug.Log("palanca " + PlayerManager.State);
            }
            // SI NO TIENE PIERNAS
            else if (PlayerManager.State == PlayerManager.TimmyStates.S3)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S4);
            }

            
            // cambia el color (deberia ser sprite)
            _mySpriteRenderer.color = Color.blue;
        }
        // se pulsa T, está cerca de la palanca, está en los estados correctos y hay un brazo conectado
        else if (_inputController.RecuperarParte && _validPalancaHitbox
            && (PlayerManager.State == PlayerManager.TimmyStates.S1
            || PlayerManager.State == PlayerManager.TimmyStates.S4) 
            && _brazoConectado)
        {
            // desconecta el brazo
            _brazoConectado = false;

            // si tiene las piernas cambia de estado
            if (PlayerManager.State == PlayerManager.TimmyStates.S1)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S0);
            }
            // si no tiene piernas cambia de estado
            else if (PlayerManager.State == PlayerManager.TimmyStates.S4)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S3);
            }

            // cambia de color (deberia ser sprite)
            _mySpriteRenderer.color = Color.white;
        }
    }

}
