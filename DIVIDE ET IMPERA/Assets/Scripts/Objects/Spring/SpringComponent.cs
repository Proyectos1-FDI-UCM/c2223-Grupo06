using UnityEditor.Animations;
using UnityEngine;

public class SpringComponent : MonoBehaviour
{
    #region References
    private Animator _animator;
    #endregion
    #region Parameters
    [Tooltip("Fuerza del muelle")]
    [SerializeField]
    private float _springForce;
    #endregion
    #region Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _animator.Play("Boing");
        collision.attachedRigidbody.velocity = new Vector2(collision.attachedRigidbody.velocity.x, _springForce);
    }
    #endregion
    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
    }
}
