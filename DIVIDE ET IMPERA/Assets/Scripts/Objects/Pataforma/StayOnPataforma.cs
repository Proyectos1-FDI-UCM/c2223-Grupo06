using UnityEngine;
using UnityEngine.WSA;
using UnityEngine.Tilemaps;
using System.Runtime.CompilerServices;

public class StayOnPataforma : MonoBehaviour
{
    private InputController _inputController;


    #region Methods

    /*
     * ojo cuidado intento sus de hacer algo pero es una cosa no tan curiosa (esta mal)
    private bool QueEsEso(Collision2D col)
    {
        {col.gameObject.GetComponent<PlayerManager>() 
        || col.gameObject.GetComponent<BallComponent>()
        || col.gameObject.GetComponent<SpringComponent>()
        }
        return true;
    }
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("enter "+collision.gameObject);
        if ((collision.gameObject.GetComponent<PataformaComponent>()
            || collision.gameObject.GetComponent<MovingPlatformComponent>())
            && !collision.gameObject.GetComponent<Tilemap>())
        {
            Debug.Log("uwu");
            if (gameObject.GetComponent<WeightComponent>() && _inputController.enabled)
            {
                Debug.Log("ùwú");
                // cambia el padre de timmy (no alubia, otro)
                gameObject.transform.SetParent(collision.gameObject.transform, true);
            }
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
