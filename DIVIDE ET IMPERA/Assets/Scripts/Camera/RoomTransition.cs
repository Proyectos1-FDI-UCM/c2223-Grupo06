using System.Xml.Serialization;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    #region References
    //Transforms camara y jugador
    private Transform _cameraTransform;
    private Transform _player;

    //Aqui empieza lo bueno
    [SerializeField]
    private Transform _roomCameraPosition; //Posicion a la que queremos que la camara se mueva durante la transicion
    [SerializeField]
    private Transform _roomSpawn; //Lugar al que el jugador se movera tras la transicion (ademas servira para spawnearle ahi si quiere resetear la sala)
    #endregion
    #region Parameters
    [SerializeField]
    private float _transitionSpeed; //Velocidad de transicion de la camara, valor alto para que no se note lo que ocurre durante la transicion
    #endregion
    #region Properties
    private Vector3 _futureCamPos; //futura posicion de la camara
    private bool _onTransition; //booleano que determina si esta ocurrinedo una transicion
    GameObject[] _transitions; //Array de todas las transiciones del juego (durante el transcurso de una transicion se desactivan para evitar bugs, antes se rompia todo si entrabas en una transicion estando ya en una)
    #endregion
    #region Methods
    private void OnTriggerEnter2D(Collider2D collision) //Cuando entras en la transicion
    {
        CameraMovement.Instance.enabled = false; //desactivar movimiento de la camara de seguir al jugador

        _player.transform.position = _roomSpawn.position; //mover al jugador, desactivar el movimiento y la animacion para evitar que entre en otra transicion
        PlayerAccess.Instance.MovementComponent.enabled= false;
        PlayerAccess.Instance.Animator.enabled= false;

        _futureCamPos = new Vector3(_roomCameraPosition.position.x, _roomCameraPosition.position.y, _cameraTransform.position.z); //calculo posicion futura de la camara

        for (int i = 0; i < _transitions.Length; i++)
        {
            _transitions[i].GetComponentInChildren<BoxCollider2D>().enabled = false; //se desactivan todas las transiciones
        }

        _onTransition = true; //activar transicion
    }

    private void OnTRansition()
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

            _onTransition = false; //Se termina la transicion
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = CameraMovement.Instance.gameObject.transform;
        _player = PlayerAccess.Instance.transform;
        _transitions = GameObject.FindGameObjectsWithTag("Transition"); //Ducktyping lo se pero ahorra mucho no me mateis
    }

    // Update is called once per frame
    void Update()
    {
        if (_onTransition) //si estas en una transicion
        {
            OnTRansition();
        }
    }
}
