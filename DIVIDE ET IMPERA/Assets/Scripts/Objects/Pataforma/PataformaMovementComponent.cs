using UnityEngine;

public class PataformaMovementComponent : MonoBehaviour
{

    #region Referencias
    private PataformaInputComponent _pataformaInputComponent;
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
    public int _pDireccion;
    public int PataformaDireccion { get { return _pDireccion; } }
    #endregion




    private void Move2()
    {
        if (_pataformaInputComponent.PataformaDireccion == 1)
        {
            transform.Translate(Vector3.right * Time.deltaTime * _speed);
        }
        else if (_pataformaInputComponent.PataformaDireccion == -1)
        {
            transform.Translate(Vector3.left * Time.deltaTime * _speed);
        }
    }

    public void Move()
    {
        //Cuando se pulsa una tecla para moverse, el jugador se mueve en esa direccion cambiando la velocidad
        if (_pDireccion != 0)
        {
            _myRigidbody2D.velocity = new Vector2(_speed * _pDireccion, _myRigidbody2D.velocity.y);
        }
        /*Si el jugador ya no pulsa ninguna tecla y sigue moviendose en alguna direccion, se aumenta o decrementa la velocidad
        progresivamente hasta que llega a 0 para dar la sensacion de inercia*/
        else
        {
            if (_myRigidbody2D.velocity.x > Mathf.Epsilon)
                _myRigidbody2D.velocity = new Vector2(_myRigidbody2D.velocity.x - Time.deltaTime * _rozamientoFreno, _myRigidbody2D.velocity.y);
            if (_myRigidbody2D.velocity.x < Mathf.Epsilon)
                _myRigidbody2D.velocity = new Vector2(_myRigidbody2D.velocity.x + Time.deltaTime * _rozamientoFreno, _myRigidbody2D.velocity.y);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        _pataformaInputComponent = GetComponent<PataformaInputComponent>();
        //_inputController = GetComponent<InputController>();
        _myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _pDireccion = _pataformaInputComponent.PataformaDireccion;

    }

    private void FixedUpdate()
    {
        Move2();
    }
}
