using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnPataforma : MonoBehaviour
{
    private bool _isOnPlatform;


    #region Methods


    // gameObject.transform.SetParent(collision.gameObject.transform, true);

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PataformaInputComponent>())
        {
            gameObject.transform.SetParent(collision.gameObject.transform, true);
        }
        
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PataformaComponent>())
        {
            gameObject.transform.parent = null;
        }
            
    }
    

    /*
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<PataformaComponent>())
        {
            _isOnPlatform = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<PataformaComponent>())
        {
            _isOnPlatform = false;
        }
    }
    */
    #endregion

}
