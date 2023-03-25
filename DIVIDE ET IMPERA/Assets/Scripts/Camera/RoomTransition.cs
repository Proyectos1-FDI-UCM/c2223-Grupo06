using System.Xml.Serialization;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    #region References
    private Transform _cameraTransform;
    private Transform _player;
    [SerializeField]
    private Transform _roomCameraPosition;
    [SerializeField]
    private Transform _roomSpawn;
    #endregion
    #region Parameters
    [SerializeField]
    private float _transitionSpeed;
    #endregion
    #region Properties
    private Vector3 _futureCamPos;
    private bool _onTransition;
    GameObject[] _transitions;
    #endregion
    #region Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CameraMovement.Instance.enabled = false;
        _player.transform.position = _roomSpawn.position;
        PlayerAccess.Instance.MovementComponent.enabled= false;
        PlayerAccess.Instance.Animator.enabled= false;
        _futureCamPos = new Vector3(_roomCameraPosition.position.x, _roomCameraPosition.position.y, _cameraTransform.position.z);
        _onTransition = true;
        for (int i = 0; i < _transitions.Length; i++)
        {
            _transitions[i].GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
    }

    private void CameraTransitionMovement()
    {
        if (_cameraTransform.position.x < _futureCamPos.x - 0.1
            || _cameraTransform.position.x > _futureCamPos.x + 0.1 || 
            _cameraTransform.position.y < _futureCamPos.y - 0.1 || _cameraTransform.position.y > _futureCamPos.y + 0.1)
        {
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _futureCamPos, _transitionSpeed * Time.deltaTime);
        }
        else
        {
            for (int i = 0; i < _transitions.Length; i++)
            {
                _transitions[i].GetComponent<BoxCollider2D>().enabled = true;
            }
            PlayerAccess.Instance.MovementComponent.enabled = true;
            PlayerAccess.Instance.Animator.enabled = true;
            CameraMovement.Instance.enabled = true;
            _onTransition = false;
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = CameraMovement.Instance.gameObject.transform;
        _player = PlayerAccess.Instance.transform;
        _transitions = GameObject.FindGameObjectsWithTag("Transition");
    }

    // Update is called once per frame
    void Update()
    {
        if (_onTransition)
        {
            CameraTransitionMovement();
        }
    }
}
