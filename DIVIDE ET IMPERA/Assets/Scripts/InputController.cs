using System.Threading;
using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Referencias
    private JumpComponent _playerJump;
    #endregion
    #region Properties 
    //Setea la direccion en la que se mueve el jugador, -1 = izq y 1 = drcha
    public int _direccion;
    public int Direccion { get { return _direccion; } }
    #endregion
    // Start is called beforse the first frame update
    void Start()
    {
        _playerJump = GetComponentInChildren<JumpComponent>();
    }

    private void FixedUpdate()
    {

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
            _direccion = 0;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _playerJump.Jump();
        }
    }
}
