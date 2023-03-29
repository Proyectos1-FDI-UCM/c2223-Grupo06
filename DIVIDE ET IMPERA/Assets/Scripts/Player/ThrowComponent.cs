using UnityEngine;

public class ThrowComponent : MonoBehaviour
{
    #region References
    private PlayerManager _playerManager;
    private Transform _myTransform;
    private InputController _myInputController;
    private PlayerAnimationController _myPlayerAnimator;
    private Animator _myAnimator;
    [SerializeField]
    private GameObject _armPrefab;
    [SerializeField]
    private GameObject _ballPrefab;
    [SerializeField]
    private Transform _objectsReset;
    #endregion
    #region Parameters
    [SerializeField]
    private float _horizontalForce;
    [SerializeField]
    private float _verticalForce;
    [Tooltip("Si Timoteo puede lanzar bolas aun sin brazos")]
    [SerializeField]
    private bool _furbo;
    [Tooltip("Si las bolas giran al ser lanzados o no")]
    [SerializeField]
    private bool _inerciaBolas;
    [Tooltip("Si los brazos giran al ser lanzados o no")]
    [SerializeField]
    private bool _inerciaBrazos;
    [Tooltip("La fuerza con lo que los objetos giran al ser lanzados")]
    [SerializeField]
    private float _torque;
    private bool _isThrowing;
    public bool IsThrowing { get { return _isThrowing; } set { _isThrowing = value; } }
    #endregion
    #region Properties
    //private PlayerManager.TimmyStates _currentState;
    private GameObject _thrownObject;
    private Rigidbody2D _thrownObjectRB;
    private Collider2D[] _colliders;
    private bool _ballFound = false;
    #endregion
    #region Methods
    public void LanzarBrazo()
    { // SOLO PARA LANZAR EL BRAZO
        if (PlayerManager.Instance.Brazos > 0 || _furbo)
        {
            if (PlayerManager.Instance.Brazos > 0)
            {   // Lo he intentado optimizar un poco, no lo he querido mancillar
                PlayerManager.Instance.Brazos--; // Cambia directamente el estado en su propio update, no worries
                _thrownObject = Instantiate(_armPrefab, _myTransform.position, _myTransform.rotation, _objectsReset);
                _thrownObjectRB = _thrownObject.GetComponent<Rigidbody2D>();
            }

            if (Lanzamiento(_inerciaBrazos))
            {
                if (_inerciaBrazos) _thrownObjectRB.constraints = RigidbodyConstraints2D.None;
            }
        }
    }

    public void LanzarBola() // voy a hacer otro método porque el otro está joya y no quiero mancillarlo
    { // ESTÁ PENSADO PARA LANZAR LA BOLA *DEL RIBCAGE*
        if (PlayerManager.Instance.Objeto == PlayerManager.Objetos.BOLA && (_playerManager.Brazos > 0 || _furbo)) // Si tiene una bola
        {
            PlayerAccess.Instance.CollisionManager.ObjectStored.SetActive(true);
            PlayerAccess.Instance.CollisionManager.ObjectStored.transform.position = _myTransform.position + (_myTransform.right * _myTransform.localScale.x) / 2;
            //_thrownObject.transform.position += Vector3.up; // Más arriba ??
            _thrownObjectRB = PlayerAccess.Instance.CollisionManager.ObjectStored.GetComponent<Rigidbody2D>(); // Pilla su RB
        }

        if (Lanzamiento(_inerciaBolas)) 
        {
            PlayerManager.Instance.EliminarObjeto(); // PUM ya no tiene bola :P
        }
    }

    public void ChutarBola()
    { // ESTÁ PENSADO PARA CHUTAR UNA BOLA *DELANTE*
        _colliders = Physics2D.OverlapCircleAll(_myTransform.position, 1f);
        int i = 0;
        _ballFound = false;

        while (i < _colliders.Length && !_ballFound)
        {
            if (_colliders[i].gameObject.GetComponent<BallComponent>() != null)
            {
                _thrownObject = _colliders[i].gameObject;
                _ballFound = true;
            }
            i++;
        }
        if (_ballFound)
        {
            _thrownObjectRB = _thrownObject.GetComponent<Rigidbody2D>();
            _thrownObject.transform.position += Vector3.up;
        }

        Lanzamiento(_inerciaBolas);
    }

    public bool Lanzamiento(bool inercia)
    {
        if (_thrownObjectRB != null)
        {
            _thrownObjectRB.AddForce(new Vector2(_horizontalForce * 100 * _myTransform.localScale.x, _verticalForce * 100));
            if (inercia) _thrownObjectRB.AddTorque(_torque * -_myTransform.localScale.x, ForceMode2D.Force);
            return true;
        }
        else return false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_thrownObjectRB != null)
        {
            _thrownObject = null;
            _thrownObjectRB = null;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_thrownObjectRB != null)
        {
            _thrownObject = null;
            _thrownObjectRB = null;
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        _myTransform = GetComponent<Transform>();
        _myInputController = GetComponent<InputController>();
        _myPlayerAnimator = GetComponent<PlayerAnimationController>();
        _myAnimator = GetComponent<Animator>();
    }
}
