using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region References
    private Transform _cameraTransform;
    [SerializeField]
    private Transform _playerTransform;
    #endregion
    #region Parameters
    [SerializeField] 
    private float _followSpeed;
    [SerializeField]
    private float _verticalOffset;
    #endregion
    #region Properties
    private Vector3 _futureCamPos;
    #endregion
    #region Methods
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform= GetComponent<Transform>();
        //_playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _futureCamPos = _playerTransform.position + new Vector3(0, _verticalOffset, _cameraTransform.position.z);
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _futureCamPos, _followSpeed * Time.deltaTime);
    }
}
