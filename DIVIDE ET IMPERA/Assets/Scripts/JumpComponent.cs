using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpComponent : MonoBehaviour
{
    #region References
    [SerializeField]
    private Rigidbody2D _playerRigidbody2D;
    #endregion
    #region Parameters
    [SerializeField]
    private float _jumpForce;
    #endregion
    #region Properties
    [SerializeField]
    private bool _isGrounded;
    private LayerMask _groundMask;
    #endregion
    #region Methods
    public void Jump ()
    {
        if (_isGrounded)
        {
            _playerRigidbody2D.AddForce(new Vector2(0, _jumpForce * 100));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _groundMask)
        {
            _isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _groundMask)
        {
            _isGrounded = false;
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _groundMask = LayerMask.GetMask("Ground");
    }
}
