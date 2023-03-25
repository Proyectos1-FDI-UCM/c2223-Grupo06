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
    #endregion
    #region Parameters
    [SerializeField]
    private float _horizontalForce;
    [SerializeField]
    private float _verticalForce;
    [SerializeField]
    private bool _furbo;
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
                _thrownObject = Instantiate(_armPrefab, _myTransform.position, _myTransform.rotation);
                _thrownObjectRB = _thrownObject.GetComponent<Rigidbody2D>();
            }
            if (_thrownObjectRB != null)
            {
                _isThrowing = true;
                _thrownObjectRB.AddForce(new Vector2(_horizontalForce * 100 * _myTransform.localScale.x, _verticalForce * 100));
            }
        }
    }

    public void LanzarBola() // voy a hacer otro m�todo porque el otro est� joya y no quiero mancillarlo
    { // EST� PENSADO PARA LANZAR LA BOLA *DEL RIBCAGE*
        if (PlayerManager.Instance.Objeto == PlayerManager.Objetos.BOLA && (_playerManager.Brazos > 0 || _furbo)) // Si tiene una bola
        {
            _thrownObject = Instantiate(_ballPrefab, _myTransform.position + (_myTransform.right * _myTransform.localScale.x) / 2, _myTransform.rotation); // La instancia
            //_thrownObject.transform.position += Vector3.up; // M�s arriba porque si no se choca con timmy LOL
            _thrownObjectRB = _thrownObject.GetComponentInChildren<Rigidbody2D>(); // Pilla su RB
            _thrownObjectRB.AddForce(new Vector2(_horizontalForce * 100 * _myTransform.localScale.x, _verticalForce * 100)); // Lo yeetea
            PlayerManager.Instance.EliminarObjeto(); // PUM ya no tiene bola :P
            _isThrowing = true;
        }
    }

    public void ChutarBola()
    { // EST� PENSADO PARA CHUTAR UNA BOLA *DELANTE*
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
        if (_thrownObjectRB != null)
        {
            _isThrowing = true;
            _thrownObjectRB.AddForce(new Vector2(_horizontalForce * 100 * _myTransform.localScale.x, _verticalForce * 100));
        }
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