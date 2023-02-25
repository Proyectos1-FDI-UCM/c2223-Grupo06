using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    #region Referencias
    private InputController _inputController;
    private Rigidbody2D _myRigidbody2D;
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
    private int _direccion;
    #endregion
    #region Methods
    public void Move()
    {
        //Cuando se pulsa una tecla para moverse, el jugador se mueve en esa direccion cambiando la velocidad
        if (_direccion != 0)
        {
            _myRigidbody2D.velocity = new Vector2(_speed * _direccion, _myRigidbody2D.velocity.y);
        }
        /*Si el jugador ya no pulsa ninguna tecla y sigue moviendose en alguna direccion, se aumenta o decrementa la velocidad
        progresivamente hasta que llega a 0 para dar la sensacion de inercia*/
        else
        {
            if (_myRigidbody2D.velocity.x > 0)
                _myRigidbody2D.velocity = new Vector2(_myRigidbody2D.velocity.x - Time.deltaTime * _rozamientoFreno, _myRigidbody2D.velocity.y);
            if (_myRigidbody2D.velocity.x < 0)
                _myRigidbody2D.velocity = new Vector2(_myRigidbody2D.velocity.x + Time.deltaTime * _rozamientoFreno, _myRigidbody2D.velocity.y);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _inputController = GetComponent<InputController>();
        _myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _direccion = _inputController.Direccion;
        Move();
    }
}
