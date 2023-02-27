using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    #region References
    private Transform _cameraTransform;
    [SerializeField]
    private Transform _roomCameraPosition;
    [SerializeField]
    private Transform _player;
    #endregion
    #region Parameters
    [SerializeField]
    private float _transitionSpeed;
    #endregion
    #region Properties
    private Vector3 _futureCamPos;
    #endregion
    #region Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player.position = transform.position + new Vector3(1, 0, 0);
        _futureCamPos = new Vector3(_roomCameraPosition.position.x, _roomCameraPosition.position.y, _cameraTransform.position.z);
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _futureCamPos, _transitionSpeed * Time.deltaTime);
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
