using UnityEngine;

public class SpringComponent : MonoBehaviour
{
    #region Parameters
    [Tooltip("Fuerza del muelle")]
    [SerializeField]
    private float _springForce;
    #endregion
    #region Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.attachedRigidbody.velocity = new Vector2(collision.attachedRigidbody.velocity.x, _springForce);
    }
    #endregion
}
