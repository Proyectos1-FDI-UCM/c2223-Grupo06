using UnityEngine;

public class DoorComponent : MonoBehaviour
{
    #region Methods
    private void Activar()
    {
        if (PlayerManager.Instance.Objeto == PlayerManager.Objetos.LLAVE)
        {
            gameObject.SetActive(false);
            PlayerManager.Instance.EliminarObjeto();
        }
    }
    #endregion
}
