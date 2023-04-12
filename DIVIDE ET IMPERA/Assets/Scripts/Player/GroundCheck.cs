using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    #region Properties 
    public bool _isGrounded;
    public bool IsGrounded { get { return _isGrounded; } }
    private LayerMask _levelMask;

    private float _distance = 1f;
    private float _separation = 0.4f;
    #endregion

    #region Methods
    public bool FallDamageGroundCheck()
    {
        return Physics2D.Raycast(transform.parent.position, Vector2.down, _distance, _levelMask);
    }
    #endregion

    private void Start()
    {
        _levelMask = LayerMask.GetMask("Level");
    }

    private void Update()
    {
        //Lanza raycast hacia abajo en tres puntos (izq, centro y drcha de Timmy, para no depender solo de uno de esos puntos)
        if (Physics2D.Raycast(transform.parent.position, Vector2.down, _distance, _levelMask)
            || Physics2D.Raycast(transform.parent.position - new Vector3(_separation, 0f, 0f), Vector2.down, _distance, _levelMask)
            || Physics2D.Raycast(transform.parent.position + new Vector3(_separation, 0f, 0f), Vector2.down, _distance, _levelMask))
        {
            _isGrounded = true;
        }
        else
            _isGrounded = false;
    }
}
