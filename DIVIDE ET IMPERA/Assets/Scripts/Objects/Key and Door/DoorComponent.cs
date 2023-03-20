using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorComponent : MonoBehaviour
{
    #region References

    #endregion
    #region Parameters

    #endregion
    #region Properties

    #endregion
    #region Methods
    private void Activar()
    {
        if(PlayerManager.Instance.Objeto == PlayerManager.Objetos.LLAVE)
        {
            gameObject.SetActive(false);
            PlayerManager.Instance.EliminarObjeto();
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
