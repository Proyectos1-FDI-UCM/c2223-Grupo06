using UnityEngine;

public class ThrowArm : MonoBehaviour
{
    #region References
    private PlayerManager _playerManager;
    private Transform _myTransform;
    [SerializeField]
    private GameObject _armPrefab;
    #endregion
    #region Parameters
    [SerializeField]
    private float _horizontalForce;
    [SerializeField]
    private float _verticalForce;
    #endregion
    #region Properties
    private PlayerManager.TimmyStates _currentState;
    private GameObject _thrownObject;
    private Rigidbody2D _thrownObjectRB;
    private Collider2D[] _colliders;
    private bool _ballFound = false;
    #endregion
    #region Methods
    public void LanzarBrazo()
    {
        _colliders = Physics2D.OverlapCircleAll(_myTransform.position, 1f);
        int i = 0;
        _ballFound = false;

        while (i < _colliders.Length && !_ballFound)
        {
            if (_colliders[i].gameObject.CompareTag("Ball"))
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
        else
        {
            if (_currentState != PlayerManager.TimmyStates.S2 && _currentState != PlayerManager.TimmyStates.S5)
            {
                if (_currentState == PlayerManager.TimmyStates.S0)
                {
                    _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S1);
                }
                if (_currentState == PlayerManager.TimmyStates.S1)
                {
                    _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S2);
                }
                if (_currentState == PlayerManager.TimmyStates.S3)
                {
                    _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S4);
                }
                if (_currentState == PlayerManager.TimmyStates.S4)
                {
                    _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S5);
                }
                _thrownObject = Instantiate(_armPrefab, _myTransform.position, _myTransform.rotation);
                _thrownObjectRB = _thrownObject.GetComponent<Rigidbody2D>();
            }
        }
        _thrownObjectRB.AddForce(new Vector2(_horizontalForce * 100 * _myTransform.localScale.x, _verticalForce * 100));
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        _myTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState = PlayerManager.State;
    }
}
