using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    #region Parameters
    private bool _validHitbox = false;
    public bool ValidHitbox { get { return _validHitbox; } }
    private Collider2D _objetoColisionado;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _validHitbox = true;
        _objetoColisionado = collision;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
         _objetoColisionado = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _validHitbox = false;
        _objetoColisionado = null;
    }

    public bool DestruirBrazo()
    {
        if (_objetoColisionado.GetComponentInParent<ArmComponent>() != null) // esto es ducktyping mi gente
        {
            var padre = _objetoColisionado.transform.parent.gameObject;
            Destroy(padre);
            return true;
        } else
        {
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _objetoColisionado = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
