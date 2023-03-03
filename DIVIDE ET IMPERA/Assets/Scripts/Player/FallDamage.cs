using UnityEngine;

public class FallDamage : MonoBehaviour
{
    #region references
    public BoneStateBar _boneStateBar;
    private GroundCheck _groundCheck;
    Rigidbody2D _rigidBody2D;
    private Transform _transform;
    private LayerMask _level;
    #endregion
    #region parameters
    [SerializeField]
    private float _allowedHeight;
    #endregion
    #region properties
    [SerializeField]
    bool _onGround; // para saber si está en el suelo
    private RaycastHit2D _hit;
    [SerializeField]
    private float _maxHeightReached;
    [SerializeField]
    private bool _applyFallDamage;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _boneStateBar = GetComponent<BoneStateBar>();
        _groundCheck = GetComponentInChildren<GroundCheck>();
        _transform = GetComponent<Transform>();
        _level = LayerMask.GetMask("Level");
    }

    // Update is called once per frame
    void Update()
    {
        _onGround = _groundCheck._isGrounded;  // comprobación de _onGround
        if (_onGround)
            _maxHeightReached = 0;
        if (_onGround && _applyFallDamage)
        {
            _boneStateBar.BoneDamage(_damage: 20f);
            _applyFallDamage = false;
        }
        _hit = Physics2D.Raycast(_transform.position, new Vector2(_transform.position.x, _transform.position.y - 1), 100, _level);
        if (_hit.distance >= _maxHeightReached)
        {
            _maxHeightReached = _hit.distance;
        }

        if (_maxHeightReached > _allowedHeight && _onGround)
        {
            _applyFallDamage = true;
        }

        Debug.Log(_hit.distance);
        /*if (!_onGround && (_rigidBody2D.velocity.y >= -_velocityFrame && _rigidBody2D.velocity.y <= _velocityFrame))
        {
            
        }*/
        Debug.DrawRay(_transform.position, new Vector2(0, -1));
    }
}
