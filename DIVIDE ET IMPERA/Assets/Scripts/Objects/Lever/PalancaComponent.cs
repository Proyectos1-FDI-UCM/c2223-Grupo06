using UnityEngine;

public class PalancaComponent : MonoBehaviour
{

    #region Referencias
    private InputController _inputController;
    private PlayerManager _playerManager;
    private MovingPlatformComponent _movingPlatform;
    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private GameObject _objeto;
    #endregion

    #region Properties
    // palanca activada o no
    [SerializeField]
    private bool _palanca;
    public bool Palanca { get { return _palanca; } }

    // brazo conectado a la palanca 
    private bool _brazoConectado = false;
    public bool BrazoConectado { get { return _brazoConectado; } set { _brazoConectado = value; } }

    // esta en el área de una palanca
    public bool _validPalancaHitbox; //no tengo ni idea de por que se llama asi,
                                     //he copiado el nombre que utilizaba antes
                                     //porque no se me ocurre otro xd

    private bool _activarObjeto;
    public bool ActivarObjeto { get { return _activarObjeto; } }
    #endregion

    #region Métodos

    private void Activar()
    {
        _palanca = ActivarPalanca();
        ActivarObjetos();

    }

    // activa o desactiva la palanca dependiendo de su estado anterior
    private bool ActivarPalanca()
    {
        bool _lvr = !_palanca;
        _palanca = !_palanca;
        return _lvr;
    }

    // interactua con el objeto de fuera (puerta, plataforma etc)
    private void ActivarObjetos()
    {
        if (_palanca && !_movingPlatform.enabled)
        {
            _movingPlatform.enabled = true;
            //Debug.Log("activar " + _movingPlatform.enabled);
        }
        else if (!_palanca && _movingPlatform.enabled)
        {
            _movingPlatform.enabled = false;
            //Debug.Log("desactivar " + _movingPlatform.enabled);
        }


    }

    //Conecta el brazo
    public void ConectarBrazo(bool conected)
    {
        _brazoConectado = conected;
    }
    #endregion


    private void Start()
    {
        _inputController = PlayerAccess.Instance.InputController;
        _playerManager = PlayerAccess.Instance.PlayerManager;
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _movingPlatform = _objeto.GetComponent<MovingPlatformComponent>();

    }

    private void Update()
    {
        // si se ha pulsado la E, el brazo está conectado y está en el estado correcto
        if (_inputController.Interactuar && _brazoConectado
            && (PlayerManager.State == PlayerManager.TimmyStates.S1
            || PlayerManager.State == PlayerManager.TimmyStates.S4
            || PlayerManager.State == PlayerManager.TimmyStates.S2
            || PlayerManager.State == PlayerManager.TimmyStates.S5))
        {
            Activar();
        }

        //-------CONECTAR BRAZO-------------------
        // se pulsa R y se esta cerca de la palanca
        if (_inputController.ConectarParte && _validPalancaHitbox
            && (PlayerManager.State == PlayerManager.TimmyStates.S0
            || PlayerManager.State == PlayerManager.TimmyStates.S3
            || PlayerManager.State == PlayerManager.TimmyStates.S1
            || PlayerManager.State == PlayerManager.TimmyStates.S4))
        {
            // conecta el brazo
            ConectarBrazo(true);

            // SI TIENE PIERNAS
            if (PlayerManager.State == PlayerManager.TimmyStates.S0)
            {
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S1);
            }
            // SI NO TIENE PIERNAS
            else if (PlayerManager.State == PlayerManager.TimmyStates.S3)
            {
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S4);
            }

            else if (PlayerManager.State == PlayerManager.TimmyStates.S1)
            {
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S2);
            }

            else if (PlayerManager.State == PlayerManager.TimmyStates.S4)
            {
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S5);
            }

            // cambia el color (deberia ser sprite)
            //_mySpriteRenderer.color = Color.blue;
        }
        // se pulsa T, está cerca de la palanca, está en los estados correctos y hay un brazo conectado
        else if (_inputController.RecuperarParte && _validPalancaHitbox
            && (PlayerManager.State == PlayerManager.TimmyStates.S1
            || PlayerManager.State == PlayerManager.TimmyStates.S4
            || PlayerManager.State == PlayerManager.TimmyStates.S2
            || PlayerManager.State == PlayerManager.TimmyStates.S5)
            && _brazoConectado)
        {
            // desconecta el brazo
            ConectarBrazo(false);

            // si tiene las piernas cambia de estado
            if (PlayerManager.State == PlayerManager.TimmyStates.S1)
            {
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S0);
            }
            // si no tiene piernas cambia de estado
            else if (PlayerManager.State == PlayerManager.TimmyStates.S4)
            {
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S3);
            }

            else if (PlayerManager.State == PlayerManager.TimmyStates.S2)
            {
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S1);
            }

            else if (PlayerManager.State == PlayerManager.TimmyStates.S5)
            {
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S4);
            }

            // cambia de color (deberia ser sprite)
            //_mySpriteRenderer.color = Color.white;
        }
    }

}
