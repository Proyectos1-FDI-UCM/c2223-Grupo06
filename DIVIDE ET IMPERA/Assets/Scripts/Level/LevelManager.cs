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
    private GameObject _objectsReset;
    #endregion
    #region Properties
    private int _currentLevelNum; //indice de la sala
    private Transform _roomSpawn; //spawn actual
    private float _currentHealth;
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

        ResetRoom(_currentLevelNum);

        ResetPlayer(); //devuelve al jugador a las condiciones originales

        ResetObjects();

        ResetConnectedParts();

        UpdateCurrentLevel(); //actualiza nivel
    }

    private void ResetRoom(int levelNum)
    {
        Vector3 lvlTransform = _levels[levelNum].transform.position; //almacena posicion de la sala en la escena
        Destroy(_levels[levelNum]); //destruye sala
        _levels[levelNum] = Instantiate(_levelsPrefabs[levelNum]); //instancia la sala desde su prefab
        _levels[levelNum].transform.position = lvlTransform; //mueve la sala a la posicion de la sala antes de resetearse
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
        PlayerAccess.Instance.BoneBar.SetBar(_currentHealth); //elimina daño de caida acumulado

        PlayerAccess.Instance.InputController.ResetThisShit();
    }
    private void ResetObjects()
    {
        int i = _objectsReset.transform.childCount;
        for (int j = 0; j < i; j++)
        {
            Destroy(_objectsReset.transform.GetChild(j).gameObject);
        }
    }

    private void ResetConnectedParts()
    {
        if (PlayerManager.Instance.Lever != null)
        {
            PlayerManager.Instance.Lever.GetComponent<PalancaAnimator>().DesconectarBrazo();
            PlayerManager.Instance.Lever.GetComponent<PalancaComponent>().DesconectarBrazo();
        }
        
        if (PlayerManager.Instance.Pataforma != null)
        {
            PlayerManager.Instance.Pataforma.GetComponent<PataformaComponent>().DesconectaLasPutasPiernas();
        }
    }

    private void UpdateCurrentLevel() //actualiza spawn y nivel al del indice
    {
        _currentLevel = _levels[_currentLevelNum];
        _roomSpawn = _roomSpawns[_currentLevelNum];
        _currentHealth = PlayerAccess.Instance.BoneBar.CurrentBoneState;
    }

    public void GlobalReset()
    {
        for (int i = 0; i < _levels.Length; i++)
        {
            ResetRoom(i);
        }

        _roomSpawn = _originalSpawn;
        _currentHealth = PlayerAccess.Instance.BoneBar.MaxBoneState;
        ResetPlayer();

        _currentLevelNum= 0;
        UpdateCurrentLevel();
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
