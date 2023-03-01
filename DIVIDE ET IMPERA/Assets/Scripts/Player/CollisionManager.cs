using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionManager : MonoBehaviour
{
    #region parameters
    //----------------LEVER-------------
    public bool _validPalancaHitbox = false; // HE PUESTO LAS VARIABLES PRIVADAS PUBLICAS PARA PODER HACER DEBUGGING MEJOR EN EL INSPECTOR
    public bool ValidPalancaHitbox { get { return _validPalancaHitbox; } }

    //--------
    public bool _validHitbox = false;
    public bool ValidHitbox { get { return _validHitbox; } }

    public Collider2D _objetoColisionado;
    public Collider2D ObjetoColisionado { get { return _objetoColisionado; } }
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //----- si colisiona con la palanca
        if (collision.GetComponent<PalancaComponent>())
        {
            _validPalancaHitbox = true;
        }
        if (collision.GetComponent<Tilemap>() == false) // manera muy rudimentaria de comprobar que la colisión no es con el suelo!
        {
            _validHitbox = true;
            _objetoColisionado = collision;
        } 
    }

    /* Lo dejo por si acaso pero de momento nada
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Tilemap>() == false)
        {
            _objetoColisionado = collision;
        }
    }
    */

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PalancaComponent>())
        {
            _validPalancaHitbox = false;
        }
        if (collision.GetComponent<Tilemap>() == false)
        {
            _validHitbox = false;
            _objetoColisionado = null;
        }
    }

    public bool DestruirBrazo()
    {
        if (_objetoColisionado != null && _objetoColisionado.GetComponentInParent<ArmComponent>() != null) // esto es ducktyping mi gente
        {
            var padre = _objetoColisionado.transform.parent.gameObject;
            Destroy(padre);
            return true;
        }
        else return false;
    }

    public bool DestruirPierna()
    {
        if (_objetoColisionado != null && _objetoColisionado.GetComponentInParent<LegsComponent>() != null) // esto es ducktyping mi gente
        {
            var padre = _objetoColisionado.transform.parent.gameObject;
            Destroy(padre);
            return true;
        }
        else return false;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _validHitbox = false;
        _validPalancaHitbox = false;
        _objetoColisionado = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
