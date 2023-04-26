//using UnityEditor.Tilemaps;
using UnityEngine;

public class PataformaComponent : MonoBehaviour
{
    #region TUTORIAL
    /// <summary>
    /// Como poner la Patwoforma en escena es un coñazo asqueroso y es entre muy y demasiado tiquisimiquis
    /// voy a haceros un tutorial dpm para que sepais como cuidar bien a mi pequeña patwoforma:
    /// 
    /// Cuando se pone en escena el objeto, tiene referenciado a Timmy, pero no al Timmy de la escena,
    /// por lo que teneis que arrastrar al Timmy de la escena a la referencia en el componente
    /// 
    /// /// /// Ya no es necesario lo anterior, cambiado a que este hecho por codigo
    /// 
    /// y con esto se acabó el tutorial maravilloso y estupendo, recordad darle a like y compartir
    /// y acivar la campanita ;)
    /// </summary>
    #endregion

    #region Referencias
    private InputController _inputController;
    private SpriteRenderer _mySpriteRenderer;
    private GameObject _player;
    private Rigidbody2D _playerRigidbody;
    private SpriteRenderer _patasSprite;

    [SerializeField]
    private GameObject _pataformaMain;
    private PataformaMovementComponent _pataformaMovementComponent;
    private Rigidbody2D _pataformaRigidbody;
    [SerializeField]
    private PataformaMovementComponent pataformaMovementComponent;

    [SerializeField] GameObject _patas;
    [SerializeField] Animator _animator;
    #endregion

    #region Properties
    // piernas conectadas a la pataforma 
    [SerializeField]
    private bool _piernasConectadas = false;
    public bool PiernasConectadas { get { return _piernasConectadas; } set { _piernasConectadas = value; } }

    // alubiat conectadas a la pataforma 
    [SerializeField]
    private bool _alubiatConectadas = false;
    public bool AlubiatConectadas { get { return _alubiatConectadas; } set { _alubiatConectadas = value; } }

    // esta en el área de una pataforma
    public bool _validPataformaHitbox;

    private bool _isPataforma;
    public bool Pataforma { get { return _isPataforma; } }

    //-------------DIRECCIÓN----------------------------
    //Setea la direccion en la que se mueve el jugador, -1 = izq y 1 = drcha
    private int _pDireccion;
    public int PataformaDireccion { get { return _pDireccion; } }


    [SerializeField]
    public bool _activarPataforma;
    #endregion



    #region Initial methods

    private bool ConectarPiernas()
    {
        bool aux;
        if (_inputController.ConectarPiernas && _validPataformaHitbox
            && (PlayerManager.Instance.Piernas))
        {
            aux = true;
        }
        else
        {
            aux = false;
        }
        return aux;
    }

    private bool DesconectarPiernas()
    {
        bool aux;
        if (_inputController.RecuperarPiernas && _validPataformaHitbox
            && !PlayerManager.Instance.Piernas
            && _piernasConectadas)
        {
            aux = true;
        }
        else
        {
            aux = false;
        }
        return aux;
    }

    private bool ConectarAlubiat()
    {
        bool aux;
        if (_inputController.ConectarAlubiat && _validPataformaHitbox
            && (PlayerManager.Instance.Alubiat))
        {
            aux = true;
        }
        else
        {
            aux = false;
        }
        return aux;
    }

    private bool DesconectarAlubiat()
    {
        bool aux;
        if (_inputController.RecuperarAlubiat && _validPataformaHitbox
            && !PlayerManager.Instance.Alubiat
            && _alubiatConectadas)
        {
            aux = true;
        }
        else
        {
            aux = false;
        }
        return aux;
    }
    #endregion

    #region Methods
    private void ConectaPiernas()
    {
        //-------CONECTAR PIERNAS-------------------
        // se pulsa D y se esta cerca de la pataforma
        if (ConectarPiernas())
        {
            // conecta las piernas en la pataforma
            _piernasConectadas = true;

            if (PlayerManager.Instance.Piernas)
            {
                //PlayerManager.Instance.Piernas = false;
                PlayerManager.Instance.AdiosPiernas();
            }

            PlayerManager.Instance.ConnectedToPataforma(gameObject);
        }
        // se pulsa D, está cerca de la pataforma, está en los estados correctos y hay patas conectadas
        else if (DesconectarPiernas())
        {
            // desconecta las piernas en la pataforma
            _piernasConectadas = false;

            //PlayerManager.Instance.Piernas = true;
            PlayerManager.Instance.HolaPiernas();

            PlayerManager.Instance.ConnectedToPataforma(null);
        }
        else
        //-------CONECTAR ALUBIAT-------------------
        // se pulsa C y se esta cerca de la pataforma
        if (ConectarAlubiat())
        {
            Debug.Log("alubiat trolleando ");
            // conecta las piernas en la pataforma
            _alubiatConectadas = true;

            if (PlayerManager.Instance.Alubiat)
            {
                PlayerManager.Instance.AdiosAlubiat();
            }

            PlayerManager.Instance.ConnectedToPataforma(gameObject);
        }
        // se pulsa C, está cerca de la pataforma, está en los estados correctos y hay patas conectadas
        else if (DesconectarAlubiat())
        {
            // desconecta las piernas en la pataforma
            _alubiatConectadas = false;

            PlayerManager.Instance.HolaAlubiat();

            PlayerManager.Instance.ConnectedToPataforma(null);
        }

    }
    private void Visual()
    {
        if (_piernasConectadas || _alubiatConectadas)
        {
            _patas.SetActive(true);
            if (_alubiatConectadas)
            {
                _patasSprite.color = Color.green;
            }
            else if (_piernasConectadas)
            {
                _patasSprite.color = Color.white;
            }
        }
        else
        {
            _patas.SetActive(false);
        }


    }

    private void ChangeInput()
    {
        //---INPUT CHANGE---------------------------------------
        //------de pataforma a player---------------------------
        // Shift + D para cambiar de vuelta
        if (((Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D)) && _piernasConectadas)
            || (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C)) && _alubiatConectadas)
        {
            PlayerInControl();
        }

        //------de player a pataforma----------------------------
        // Shift + D para reactivar al player
        if (_inputController.ChangeToPiernas && _piernasConectadas
            || _inputController.ChangeToAlubiat && _alubiatConectadas)
        {
            LegsInControl();
        }
    }

    public void PlayerInControl()
    {
        // si el input del player esta desactivado y el de la pataforma específica
        // esta activado procede a invertirlos, asi se asegura de que el cambio de
        // input está individualizado
        if (!_inputController.enabled && _activarPataforma)
        {
            // cambia el rigidbody de la pataforma a kinematic para que no se mueva cuando
            // timmy se suba encima
            _pataformaRigidbody.bodyType = RigidbodyType2D.Kinematic;
            // cambia el rigidbody del player para que siga las físicas
            _playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
            // cambia el constraint al eje Z
            _playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            // desactiva la plataforma
            _activarPataforma = false;
            // activa el input del player
            _inputController.enabled = true;

            // cambio de control de parte (es para el HUD)
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.CABEZA);

            //Cambia que la camara siga a Timmy
            CameraMovement.Instance.ChangeWhoToFollow(_player);

            //Cambio objeto en control para la pausa
            PlayerManager.Instance.ChangePartInControl(_player);

            _pDireccion = 0;
            _animator.SetBool("move", false);
        }

    }

    private void LegsInControl()
    {
        // cambia el rb de la pataforma a dynamic, para que se choque con los obstaculos
        _pataformaRigidbody.bodyType = RigidbodyType2D.Dynamic;
        // cambia el rb del player para que siga a la pataforma (?)
        _playerRigidbody.bodyType = RigidbodyType2D.Kinematic;
        // cambia las contraints del player para que no resbale
        _playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;

        // activa la plataforma
        _activarPataforma = true;
        _player.GetComponent<InputController>().enabled = false;

        // desactiva el input del player
        if (_piernasConectadas)
        {
            _inputController._changeToPiernas = false;
        }
        else if (_alubiatConectadas)
        {
            _inputController._changeToAlubiat = false;
        }


        // cambio de control de parte (es para el HUD)
        PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.PIERNAS);
        Debug.Log("Piernas controlan");

        //Cambia el movimiento de la camara para que siga a las piernas
        CameraMovement.Instance.ChangeWhoToFollow(gameObject);

        //Cambio para el menu de pausa
        PlayerManager.Instance.ChangePartInControl(gameObject);

        _pDireccion = 0;
        _animator.SetBool("move", false);
    }

    private void PataformaInput()
    {
        //---MOVIMIENTO--------------------------------
        //------Input del movimiento horizontal de la pataforma--------
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _pDireccion = 1;

            // inicia el animator
            _animator.SetBool("move", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _pDireccion = -1;

            // inicia el animator
            _animator.SetBool("move", true);
        }
        else
        {
            _pDireccion = 0;

            // inicia el animator
            _animator.SetBool("move", false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.RequestStateChange(GameManager.GameStates.PAUSE);
            //_activarPataforma = false;
            GetComponent<PataformaMovementComponent>().enabled = false;
            GetComponentInChildren<Animator>().enabled = false;
            enabled = false;
        }
    }

    public void SetLegs(bool onoff)
    {
        _piernasConectadas = onoff;
    }
    #endregion

    void Start()
    {
        _player = PlayerAccess.Instance.gameObject;
        _inputController = _player.GetComponent<InputController>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _patasSprite = _patas.GetComponent<SpriteRenderer>();
        _pataformaMovementComponent = GetComponent<PataformaMovementComponent>();
        _pataformaRigidbody = _pataformaMain.GetComponent<Rigidbody2D>();
        _playerRigidbody = _player.GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        ConectaPiernas();

        Visual();

        ChangeInput();

        if (_activarPataforma)
        {
            PataformaInput();
        }
    }
}
