//using UnityEditor.Tilemaps;
using UnityEngine;

public class PataformaComponent : MonoBehaviour
{
    #region Referencias
    private InputController _inputController;
    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private GameObject _player;
    private PataformaMovementComponent _pataformaMovementComponent;
    private Rigidbody2D _pataformaRigidbody;
    private Rigidbody2D _playerRigidbody;


    [SerializeField]
    private PataformaMovementComponent pataformaMovementComponent;

    #endregion

    #region Properties
    // piernas conectadas a la pataforma 
    [SerializeField]
    private bool _piernasConectadas = false;
    public bool PiernasConectadas { get { return _piernasConectadas; } }

    // esta en el área de una pataforma
    public bool _validPataformaHitbox;

    private bool _isPataforma;
    public bool Pataforma { get { return _isPataforma; } }

    //-------------DIRECCIÓN----------------------------
    //Setea la direccion en la que se mueve el jugador, -1 = izq y 1 = drcha
    private int _pDireccion;
    public int PataformaDireccion { get { return _pDireccion; } }


    [SerializeField]
    private bool _activarPataforma;


    #endregion

    #region Methods
    private void ConectaPiernas()
    {
        //-------CONECTAR PIERNAS-------------------
        // se pulsa R y se esta cerca de la pataforma
        if (_inputController.ConectarParte && _validPataformaHitbox
            && (PlayerManager.State == PlayerManager.TimmyStates.S0
            || PlayerManager.State == PlayerManager.TimmyStates.S1
            || PlayerManager.State == PlayerManager.TimmyStates.S2))
        {
            // conecta el brazo
            _piernasConectadas = true;

            // SI TIENE BRAZOS
            if (PlayerManager.State == PlayerManager.TimmyStates.S0)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S3);
            }
            // SI NO TIENE UN BRAZO
            else if (PlayerManager.State == PlayerManager.TimmyStates.S1)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S4);
            }
            // SI NO TIENE BRAZOS
            else if (PlayerManager.State == PlayerManager.TimmyStates.S2)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S5);
            }

            Debug.Log(PlayerManager.State);
            // cambia el color (deberia ser sprite)
            _mySpriteRenderer.color = Color.cyan;
        }
        // se pulsa T, está cerca de la pataforma, está en los estados correctos y hay patas conectadas
        else if (_inputController.RecuperarParte && _validPataformaHitbox
            && (PlayerManager.State == PlayerManager.TimmyStates.S3
            || PlayerManager.State == PlayerManager.TimmyStates.S4
            || PlayerManager.State == PlayerManager.TimmyStates.S5)
            && _piernasConectadas)
        {
            // desconecta el brazo
            _piernasConectadas = false;

            // si tiene los brazos cambia de estado
            if (PlayerManager.State == PlayerManager.TimmyStates.S3)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S0);
            }
            // si no tiene un brazo cambia de estado
            else if (PlayerManager.State == PlayerManager.TimmyStates.S4)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S1);
            }
            // si no tiene brazos cambia de estado
            else if (PlayerManager.State == PlayerManager.TimmyStates.S5)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S2);
            }

            // cambia de color (deberia ser sprite)
            _mySpriteRenderer.color = Color.white;
        }
    }
    private void ChangeInput()
    {
        //---INPUT CHANGE---------------------------------------
        //------de pataforma a player---------------------------
        // 2 + E para cambiar de vuelta
        if (Input.GetKey(KeyCode.Alpha2) && Input.GetKeyUp(KeyCode.E))
        {
            // si el input del player esta desactivado y el de la pataforma específica
            // esta activado procede a invertirlos, asi se asegura de que el cambio de
            // input está individualizado
            if (!_inputController.enabled && _activarPataforma)
            {
                //_player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                // cambia el rigidbody de la pataforma a kinematic para que no se mueva cuando
                // timmy se suba encima
                _pataformaRigidbody.bodyType = RigidbodyType2D.Kinematic;
                // cambia el rigidbody del player para que siga las físicas
                _playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
                // cambia el constraint al eje Z
                _playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                // desactiva la plataforma
                _activarPataforma = false;
                
                //_myRigidbody.bodyType = RigidbodyType2D.Kinematic;

                // activa el input del player
                _inputController.enabled = true;

                _mySpriteRenderer.color = Color.cyan;

                // cambio de control de parte (es para el HUD)
                PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.CABEZA);
            }

        }
        //------de player a pataforma----------------------------
        // 2 + E para reactivar al player
        if (_inputController.ChangeToPataforma && _piernasConectadas)
        {
            // cambia el rb de la pataforma a dynamic, para que se choque con los obstaculos
            _pataformaRigidbody.bodyType = RigidbodyType2D.Dynamic;
            // cambia el rb del player para que siga a la pataforma (?)
            _playerRigidbody.bodyType = RigidbodyType2D.Kinematic;
            // cambia las contraints del player para que no resbale
            _playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;

            // activa la plataforma
            _activarPataforma = true;
            
            

            // desactiva el input del player
            _inputController._changeToPataforma = false;
            _mySpriteRenderer.color = Color.blue;

            // cambio de control de parte (es para el HUD)
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.PIERNAS);
        }
    }
    private void PataformaInput()
    {
        //---MOVIMIENTO--------------------------------
        //------Input del movimiento horizontal de la pataforma--------
        if (Input.GetKey(KeyCode.D))
        {
            _pDireccion = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _pDireccion = -1;
        }
        else
        {
            _pDireccion = 0;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _inputController = _player.GetComponent<InputController>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _pataformaMovementComponent = GetComponent<PataformaMovementComponent>();
        _pataformaRigidbody = GetComponent<Rigidbody2D>();
        _playerRigidbody = _player.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        ConectaPiernas();
        ChangeInput();

        if (_activarPataforma)
        {
            PataformaInput();
        }

        

    }
}
