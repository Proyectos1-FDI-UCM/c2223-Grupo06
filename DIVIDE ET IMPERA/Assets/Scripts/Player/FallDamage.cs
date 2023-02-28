using UnityEngine;

public class FallDamage : MonoBehaviour
{
    #region references
    public BoneStateBar _boneStateBar;
    private GroundCheck _groundCheck;
    Rigidbody2D _rigidBody2D;
    #endregion
    #region parameters
    [SerializeField]
    private float _allowedSpeed; // velocidad m�x que permito antes de hacer da�o
    [SerializeField]
    private float _previousSpeed; // velocidad antes de llegar al suelo
    #endregion
    #region properties
    [SerializeField]
    bool _onGround; // para saber si est� en el suelo
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _boneStateBar = GetComponent<BoneStateBar>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {

        // a quien vea esto: me estoy volviendo completamente loca
        /*_onGround = _groundCheck._isGrounded; // comprobaci�n de _onGround
        if (!_onGround)
        {
            _previousSpeed = _rigidBody2D.velocity.y; // si no est� en el suelo se mantiene la velocidad
        }
        else
        {
            if (_rigidBody2D.velocity.y < _allowedSpeed) // si se supera la velocidad permitida -> aplicas da�o
            {
                Debug.Log(_rigidBody2D.velocity.y);
                _boneStateBar.BoneDamage(_damage: 20f);
                _previousSpeed = 0; // si se llega al suelo la velocidad vuelve a 0
            }
        }*/

        // _onGround = _groundCheck._isGrounded; // comprobaci�n de _onGround
        if (_rigidBody2D.velocity.y < _allowedSpeed) // si se supera la velocidad permitida y se est� en el suelo -> aplicas da�o
        {
            Debug.Log(_rigidBody2D.velocity.y);
            _boneStateBar.BoneDamage(_damage: 20f);
            //_previousSpeed = 0; // si se llega al suelo la velocidad vuelve a 0
        }
    }
}
