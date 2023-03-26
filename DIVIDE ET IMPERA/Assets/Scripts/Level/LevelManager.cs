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
    #endregion
    #region Properties
    private int _currentLevelNum; //indice de la sala
    private Transform _roomSpawn; //spawn actual
    #endregion
    #region Methods
    public void IncrementLevelCounter() //aumenta indice y actualiza el nivel en el que te encuentras
    {
        _currentLevelNum++; 
        UpdateCurrentLevel();
    }
    public void DecrementLevelCounter() //decrementa indice y actualiza el nivel en el que te encuentras
    {
        _currentLevelNum--;
        UpdateCurrentLevel();
    }

    public void ResetCurrentLevel()
    {
        ResetPlayer(); //devuelve al jugador a las condiciones originales

        Vector3 lvlTransform = _levels[_currentLevelNum].transform.position; //almacena posicion de la sala en la escena
        Destroy(_levels[_currentLevelNum]); //destruye sala
        _levels[_currentLevelNum] = Instantiate(_levelsPrefabs[_currentLevelNum]); //instancia la sala desde su prefab
        _levels[_currentLevelNum].transform.position = lvlTransform; //mueve la sala a la posicion de la sala antes de resetearse

        UpdateCurrentLevel(); //actualiza nivel
    }

    private void ResetPlayer()
    {
        if(PlayerManager.Instance._partInControl.GetComponent<PataformaComponent>())
        {
            PlayerManager.Instance._partInControl.GetComponent<PataformaComponent>().PlayerInControl();
        }
        PlayerManager.Instance.ChangePartInControl(PlayerAccess.Instance.gameObject); //cambia el control al jugador en caso de estar controlando piernas
        PlayerAccess.Instance.transform.parent = null; //adoptiont por si esta en plataforma
        PlayerAccess.Instance.transform.position = _roomSpawn.position; //mueve player al spawn
        PlayerManager.Instance.RequestTimmyState(PlayerManager.TimmyStates.S0); //devuelve al player al estado original
        PlayerManager.Instance.EliminarObjeto(); //elimina objetos
        PlayerAccess.Instance.BoneBar.ResetBar(); //elimina daño de caida acumulado
    }

    private void UpdateCurrentLevel() //actualiza spawn y nivel al del indice
    {
        _currentLevel = _levels[_currentLevelNum];
        _roomSpawn = _roomSpawns[_currentLevelNum];
    }
    #endregion
    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentLevelNum = 0;
        UpdateCurrentLevel();
        _roomSpawn = _originalSpawn;
	}
}
