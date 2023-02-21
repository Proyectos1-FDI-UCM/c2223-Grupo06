using UnityEngine;

public class JumpComponent : MonoBehaviour
{
    #region References
    [SerializeField]
    private Rigidbody2D _playerRigidbody2D;
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
            _playerRigidbody2D.AddForce(new Vector2(0, _jumpForce));
        }
    }
    //Cuando los pies del jugador (o sea el Ground Check) toca el suelo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isGrounded = true;
    }
    //Cuando los pies del jugador (o sea el Ground Check) dejan de tocar el suelo
    private void OnTriggerExit2D(Collider2D collision)
    {
        _isGrounded = false;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
