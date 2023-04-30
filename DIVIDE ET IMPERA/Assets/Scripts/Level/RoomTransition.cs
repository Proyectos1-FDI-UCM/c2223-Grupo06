using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    #region References
    //Transforms camara, jugador y transicion
    private Transform _cameraTransform;
    private Transform _playerTransform;
    private Transform _transitionTransform;

    //Aqui empieza lo bueno

    [SerializeField]
    private Transform _leftRoomCameraPosition; //Posicion a la que queremos que la camara se mueva durante la transicion hacia la izquierda
    [SerializeField]
    private Transform _rightRoomCameraPosition; //Posicion a la que queremos que la camara se mueva durante la transicion hacia la derecha
    /*
    [SerializeField]
    private Transform _leftRoomSpawn; //Lugar al que el jugador se movera tras la transicion hacia la izquierda (ademas servira para spawnearle ahi si quiere resetear la sala)
                                      //IMPORTANTE: ponerlo cerquita de la caja de transicion para que la transicion no sea horrible pero
                                      //tampoco mucho para que no entre instantaneamente en la transicion de vuelta
    [SerializeField]
    private Transform _rightRoomSpawn; //Lugar al que el jugador se movera tras la transicion hacia la derecha (ademas servira para spawnearle ahi si quiere resetear la sala)
                                       //IMPORTANTE: ponerlo cerquita de la caja de transicion para que la transicion no sea horrible pero
                                       //tampoco mucho para que no entre instantaneamente en la transicion de vuelta*/
    #endregion
    #region Parameters
    [SerializeField]
    private float _transitionDistance;
    [SerializeField]
    private float _transitionSpeed; //Velocidad de transicion de la camara, valor alto para que no se note lo que ocurre durante la transicion
    #endregion
    #region Properties
    private Vector3 _roomSpawn;
    private Transform _roomCameraPosition;
    private Vector3 _futureCamPos; //futura posicion de la camara
    private bool _onTransition; //booleano que determina si esta ocurrinedo una transicion
    GameObject[] _transitions; //Array de todas las transiciones del juego (durante el transcurso de una transicion se desactivan para evitar bugs, antes se rompia todo si entrabas en una transicion estando ya en una)
    private bool _canGivePoints;
    #endregion
    #region Methods
    private void OnTriggerEnter2D(Collider2D collision) //Cuando entras en la transicion
    {
        LevelManager.Instance.SetAlubia(GameManager.Instance.Alubiat);
        if (_playerTransform.position.x < _transitionTransform.position.x)
        {
            _roomSpawn = new Vector3((float)(_transitionTransform.position.x + _transitionDistance), _playerTransform.position.y, 0); //Si el jugador esta a la izquierda setear para la transicion a la sala derecha
            _roomCameraPosition = _rightRoomCameraPosition;
        }
        else if (_playerTransform.position.x > _transitionTransform.position.x)
        {
            _roomSpawn = new Vector3((float)(_transitionTransform.position.x - _transitionDistance), _playerTransform.position.y, 0); //Si esta a la derecha setear para la transicion a la sala izquierda
            _roomCameraPosition = _leftRoomCameraPosition;
        }

        CameraMovement.Instance.enabled = false; //desactivar movimiento de la camara de seguir al jugador

        _playerTransform.position = _roomSpawn; //mover al jugador, desactivar el movimiento y la animacion para evitar que entre en otra transicion
        PlayerAccess.Instance.MovementComponent.enabled = false;
        PlayerAccess.Instance.Animator.enabled = false;

        _futureCamPos = new Vector3(_roomCameraPosition.position.x, _roomCameraPosition.position.y, _cameraTransform.position.z); //calculo posicion futura de la camara
        for (int i = 0; i < _transitions.Length; i++)
        {
            _transitions[i].GetComponentInChildren<BoxCollider2D>().enabled = false; //se desactivan todas las transiciones
        }
        ResetConnectedParts();
        LevelManager.Instance.ResetParts();
        PlayerManager.Instance.RequestTimmyState(PlayerManager.TimmyStates.S0);

        _onTransition = true; //activar transicion
    }

    private void OnTransition()
    {
        if (_cameraTransform.position.x < _futureCamPos.x - 0.1
            || _cameraTransform.position.x > _futureCamPos.x + 0.1 ||
            _cameraTransform.position.y < _futureCamPos.y - 0.1 || _cameraTransform.position.y > _futureCamPos.y + 0.1) //Mientras la camara no este en la posicion futura (con un intervalo porque con == no funciona)
        {
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _futureCamPos, _transitionSpeed * Time.deltaTime); //Se mueve la camara
        }
        else //Cuando la camara ha llegado ha terminado la transicion, ahora proceso inverso de todo lo anterior
        {
            for (int i = 0; i < _transitions.Length; i++)
            {
                _transitions[i].GetComponent<BoxCollider2D>().enabled = true; //Activar otra vez todas las transiciones
            }
            PlayerAccess.Instance.MovementComponent.enabled = true; //Devolver al jugador movimiento y animaciones
            PlayerAccess.Instance.Animator.enabled = true;

            CameraMovement.Instance.enabled = true; //Hacer que la camara vuelva a seguir al jugador

            if (_canGivePoints)
            {
                _canGivePoints = false;
                // Suma la cantidad de puntos debida dependiendo de las partes que lleve
                if (PlayerManager.State == PlayerManager.TimmyStates.S0)
                {
                    Debug.Log("croqueta amarilla");
                    if (GameManager.Instance != null) GameManager.Instance.AddScore(50);
                }
                else if (PlayerManager.State == PlayerManager.TimmyStates.S1)
                {
                    if (GameManager.Instance != null) GameManager.Instance.AddScore(40);
                }
                else if (PlayerManager.State == PlayerManager.TimmyStates.S2 || PlayerManager.State == PlayerManager.TimmyStates.S3)
                {
                    if (GameManager.Instance != null) GameManager.Instance.AddScore(30);
                }
                else if (PlayerManager.State == PlayerManager.TimmyStates.S4)
                {
                    if (GameManager.Instance != null) GameManager.Instance.AddScore(10);
                }
            }

            _onTransition = false; //Se termina la transicion
        }
    }
    private void ResetConnectedParts()
    {
        if (PlayerManager.Instance.Lever != null) //si estas conectado a una palanca desconectar brazo
        {
            PlayerManager.Instance.Lever.GetComponent<PalancaAnimator>().DesconectarBrazo();
            PlayerManager.Instance.Lever.GetComponent<PalancaComponent>().DesconectarBrazo();
        }

        if (PlayerManager.Instance.Pataforma != null) //si estas conectado a pataforma desconectar piernas
        {
            PlayerManager.Instance.Pataforma.GetComponent<PataformaComponent>().SetLegs(false);
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = CameraMovement.Instance.gameObject.transform;
        _playerTransform = PlayerAccess.Instance.transform;
        _transitionTransform = GetComponent<Transform>();
        _transitions = GameObject.FindGameObjectsWithTag("Transition"); //Ducktyping lo se pero ahorra mucho no me mateis
        _canGivePoints = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_onTransition) //si estas en una transicion
        {
            OnTransition();
        }
    }
}
