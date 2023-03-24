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
    private float _springForce; //Fuerza con la que el muelle hace rebotar las cosas
    #endregion
    #region Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _animator.Play("Boing"); //animacion
        collision.attachedRigidbody.velocity = new Vector2(collision.attachedRigidbody.velocity.x, _springForce); //modifica velocidad del objeto
                                                                                                                  //para que el salto sea consistente
                                                                                                                  //(con fuerzas no lo era)
    }
    #endregion
    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
    }
}
