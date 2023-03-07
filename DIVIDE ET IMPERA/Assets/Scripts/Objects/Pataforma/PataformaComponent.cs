using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PataformaComponent : MonoBehaviour
{
    #region Referencias
    private InputController _inputController;
    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private GameObject _player;
    private PataformaInputComponent _pataformaInputComponent;
    private PataformaMovementComponent _pataformaMovementComponent;
    private Rigidbody2D _myRigidbody;
    
    #endregion

    #region Properties
    // piernas conectadas a la pataforma 
    private bool _piernasConectadas = false;
    public bool PiernasConectadas { get { return _piernasConectadas; } }

    // esta en el área de una pataforma
    public bool _validPataformaHitbox;

    private bool _isPataforma;
    public bool Pataforma { get { return _isPataforma; } }
    #endregion

    #region Methods

    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _inputController = _player.GetComponent<InputController>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _pataformaInputComponent = GetComponent<PataformaInputComponent>();
        _pataformaMovementComponent = GetComponent<PataformaMovementComponent>();
        _myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

       

        //---PATAFOMA---------------------------------------
        //------Input para interactuar con las piernas-----
        //--------- Hay que dejar pulsado primero el numero y luego la E para interactuar
        if (Input.GetKey(KeyCode.Alpha2) && Input.GetKeyUp(KeyCode.E))
        {
            if (!_inputController.enabled)
            {
                // activa el input del player
                _inputController.enabled = true;
                _player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                // desactiva el input de la pataforma
                _pataformaInputComponent.enabled = false;
                _pataformaMovementComponent.enabled = false;
                _myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            }

        }
        // para activar el input de las patas
        if (_inputController.Pataforma && _piernasConectadas)
        {
            _pataformaInputComponent.enabled = true;
            _pataformaMovementComponent.enabled = true;
            _myRigidbody.bodyType = RigidbodyType2D.Dynamic;
            _player.GetComponent<InputController>()._isPataforma = false;
        }


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
            _mySpriteRenderer.color = Color.blue;
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
}
