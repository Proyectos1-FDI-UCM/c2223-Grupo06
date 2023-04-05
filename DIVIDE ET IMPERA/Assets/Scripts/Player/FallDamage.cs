using UnityEngine;

public class FallDamage : MonoBehaviour
{
    #region references
    public BoneStateBar _boneStateBar;
    private GroundCheck _groundCheck;
    Rigidbody2D _rigidBody2D;
    #endregion
    #region parameters 
    [Tooltip("Velocidad máxima permitida antes de hacer daño")]
    [SerializeField]
    private float _allowedSpeed;  // velocidad max que permito antes de hacer daño
    [Tooltip("Velocidad antes de llegar al suelo")]
    [SerializeField]
    private float _previousSpeed; // velocidad antes de llegar al suelo
    #endregion
    #region properties
    private bool _onGround; // para saber si está en el suelo
    private SpringComponent _spring;
    #endregion

    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _boneStateBar = GetComponent<BoneStateBar>();
        _groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Update()
    {
        _onGround = _groundCheck._isGrounded; // comprobación de _onGround
        if (!_onGround)
        {
            _previousSpeed = _rigidBody2D.velocity.y; // si no está en el suelo se almacena la velocidad
        }
        else
        {
            //Codigo que comprueba las colisiones del jugador para ver si alguna es un muelle mirando si los contactos tienen el componente muelle
            Collider2D collider = GetComponent<Collider2D>();
            Collider2D[] colliders = Physics2D.OverlapBoxAll(collider.bounds.center, collider.bounds.size, 0);
            foreach (Collider2D otherCollider in colliders)
            {
                _spring = otherCollider.GetComponent<SpringComponent>();
            }

            if (_previousSpeed < _allowedSpeed && _spring == null) // si se supera la velocidad permitida y no has chocado con un muelle -> aplicas daño
            {
                _boneStateBar.BoneDamage(_damage: 20f);
                _previousSpeed = 0; // si se llega al suelo la velocidad vuelve a 0

                // sfx
                SFXComponent.Instance.SFXPlayer(8);
            }
        }
    }
}