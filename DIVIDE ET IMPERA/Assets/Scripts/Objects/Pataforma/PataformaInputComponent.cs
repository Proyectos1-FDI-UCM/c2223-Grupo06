using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PataformaInputComponent : MonoBehaviour
{
    private MovementComponent _movementComponent;
    private InputController _inputController;
    private PataformaInputComponent _pataformaInputComponent;
    [SerializeField]
    private GameObject _pataforma;
    [SerializeField]
    private GameObject _player;


    //-------------DIRECCIÓN----------------------------
    //Setea la direccion en la que se mueve el jugador, -1 = izq y 1 = drcha
    private int _pDireccion;
    public int PataformaDireccion { get { return _pDireccion; } }


    #region Methods

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _inputController = _player.GetComponent<InputController>();
        _movementComponent = GetComponent<MovementComponent>();
        _pataformaInputComponent = GetComponent<PataformaInputComponent>();
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
                _inputController.enabled = true;
                Debug.Log(_inputController.enabled);

                // desactiva el movimiento de la pataforma
                // desactiva este componente
                this.enabled = false;
            }
            
        }



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
}
