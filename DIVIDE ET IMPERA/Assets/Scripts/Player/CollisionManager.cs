using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionManager : MonoBehaviour
{
    #region References

    #endregion

    #region Properties
    private bool _validHitbox = false;
    public bool ValidHitbox { get { return _validHitbox; } }

    private Collider2D _parteColisionada;
    public Collider2D ParteColisionada { get { return _parteColisionada; } }

    public Collider2D _objetoColisionado;
    public Collider2D ObjetoColisionado { get { return _objetoColisionado;} }

    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //----- si colisiona con la palanca
        if (collision.GetComponent<PalancaComponent>())
        {
            collision.GetComponent<PalancaComponent>()._validPalancaHitbox = true;
        }
        else if (collision.GetComponent<PataformaComponent>())
        {
            collision.GetComponent<PataformaComponent>()._validPataformaHitbox = true;
        }


        if (collision.GetComponent<Tilemap>() == false) // manera muy rudimentaria de comprobar que la colisión no es con el suelo!
        {
            _validHitbox = true;
            if (collision.gameObject.layer == 9) // Timoteo y sus partes 
            {
                _parteColisionada = collision;
            }
            if (collision.gameObject.layer == 10) // Objetos
            {
                _objetoColisionado = collision;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Tilemap>() == false && collision != null)
        {
            if (collision.gameObject.layer == 9 && _parteColisionada == null)
            {
                _parteColisionada = collision;
            }
            else if (collision.gameObject.layer == 10 && _objetoColisionado == null)
            {
                _objetoColisionado = collision;
            }
        }
    }

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
            if (collision.gameObject.layer == 9) // Timoteo y sus partes 
            {
                _parteColisionada = null;
            }
            else if (collision.gameObject.layer == 10)
            {
                _objetoColisionado = null;
            }
        }
    }

    public bool DestruirBrazo()
    {
        if (_parteColisionada != null && _parteColisionada.GetComponentInParent<ArmComponent>() != null) // esto es ducktyping mi gente
        {
            var padre = _parteColisionada.transform.parent.gameObject;
            Destroy(padre);
            return true;
        }
        else return false;
    }

    public bool DestruirPierna()
    {
        if (_parteColisionada != null && _parteColisionada.GetComponentInParent<LegsComponent>() != null) // esto es ducktyping mi gente
        {
            var padre = _parteColisionada.transform.parent.gameObject;
            Destroy(padre);
            return true;
        }
        else return false;
    }

    public int DestruirObjeto() // 0 llave, 1 muelle, 2 bola
    {
        if (_objetoColisionado != null)
        {
            var padre = _objetoColisionado.transform.gameObject;
            if (_objetoColisionado.GetComponent<KeyComponent>() != null)
            { // si es una llave
                Destroy(padre);
                return 0;
            }
            else if (_objetoColisionado.GetComponentInChildren<SpringComponent>() != null)
            { // si es un muelle
                Destroy(padre);
                return 1;
            }
            else if (_objetoColisionado.GetComponent<BallComponent>() != null)
            { // si es una bola
                padre = _objetoColisionado.transform.parent.gameObject;
                var abuelo = padre.transform.parent.gameObject;
                //Destroy(padre);
                Destroy(abuelo); // estoy probando cosas, esto es horrible y mal hecho
                return 2;
            }
        } // si ninguna de las condiciones se ha cumplido:
        return -1;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _validHitbox = false;
        _parteColisionada = null;
        _objetoColisionado = null;
    }
}
