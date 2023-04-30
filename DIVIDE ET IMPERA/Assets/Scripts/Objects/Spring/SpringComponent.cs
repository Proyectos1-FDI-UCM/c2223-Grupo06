using UnityEngine;
using UnityEngine.Tilemaps;

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
        // para que no colisione con el tilemap 
        if (!collision.GetComponent<Tilemap>() && collision.GetComponent<Rigidbody2D>()) 
        {
            _animator.Play("Boing"); //animacion
            collision.attachedRigidbody.velocity = new Vector2(collision.attachedRigidbody.velocity.x, _springForce); //modifica velocidad del objeto
                                                                                                                      //para que el salto sea consistente
                                                                                                                      //(con fuerzas no lo era)
                                                                                                                      // sfx
            if (SFXComponent.Instance != null)
                SFXComponent.Instance.SFXObjects(1);
        }
    }
    #endregion

    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
    }
}
