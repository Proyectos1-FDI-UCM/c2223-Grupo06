using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionManager : MonoBehaviour
{
    #region References
    private GameObject _objectStored;
    public GameObject ObjectStored { get { return _objectStored; } }
    #endregion

    #region Properties
    private bool _validHitbox = false;
    public bool ValidHitbox { get { return _validHitbox; } }

    private Collider2D _parteColisionada;
    public Collider2D ParteColisionada { get { return _parteColisionada; } }

    private Collider2D _objetoColisionado;
    public Collider2D ObjetoColisionado { get { return _objetoColisionado; } }

    private Collider2D _hitboxColisionada;
    public Collider2D HitboxColisionada { get { return _hitboxColisionada; } }
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //----- si colisiona con la palanca
        if (collision.GetComponent<PalancaComponent>())
        {
            collision.GetComponent<PalancaComponent>()._validPalancaHitbox = true;
        }
        else if (collision.GetComponentInParent<PataformaComponent>())
        {
            collision.GetComponentInParent<PataformaComponent>()._validPataformaHitbox = true;
        }

        if (collision.GetComponent<Tilemap>() == false) // manera muy rudimentaria de comprobar que la colisión no es con el suelo!
        {
            _validHitbox = true;
            if (collision.gameObject.layer == 9) // Timoteo y sus partes 
            {
                _parteColisionada = collision;
            }
            else if (collision.gameObject.layer == 10) // Objetos
            {
                _objetoColisionado = collision;
            }
            else
            {
                _hitboxColisionada = collision;
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
            else if (_hitboxColisionada == null)
            {
                _hitboxColisionada = collision;
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
            else
            {
                _hitboxColisionada = null;
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
            if (!PlayerManager.Instance.Piernas)
            {
                var padre = _parteColisionada.transform.parent.gameObject;
                Destroy(padre);
            }

            return true;
        }
        else return false;
    }

    public bool DestruirAlubiat() // método para pulsar tecla delante etc como si fueran piernas normales
    {
        if (_parteColisionada != null && _parteColisionada.GetComponentInParent<AlubiatComponent>() != null)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.Alubiat = true;
            if (!PlayerManager.Instance.Alubiat)
            {
                var padre = _parteColisionada.transform.parent.gameObject;
                Destroy(padre);
            }
            return true;
        }
        else return false;
    }

    public void SetEnabledAndParent(bool newEnabled, Transform newParent)
    {
        transform.SetParent(newParent);
        enabled = newEnabled;
    }

    public int DesactivarObjeto() // 0 llave, 1 muelle, 2 bola
    {
        var obj = -1;
        if (_objetoColisionado != null && !_objetoColisionado.GetComponent<ObjectComponent>().StaticObject)
        {
            _objectStored = _objetoColisionado.transform.gameObject;
            if (_objetoColisionado.GetComponent<KeyComponent>() != null)
            { // si es una llave
                _objectStored.SetActive(false);
                LevelManager.Instance.ObjectLevelIndex(LevelManager.Instance.CurrentLevelNum);
                obj = 0;
            }
            else if (_objetoColisionado.GetComponentInChildren<SpringComponent>() != null)
            { // si es un muelle
                _objectStored.SetActive(false);
                LevelManager.Instance.ObjectLevelIndex(LevelManager.Instance.CurrentLevelNum);
                obj = 1;
            }
            else if (_objetoColisionado.GetComponentInParent<BallComponent>() != null)
            { // si es una bola
                _objectStored = _objetoColisionado.transform.parent.gameObject;
                //SetEnabledAndParent(false, null);
                _objectStored.SetActive(false);
                LevelManager.Instance.ObjectLevelIndex(LevelManager.Instance.CurrentLevelNum);
                obj = 2;
            }
        } // si ninguna de las condiciones se ha cumplido:
        return obj;
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
