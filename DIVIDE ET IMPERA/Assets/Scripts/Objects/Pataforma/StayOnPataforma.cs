using UnityEngine;
using UnityEngine.WSA;
using UnityEngine.Tilemaps;
using System.Runtime.CompilerServices;

public class StayOnPataforma : MonoBehaviour
{
    private InputController _inputController;


    #region parameters
    private bool _parent;
    private bool _stayOn;

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
        gameObject.transform.SetParent(collision.gameObject.transform, true);
    }
    // le quita el padre al objeto
    private void Adoptiont(Collision2D collision)
    {
        gameObject.transform.parent = null;
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
        Debug.Log("exist "+collision.gameObject);
        if ((collision.gameObject.GetComponent<PataformaComponent>()
            || collision.gameObject.GetComponent<MovingPlatformComponent>())
            && !collision.gameObject.GetComponent<Tilemap>())
        {
            // cambia el padre de timmy (no alubia, otro)
            gameObject.transform.parent = null;
        }
    }
    #endregion

    private void Start()
    {
        _inputController = GetComponent<InputController>();
    }
}
