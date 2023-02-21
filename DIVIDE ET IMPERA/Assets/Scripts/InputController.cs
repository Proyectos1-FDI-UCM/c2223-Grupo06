using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Referencias
    private MovementComponent _playerMovement;
    private JumpComponent _playerJump;
    #endregion
    #region Properties 
    //Setea la direccion en la que se mueve el jugador, -1 = izq y 1 = drcha
    private int _direccion;
    #endregion
    // Start is called beforse the first frame update
    void Start()
    {
        _playerMovement= GetComponent<MovementComponent>();
        _playerJump = GetComponentInChildren<JumpComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            _direccion = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _direccion = -1;
        }
        else
        {
            _direccion= 0;
        }

        _playerMovement.Move(_direccion);

        if (Input.GetKey(KeyCode.Space))
        {
            _playerJump.Jump();
        }
    }
}
