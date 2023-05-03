using UnityEngine;

public class StayOnPataforma : MonoBehaviour
{
    #region References
    private InputController _inputController;
    private Transform _originalParent;
    #endregion

    #region parameters
    // informa si puede ponerse en la plaraforma
    private bool _stayOn;
    // informa si funciona como puerta
    private bool _puerta;

    #endregion

    #region Metodos basicos
    // mira si tiene padre o no (este objeto)


    public bool ParentCheck()
    {
        if (transform.parent == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // mira si es una puerta la plataforma 
    private bool PuertaCheck(Collision2D col)
    {
        if (col.gameObject.GetComponentInChildren<DoorComponent>())
        {
            _puerta = true;
        }
        else
        {
            _puerta = false;
        }
        return _puerta;
    }
    // cambia el padre del objeto
    private void Adoption(Collision2D collision)
    {
        gameObject.transform.SetParent(collision.gameObject.transform, true);
        if (collision.gameObject.GetComponentInParent<NewPlatformMovement>())
            gameObject.transform.SetParent(collision.gameObject.transform.parent, true);

        /*
    if (collision.transform.parent != null) //dentro de esto estaba lo de encima, por que? a, pero ahora funciona
    {
    }*/
    }
    // le quita el padre al objeto
    private void Adoptiont(Collision2D collision)
    {
        // padren't
        if(gameObject.activeSelf)
        gameObject.transform.parent = _originalParent;
    }
    #endregion

    #region Metodos para ver si se deberia quedar en la plataforma

    // mira si es un objeto que se deba quedar en la plataforma
    public bool CheckStayOn(Collision2D collision)
    {
        // devuelve true si es o una pataforma o una plataforma normal con la variable de puerta 
        // desactivada y no es el tilemap
        if ((collision.gameObject.GetComponentInChildren<PataformaComponent>()
            || collision.gameObject.GetComponentInParent<NewPlatformMovement>()
            || collision.gameObject.GetComponent<NewPlatformMovement>())
            && !collision.gameObject.GetComponentInChildren<DoorComponent>())
        /*&& !collision.gameObject.GetComponent<Tilemap>()
        && !collision.gameObject.GetComponentInChildren<StayOnPataforma>())*/
        {
            //Debug.Log(PuertaCheck(collision));
            // si tiene peso el objeto
            if (gameObject.GetComponent<WeightComponent>()) //&& _inputController.enabled)
            {
                _stayOn = true;
            }
        }
        else
        {
            _stayOn = false;
        }
        return _stayOn;
    }

    #endregion

    #region Metodos principales

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CheckStayOn(collision))
        {
            Adoption(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (CheckStayOn(collision))
        {
            Debug.Log("stayon");
            Adoptiont(collision);
        }
    }
    #endregion

    private void Start()
    {
        _inputController = GetComponent<InputController>();
        _puerta = false;
        _originalParent = transform.parent;
    }
}
