using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PataformaComponent : MonoBehaviour
{
    #region Referencias
    private InputController _inputController;
    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private GameObject _player;

    
    #endregion

    #region Properties
    // piernas conectadas a la pataforma 
    private bool _piernasConectadas = false;
    // esta en el área de una pataforma
    public bool _validPataformaHitbox;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _inputController = _player.GetComponent<InputController>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        //---QUITAR Y PONER PARTES------------------------------------
        // si se ha pulsado la E, el brazo está conectado y está en el estado correcto
        if (_inputController.Interactuar && _piernasConectadas
            && (PlayerManager.State == PlayerManager.TimmyStates.S3
            || PlayerManager.State == PlayerManager.TimmyStates.S4
            || PlayerManager.State == PlayerManager.TimmyStates.S5))
        {
            
        }



        //-------CONECTAR PIERNAS-------------------
        // se pulsa R y se esta cerca de la pataforma
        if (_inputController.ConectarParte && _validPataformaHitbox
            && (PlayerManager.State == PlayerManager.TimmyStates.S0
            || PlayerManager.State == PlayerManager.TimmyStates.S1
            || PlayerManager.State == PlayerManager.TimmyStates.S2))
        {
            // conecta el brazo
            _piernasConectadas = true;

            // SI TIENE BRAZOS
            if (PlayerManager.State == PlayerManager.TimmyStates.S0)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S3);
            }
            // SI NO TIENE UN BRAZO
            else if (PlayerManager.State == PlayerManager.TimmyStates.S1)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S4);
            }
            // SI NO TIENE BRAZOS
            else if (PlayerManager.State == PlayerManager.TimmyStates.S2)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S5);
            }


            // cambia el color (deberia ser sprite)
            _mySpriteRenderer.color = Color.blue;
        }
        // se pulsa T, está cerca de la pataforma, está en los estados correctos y hay patas conectadas
        else if (_inputController.RecuperarParte && _validPataformaHitbox
            && (PlayerManager.State == PlayerManager.TimmyStates.S3
            || PlayerManager.State == PlayerManager.TimmyStates.S4
            || PlayerManager.State == PlayerManager.TimmyStates.S5)
            && _piernasConectadas)
        {
            // desconecta el brazo
            _piernasConectadas = false;

            // si tiene los brazos cambia de estado
            if (PlayerManager.State == PlayerManager.TimmyStates.S3)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S0);
            }
            // si no tiene un brazo cambia de estado
            else if (PlayerManager.State == PlayerManager.TimmyStates.S4)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S1);
            }
            // si no tiene brazos cambia de estado
            else if (PlayerManager.State == PlayerManager.TimmyStates.S5)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S2);
            }

            // cambia de color (deberia ser sprite)
            _mySpriteRenderer.color = Color.white;
        }

    }
}
