using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region References
    private Transform _cameraTransform;
    private Transform _followTransform; //transform que la camara sigue

    private static CameraMovement _instance;
    public static CameraMovement Instance { get { return _instance; } }
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
    public void ChangeWhoToFollow(GameObject followObject) //Para cambiar que la camara siga al jugador o a las piernas dependiendo de a quien este controlando
    {
        _followTransform = followObject.GetComponent<Transform>();
    }

    #endregion
    void Awake()
    {
        _instance= this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = GetComponent<Transform>();
        _followTransform = PlayerAccess.Instance.Transform;
    }

    // Update is called once per frame
    void Update()
    {
        _futureCamPos = _followTransform.position + new Vector3(0, _verticalOffset, _cameraTransform.position.z); //calculo de la posicion futura de la camara
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _futureCamPos, _followSpeed * Time.deltaTime); //Lerp entre la posicion de la camara actual y la futura
    }
}
