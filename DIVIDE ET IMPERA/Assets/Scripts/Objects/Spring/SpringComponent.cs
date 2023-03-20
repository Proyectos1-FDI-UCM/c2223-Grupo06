using UnityEngine;

public class SpringComponent : MonoBehaviour
{
    #region References

    #endregion
    #region Parameters
    [Tooltip("Fuerza del muelle")]
    [SerializeField]
    private float _springForce;
    #endregion
    #region Properties

    #endregion
    #region Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision.attachedRigidbody.AddForce(new Vector2(collision.attachedRigidbody.velocity.x, _springForce));
        collision.attachedRigidbody.velocity = new Vector2(collision.attachedRigidbody.velocity.x, _springForce);
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
