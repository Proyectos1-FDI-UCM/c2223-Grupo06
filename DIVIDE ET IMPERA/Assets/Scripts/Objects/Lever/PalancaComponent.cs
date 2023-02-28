using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class PalancaComponent : MonoBehaviour
{

    #region Referencias
    private InputController _inputController;
    private CollisionManager _collisionManager;
    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private GameObject _player;
    #endregion

    #region Parámetros
    [SerializeField]
    private bool _palanca;
    private bool _brazoConectado = false;
    [SerializeField]
    private GameObject _puerta;
    #endregion



    #region Métodos

    // activa o desactiva la palanca dependiendo de su estado anterior
    private bool ActivarPalanca()
    {
        bool _lvr = !_palanca;
        _puerta.SetActive(_palanca);
        return _lvr;
    }
    #endregion



    private void Start()
    {
        _inputController = _player.GetComponent<InputController>();
        _collisionManager = _player.GetComponent<CollisionManager>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_inputController.Interactuar && !_brazoConectado && _collisionManager.ValidHitbox)
        {
            _palanca = ActivarPalanca();
            Debug.Log("no brazo");
            Debug.Log(_palanca);
            
        }
        else if(_inputController.Interactuar && _brazoConectado)
        {
            _palanca = ActivarPalanca();
            Debug.Log("brazo");
            Debug.Log(_palanca);
        }
       
        //-------BRAZO CONECTADO O NO-------------------
        if(_inputController.ConectarParte && _collisionManager.ValidHitbox)
        {
            _brazoConectado = true;
            _mySpriteRenderer.color = Color.blue;
        }
        else if(_inputController.RecuperarParte && _collisionManager.ValidHitbox)
        {
            _brazoConectado = false;
            _mySpriteRenderer.color = Color.white;
        }
    }

}
