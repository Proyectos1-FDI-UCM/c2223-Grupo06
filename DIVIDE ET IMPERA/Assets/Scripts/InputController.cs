using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Referencias
    private MovemetComponent _playerMovement;
    #endregion
    #region Properties
    private int _direccion;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _playerMovement= GetComponent<MovemetComponent>();
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
    }
}
