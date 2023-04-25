using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region References
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    private GameObject _currentLevel;
    public GameObject CurrentLevel { get { return _currentLevel; } }

    [SerializeField]
    private GameObject[] _levelsPrefabs; //array prefabs de niveles
    [SerializeField]
    private GameObject[] _levels; //array de niveles en la escena
    [SerializeField]
    private Transform[] _roomSpawns; //array de spawns del jugador en cada sala (en la primera es el spawn original y
                                     //en las demás coincide con los spanws de las transiciones)
    [SerializeField]
    private Transform _originalSpawn; //spwan original del jugador

    [SerializeField]
    private GameObject _objectsReset; //objeto padre de los objetos/partes soltados/lanzados
    #endregion
    #region Properties
    private int _currentLevelNum; //indice de la sala
    public int CurrentLevelNum { get { return _currentLevelNum; } }

    private Transform _roomSpawn; //spawn actual
    private float _currentHealth; //valor de la vida al entrar en la sala

    private int _objectRoomIndex;
    #endregion
    #region Methods
    public void ChangeLevelIndex(int index)
    {
        _currentLevelNum = index;
        UpdateCurrentLevel();
    }

    public void ResetCurrentLevel() //resetea sala actual
    {
        ResetRoom(_currentLevelNum); //resetea sala

        ResetPlayer(); //devuelve al jugador a las condiciones originales

        ResetObjects(); //resetea objetos y partes lanzadas/soltadas

        ResetConnectedParts(); //resetea objetos connectados si los hay

        UpdateCurrentLevel(); //actualiza datos nivel

        if (GameManager.Instance != null)
        {
            Debug.Log("reset history: " + GameManager.Instance.ResetCounter);
            GameManager.Instance.ResetCounter++;
        }
    }

    private void ResetRoom(int levelNum)
    {
        Vector3 lvlTransform = _levels[levelNum].transform.position; //almacena posicion de la sala en la escena
        Destroy(_levels[levelNum]); //destruye sala
        _levels[levelNum] = Instantiate(_levelsPrefabs[levelNum]); //instancia la sala desde su prefab
        _levels[levelNum].transform.position = lvlTransform; //mueve la sala a la posicion de la sala antes de resetearse
                                                             //se rompia la instanciacion por algun motivo si se instanciaba ahi directamente
    }

    private void ResetPlayer()
    {
        if (PlayerManager.Instance._partInControl.GetComponent<PataformaComponent>()) //Si estas controlando la pataforma hace los cambios de control al player
        {
            PlayerManager.Instance._partInControl.GetComponent<PataformaComponent>().PlayerInControl();
        }
        PlayerManager.Instance.ChangePartInControl(PlayerAccess.Instance.gameObject); //cambia el control al jugador en caso de estar controlando piernas
        PlayerAccess.Instance.transform.parent = null; //adoptiont por si esta en plataforma
        PlayerAccess.Instance.transform.position = _roomSpawn.position; //mueve player al spawn
        PlayerManager.Instance.RequestTimmyState(PlayerManager.TimmyStates.S0); //devuelve al player al estado original

        if (_objectRoomIndex == _currentLevelNum)
        {
            PlayerManager.Instance.EliminarObjeto(); //elimina objetos
            PlayerManager.Instance.CambiarObjeto(PlayerManager.Objetos.NADA);
        }
        PlayerAccess.Instance.BoneBar.SetBar(_currentHealth); //elimina daño de caida acumulado

        PlayerAccess.Instance.InputController.ResetProperties();
    }

    public void ResetObjects() //Destruir todos los objetos lanzados/soltados porque son hijos de _objectsReset
    {
        int i = _objectsReset.transform.childCount;
        for (int j = 0; j < i; j++)
        {
            Destroy(_objectsReset.transform.GetChild(j).gameObject);
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

    private void UpdateCurrentLevel() //actualiza spawn y nivel al del indice
    {
        _currentLevel = _levels[_currentLevelNum];
        _roomSpawn = _roomSpawns[_currentLevelNum];
        if (PlayerManager.Instance != null) _currentHealth = PlayerAccess.Instance.BoneBar.CurrentBoneState;
    }

    public void GlobalReset()
    {
        for (int i = 0; i < _levels.Length; i++) //Reset de todas las salas
        {
            ResetRoom(i);
        }
        //reset del player devolviendole a condiciones originales
        _roomSpawn = _originalSpawn;
        _currentHealth = PlayerAccess.Instance.BoneBar.MaxBoneState;
        ResetPlayer();

        _currentLevelNum = 0; //actualiza indice
        UpdateCurrentLevel();
    }

    public void ObjectLevelIndex(int index)
    {
        _objectRoomIndex = index;
    }

    #endregion
    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (_originalSpawn != null) PlayerAccess.Instance.transform.position = _originalSpawn.position;
        _currentLevelNum = 0;
        UpdateCurrentLevel();
    }
}
