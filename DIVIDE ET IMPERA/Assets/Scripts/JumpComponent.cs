using UnityEngine;

public class JumpComponent : MonoBehaviour
{
    #region References
    private Rigidbody2D _myRigidbody2D;
    private GroundCheck _grCheck;
    #endregion
    #region Parameters
    [Tooltip("Fuerza del salto")]
    [SerializeField]
    private float _jumpForce;
    #endregion
    #region Properties
    private bool _isGrounded;
    #endregion
    #region Methods
    //Más adelante quizás probar a volverlo a hacer desde 0 por Transform en vez de físicas de Unity
    public void Jump()
    {
        if (_isGrounded)
        {
            _myRigidbody2D.AddForce(new Vector2(0, _jumpForce));
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        _isGrounded = true;
    }
    //Cuando los pies del jugador (o sea el Ground Check) dejan de tocar el suelo
    private void OnTriggerExit2D(Collider2D collision)
    {
        _isGrounded = false;
    }*/
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody2D= GetComponent<Rigidbody2D>();
        _grCheck = GetComponentInChildren<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = _grCheck._isGrounded;
    }
}
