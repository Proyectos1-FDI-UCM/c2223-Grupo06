using UnityEngine;

public class DoorComponent : MonoBehaviour
{
    #region Methods
    private void Activar()
    {
        if (PlayerManager.Instance.Objeto == PlayerManager.Objetos.LLAVE)
        {
            GetComponent<NewPlatformMovement>().OnOff(true);
            PlayerManager.Instance.EliminarObjeto();
        }
    }
    #endregion
}
