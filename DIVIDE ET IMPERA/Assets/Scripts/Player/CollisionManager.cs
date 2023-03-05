using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionManager : MonoBehaviour
{
    #region References
    
    #endregion

    #region Properties
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
            collision.GetComponent<PalancaComponent>()._validPalancaHitbox = true;
        }
        else if (collision.GetComponent<WBComponent>())
        {

        }
        else if (collision.GetComponent<PataformaComponent>())
        {
            collision.GetComponent<PataformaComponent>()._validPataformaHitbox = true;
           
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
            collision.GetComponent<PalancaComponent>()._validPalancaHitbox = false;
        }
        else if (collision.GetComponent<PataformaComponent>())
        {
            collision.GetComponent<PataformaComponent>()._validPataformaHitbox = false;
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
        _objetoColisionado = null;
        
    }
}
