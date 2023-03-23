using UnityEngine;
using UnityEngine.Tilemaps;

public class StayOnPataforma : MonoBehaviour
{
    #region References
    private InputController _inputController;
    #endregion

    #region parameters
    // informa si puede ponerse en la plaraforma
    private bool _stayOn;
    // informa si funciona como puerta
    private bool _puerta;

    #endregion

    #region Metodos basicos
    // mira si tiene padre o no (este objeto)
    private bool ParentCheck()
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
        if (col.gameObject.GetComponent<MovingPlatformComponent>() && col.gameObject.GetComponent<MovingPlatformComponent>().GetDoorPlatform)
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
        // le da un padre
        gameObject.transform.SetParent(collision.gameObject.transform, true);
    }
    // le quita el padre al objeto
    private void Adoptiont(Collision2D collision)
    {
        // padren't
        gameObject.transform.parent = null;
    }
    #endregion

    #region Metodos para ver si se deberia quedar en la plataforma

    // mira si es un objeto que se deba quedar en la plataforma
    private bool CheckStayOn(Collision2D collision)
    {
        // devuelve true si es o una pataforma o una plataforma normal con la variable de puerta 
        // desactivada y no es el tilemap
        if ((collision.gameObject.GetComponent<PataformaComponent>() || !PuertaCheck(collision))
            && !collision.gameObject.GetComponent<Tilemap>()
            && !collision.gameObject.GetComponent<StayOnPataforma>())
        {
            Debug.Log(PuertaCheck(collision));
            //Debug.Log(PuertaCheck(collision));
            // si tiene peso el objeto
            if (gameObject.GetComponent<WeightComponent>()) //&& _inputController.enabled)
            {
                Debug.Log("parte 2");
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
            Debug.Log("me cago en todo");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (CheckStayOn(collision))
        {
            Adoptiont(collision);
        }
    }
    #endregion

    private void Start()
    {
        _inputController = GetComponent<InputController>();
        _puerta = false;
    }
}
