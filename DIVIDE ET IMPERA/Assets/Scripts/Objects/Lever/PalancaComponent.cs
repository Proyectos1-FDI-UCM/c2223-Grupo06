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
    private GameManager _gameManager;
    #endregion

    #region Properties
    // indica si la palanca hay que dejarla activada para que funcione o no (como con el boton por peso)
    [SerializeField]
    private bool _temporary;

    // palanca activada o no
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

    public void Activar()
    {
        _palanca = ActivarPalanca();
        ActivarObjetos();

        // sfx
        if(SFXComponent.Instance != null)
            SFXComponent.Instance.SFXObjects(0);
    }

    // activa o desactiva la palanca dependiendo de su estado anterior
    public bool ActivarPalanca()
    {
        bool _lvr = !_palanca;
        _palanca = !_palanca;
        return _lvr;
    }

    // interactua con el objeto de fuera (puerta, plataforma etc)
    public void ActivarObjetos()
    {
        if (_palanca)
        {
            _objeto.GetComponent<NewPlatformMovement>().OnOff(true);
            //_movingPlatform.enabled = true;
            //Debug.Log("activar " + _movingPlatform.enabled);
        }
        else if (!_palanca)
        {
            _objeto.GetComponent<NewPlatformMovement>().OnOff(false);
            //_movingPlatform.enabled = false;
            //Debug.Log("desactivar " + _movingPlatform.enabled);
        }
    }

    //Conecta el brazo
    public void ConectarBrazo(bool conected)
    {
        _brazoConectado = conected;
        PlayerManager.Instance.ConnectedToLever(gameObject);
    }

    public void DesconectarBrazo()
    {
        ConectarBrazo(false);
        PlayerManager.Instance.Brazos++;
        PlayerManager.Instance.ConnectedToLever(null);
    }

    private bool FindArmInLever()
    {
        bool aux = false;




        return aux;
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
    { // Cyn: creo que no hace falta comprobar el estado en ninguna de las situaciones, pero lo he optimizado y los dejo igualmente
        // si se ha pulsado la E, el brazo está conectado y está en el estado correcto
        if (_inputController.Interactuar && _brazoConectado
            && (PlayerManager.Instance.Brazos < 2))
        {
            Activar();
        }

        if (_temporary && !_brazoConectado && _palanca)
        {
            Activar();
        }

        //-------CONECTAR BRAZO-------------------
        // se pulsa A o S y se esta cerca de la palanca
        if (_inputController.ConectarBrazo && _validPalancaHitbox
            && (PlayerManager.Instance.Brazos > 0))
        {
            // conecta el brazo
            ConectarBrazo(true);
            PlayerManager.Instance.Brazos--;
            PlayerManager.Instance.ConnectedToLever(gameObject);
        }
        // se pulsa A o S, está cerca de la palanca, está en los estados correctos y hay un brazo conectado
        else if (_inputController.RecuperarBrazo && _validPalancaHitbox
            && (PlayerManager.Instance.Brazos < 2)
            && _brazoConectado)
        {
            // desconecta el brazo
            DesconectarBrazo();
        }
    }

}
