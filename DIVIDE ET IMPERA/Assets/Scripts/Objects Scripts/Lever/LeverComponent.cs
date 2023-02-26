using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverComponent : MonoBehaviour
{
    #region Referencias
    private InputController _inputController;
    [SerializeField]
    private GameObject _player;
    #endregion

    #region Parámetros
    [SerializeField]
    private bool _palanca;
    #endregion

    #region Métodos

    // activa o desactiva la palanca dependiendo de su estado anterior
    private bool ActivarPalanca()
    {
        bool _lvr = !_palanca;
        return _lvr;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_player.GetComponent<InputController>().Interactuar)
        {
            _palanca = ActivarPalanca();
            Debug.Log(_palanca);
            //_inputController._activarPalanca = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _inputController = _player.GetComponent<InputController>();
    }
}
