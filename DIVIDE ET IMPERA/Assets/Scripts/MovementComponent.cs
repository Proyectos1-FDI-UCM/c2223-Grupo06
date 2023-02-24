using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    #region Referencias
    private InputController _inputController;
    private Rigidbody2D _myRigidbody2D;
    private GroundCheck _grCheck;
    private Transform _myTransform;
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
    private RaycastHit2D _hit;
    private LayerMask _levelLayer;
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

    private void AhoraCaigo() //No el de Arturo Valls
    {
        /*Esto son dos versiones: la primera funciona en una sola direccion, al implementarla también en el otro sentido deja de funcionar bien y es el mismo caso que
         la segunda version
        Ambas funcionan de la misma manera: si el jugador está en el aire y se esta moviendo, se lanza un rayo en esa direccion y si choca con una pared y la direccion
        no ha cambiado entonces la direccion se pone a 0 para que el jugadorno se mueva contra la pared, en el caso de que sean diferenes se deja igual*/
        
        if (!_grCheck._isGrounded) //Jugador en el aire
        {
            if (_direccion == 1) //se esta moviendo
            {
                _hit = Physics2D.Raycast(_myTransform.position, new Vector2(_myTransform.position.x + 1, 0), 1f, _levelLayer); //rayo
                if (_hit.collider != null) //rayo choca con algo
                {
                    _direccion = _inputController._direccion; //actualiza la direccion
                    if (_direccion == 1) //si es la misma entonces setear la direccion a 0 y si no no hacer nada
                        _direccion = 0;
                }
            }
        }

        /*Esta version está generalizada para ambas direcciones por lo que se introduce una direccion auxiliar para comparar la direccion antigua y la nueva.
         Tengo la teoria de que puede ser que o funcione porque la direccion no se actualice lo suficiente en el input, resultando en que
         la direccion normal y la auxiliar sean la misma siempre*/
        /*
        if (!_grCheck._isGrounded && _direccion != 0) //Jugador en aire y moviendose
        {
            _hit = Physics2D.Raycast(_myTransform.position, new Vector2(_myTransform.position.x + 1, 0), 1f, _levelLayer); //rayo en dir de movimiento
            if (_hit.collider != null)
            {
                int auxDir = _inputController._direccion; //dir auxiliar que recoge nuevo input
                if (auxDir == _direccion)
                    _direccion = 0;
                else
                    _direccion = auxDir; //en caso de que sean diferentes se actualiza la direccion con el valor de la nueva direccion
            }
        }*/
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _inputController = GetComponent<InputController>();
        _myRigidbody2D = GetComponent<Rigidbody2D>();
        _grCheck = GetComponentInChildren<GroundCheck>();
        _myTransform = GetComponent<Transform>();
        _levelLayer = LayerMask.GetMask("Level");
    }

    // Update is called once per frame
    void Update()
    {
        _direccion = _inputController._direccion;
        AhoraCaigo();
        Move();
        Debug.DrawRay(_myTransform.position, new Vector2(_myTransform.position.x + _direccion, 0));
    }
}
