using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    #region Properties 
    public bool _isGrounded;
    public bool IsGrounded { get { return _isGrounded; } }
    private LayerMask _levelMask;
    #endregion

    #region Methods
    /*
    //Cuando los pies del jugador (o sea el Ground Check) toca el suelo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isGrounded = true;
        
        
    }
    //Cuando los pies del jugador (o sea el Ground Check) dejan de tocar el suelo
    private void OnTriggerExit2D(Collider2D collision)
    {
        _isGrounded = false;
    }*/
    #endregion

    private void Start()
    {
        _levelMask = LayerMask.GetMask("Level");
    }

    private void Update()
    {
        //Lanza raycast hacia abajo en tres puntos (izq, centro y drcha de Timmy, para no depender solo de uno de esos puntos)
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.5f, _levelMask)
            || Physics2D.Raycast(transform.position - new Vector3(0.5f, 0f, 0f), Vector2.down, 1.5f, _levelMask)
            || Physics2D.Raycast(transform.position + new Vector3(0.5f, 0f, 0f), Vector2.down, 1.5f, _levelMask))
        {
            _isGrounded = true;
        }
        else
            _isGrounded = false;
    }
}
