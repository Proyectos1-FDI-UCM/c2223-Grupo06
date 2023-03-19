using UnityEngine;
using UnityEngine.WSA;
using UnityEngine.Tilemaps;
using System.Runtime.CompilerServices;

public class StayOnPataforma : MonoBehaviour
{
    private InputController _inputController;


    #region parameters
    // informa si tiene padre o no
    private bool _parent;
    // informa si puede ponerse en la plaraforma
    private bool _stayOn;
    // informa si puede quitarse de la plataforma
    private bool _stayOff;

    #endregion

    #region Metodos basicos
    // mira si tiene padre o no
    private bool ParentCheck(Collision2D col)
    {
        if (col.gameObject.transform.parent == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    // cambia el padre del objeto
    private void Adoption(Collision2D collision)
    {
        // si no tiene padre cambia el padre
        if (ParentCheck(collision))
        {
            gameObject.transform.SetParent(collision.gameObject.transform, true);
        }
        
    }
    // le quita el padre al objeto
    private void Adoptiont(Collision2D collision)
    {
        // si tiene el padre se lo quita
        if (!ParentCheck(collision))
        {
            gameObject.transform.parent = null;
        }
        
    }
    #endregion

    #region Metodos para ver si se deberia quedar en la plataforma

    // mira si es un objeto que se deba quedar en la plataforma
    private bool CheckStayOn(Collision2D collision)
    {
        // devuelve true si es o una pataforma o una plataforma normal con la variable de puerta 
        // desactivada y no es el tilemap
        if ((collision.gameObject.GetComponent<PataformaComponent>()
            || (!collision.gameObject.GetComponent<MovingPlatformComponent>().GetDoorPlatform))
            && !collision.gameObject.GetComponent<Tilemap>())
        {
            // si tiene peso el objeto
            if (gameObject.GetComponent<WeightComponent>()) //&& _inputController.enabled)
            {
                _stayOn = true;
            }
        }

        return _stayOn;
    }

    // mira si es un objeto que esta ya en la plataforma (para quitarlo)
    private bool CheckStayOff(Collision2D collision)
    {
        if ((collision.gameObject.GetComponent<PataformaComponent>()
            || collision.gameObject.GetComponent<MovingPlatformComponent>())
            && !collision.gameObject.GetComponent<Tilemap>())
        {
            _stayOff = true;
        }

        return _stayOff;
    }
    #endregion

    #region Methods
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CheckStayOn(collision))
        {
            Adoption(collision);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (CheckStayOff(collision))
        {
            Adoptiont(collision);
        }
    }
    #endregion

    private void Start()
    {
        _inputController = GetComponent<InputController>();
    }
}
