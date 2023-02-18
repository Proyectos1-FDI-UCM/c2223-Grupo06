using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovemetComponent : MonoBehaviour
{
    #region Referencias
    private Rigidbody2D _playerRigidbody2D;
    #endregion
    #region Parámetros
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _rozamientoFreno;
    #endregion
    #region Properties
    #endregion
    #region Methods
    public void Move( int _direccion)
    {
        if (_direccion != 0)
        {
            _playerRigidbody2D.velocity = new Vector2(_speed * _direccion, _playerRigidbody2D.velocity.y);
        }
        else if (_direccion == 0)
        {
            if (_playerRigidbody2D.velocity.x > 0)
                _playerRigidbody2D.velocity = new Vector2(_playerRigidbody2D.velocity.x - Time.deltaTime * _rozamientoFreno, _playerRigidbody2D.velocity.y);
            if (_playerRigidbody2D.velocity.x < 0)
                _playerRigidbody2D.velocity = new Vector2(_playerRigidbody2D.velocity.x + Time.deltaTime * _rozamientoFreno, _playerRigidbody2D.velocity.y);
        }
    }
    private void Jump()
    {

    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _playerRigidbody2D= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Mathf.Clamp(_playerRigidbody2D.velocity.x, -0.5f, 0.5f);
    }
}
