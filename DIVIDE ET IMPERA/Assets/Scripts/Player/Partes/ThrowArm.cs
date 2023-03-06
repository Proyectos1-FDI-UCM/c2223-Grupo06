using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ThrowArm : MonoBehaviour
{
    #region References
    private PlayerManager _playerManager;
    private Transform _myTransform;

    [SerializeField]
    private GameObject _armPrefab;
    #endregion
    #region Parameters
    [SerializeField]
    private float _horizontalForce;
    [SerializeField]
    private float _verticalForce;  
    #endregion
    #region Properties
    private PlayerManager.TimmyStates _currentState;
    private GameObject _arm;
    private Rigidbody2D _armRB;
    #endregion
    #region Methods
    public void LanzarBrazo()
    {
        if(_currentState != PlayerManager.TimmyStates.S2 && _currentState != PlayerManager.TimmyStates.S5)
        {
            if (_currentState == PlayerManager.TimmyStates.S0)
            {
                Debug.Log("Brazo 1 Lanzado");
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S1);
            }
            if (_currentState == PlayerManager.TimmyStates.S1)
            {
                Debug.Log("Brazo 2 Lanzado");
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S2);
            }
            if (_currentState == PlayerManager.TimmyStates.S3)
            {
                Debug.Log("Brazo 1 Lanzado");
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S4);
            }
            if (_currentState == PlayerManager.TimmyStates.S4)
            {
                Debug.Log("Brazo 2 Lanzado");
                _playerManager.RequestTimmyState(PlayerManager.TimmyStates.S5);
            }

            _arm = Instantiate(_armPrefab, _myTransform.position, _myTransform.rotation);
            _armRB = _arm.GetComponent<Rigidbody2D>();
            _armRB.AddForce(new Vector2(_horizontalForce * 100 * _myTransform.localScale.x, _verticalForce * 100));

        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _playerManager= GetComponent<PlayerManager>();
        _myTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState = PlayerManager.State;
    }
}
