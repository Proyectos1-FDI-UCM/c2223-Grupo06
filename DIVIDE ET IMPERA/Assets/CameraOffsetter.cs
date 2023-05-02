using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffsetter : MonoBehaviour
{
    [SerializeField] float _amount;
    float _previous;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Camera.main != null && collision.GetComponent<PlayerAccess>() != null)
        {
            if (Camera.main.GetComponent<CameraMovement>() != null)
            {
                var _cameraComponent = Camera.main.GetComponent<CameraMovement>();
                _previous = _cameraComponent.HorizontalOffset;
                _cameraComponent.HorizontalOffset = _amount;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Camera.main != null && collision.GetComponent<PlayerAccess>() != null)
        {
            if (Camera.main.GetComponent<CameraMovement>() != null)
            {
                var _cameraComponent = Camera.main.GetComponent<CameraMovement>();
                _cameraComponent.HorizontalOffset = _previous;
                Debug.Log(_previous);
            }
        }
    }

    private void Start()
    {
        _previous = 3;
    }
}
