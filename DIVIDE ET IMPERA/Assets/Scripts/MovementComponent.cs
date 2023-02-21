using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    #region Referencias
    private Rigidbody2D _playerRigidbody2D;
    #endregion
    #region Parámetros
    [Tooltip("Velocidad del jugador")]
    [SerializeField]
    private float _speed;
    [Tooltip("Velocidad a la que el jugador frena cuando deja de moverse")]
    [SerializeField]
    private float _rozamientoFreno;
    #endregion
    #region Properties
    #endregion
    #region Methods
    public void Move(int _direccion)
    {
        //Cuando se pulsa una tecla para moverse, el jugador se mueve en esa direccion
        if (_direccion != 0)
        {
            _playerRigidbody2D.velocity = new Vector2(_speed * _direccion, _playerRigidbody2D.velocity.y);
        }
        /*Si el jugador ya no pulsa ninguna tecla y sigue moviendose en alguna direccion, se aumenta o decrementa la velocidad
        progresivamente hasta que llega a 0 para dar la sensacion de inercia*/
        else if (_direccion == 0)
        {
            if (_playerRigidbody2D.velocity.x > 0)
                _playerRigidbody2D.velocity = new Vector2(_playerRigidbody2D.velocity.x - Time.deltaTime * _rozamientoFreno, _playerRigidbody2D.velocity.y);
            if (_playerRigidbody2D.velocity.x < 0)
                _playerRigidbody2D.velocity = new Vector2(_playerRigidbody2D.velocity.x + Time.deltaTime * _rozamientoFreno, _playerRigidbody2D.velocity.y);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
