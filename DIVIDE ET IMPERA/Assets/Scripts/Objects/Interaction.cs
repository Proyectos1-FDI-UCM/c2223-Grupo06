using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    #region References
    private GameObject _player;
    private InputController _input;
    #endregion
    #region Properties
    private bool _canInteract; //booleano que controla si el jugador puede interactuar con el objeto
    #endregion
    #region Methods
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject == _player) //filtro para que solo el jugador pueda interactuar con cosas
            _canInteract= true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _player) //filtro para que solo el jugador pueda interactuar con cosas
            _canInteract = false;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _player = PlayerAccess.Instance.gameObject;
        _input= _player.GetComponent<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canInteract && _input.Interactuar) //si puede interactuar y realiza la accion
            SendMessage("Activar"); //uso de un SendMessage ya que se quiere generalizar todo lo posible y que funcione para todas las interacciones del juego

        //---------MUY IMPORTANTE----------

        /* Para que este script funcione, en cada futuro script de acciones interactuables (como por ejemplo el dialogo con algun personaje) 
         lo que se quiera activar en dicha interaccion deberá estar en un método llamado "Activar" para que el SendMessage funcione */
    }
}
