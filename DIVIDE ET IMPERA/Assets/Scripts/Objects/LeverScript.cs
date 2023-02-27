using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{

    #region References
    [SerializeField]
    private InputController _inputController;
    #endregion
    #region Parameters

    #endregion
    #region Properties
    private bool _isActive = false;
    #endregion
    #region Methods

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_inputController.Interactuar)
        {
            _isActive = !_isActive;
            Debug.Log(_isActive);
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //_inputController = PlayerAccess.Instance.InputController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
