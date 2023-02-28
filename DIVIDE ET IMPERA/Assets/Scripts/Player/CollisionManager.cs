using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    #region parameters
    private Collider2D _objetoColisionado;


    //----------------LEVER-------------
    private bool _validPalancaHitbox = false;
    public bool ValidPalancaHitbox { get { return _validPalancaHitbox; } }
    



    //--------
    private bool _validHitbox = false;
    public bool ValidHitbox { get { return _validHitbox; } }
    
    #endregion

    #region methods



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //----- si colisiona con la palanca
        if (collision.GetComponent<PalancaComponent>())
        {
            _validPalancaHitbox = true;
        }
        _validHitbox = true;

        _objetoColisionado = collision;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _objetoColisionado = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PalancaComponent>())
        {
            _validPalancaHitbox = false;
        }
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
        }
        else
        {
            return false;
        }
    }
    #endregion

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
