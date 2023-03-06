using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PataformaInputComponent : MonoBehaviour
{
    private MovementComponent _movementComponent;
    private InputController _inputController;
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
    }

    // Update is called once per frame
    void Update()
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
}
