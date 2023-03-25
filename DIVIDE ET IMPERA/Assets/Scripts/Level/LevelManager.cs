using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region References
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    private GameObject _currentLevel;
    public GameObject CurrentLevel { get { return _currentLevel; } }

    [SerializeField]
    private GameObject[] _levelsPrefabs;
    [SerializeField]
    private GameObject[] _levels;
    public GameObject[] Levels { get { return _levels; } }
    [SerializeField]
    private Transform[] _roomSpawns;
    public Transform[] RoomSpawns { get { return _roomSpawns; } }

    private GameObject _player;
    private Transform _roomSpawn;
    [SerializeField]
    private Transform _originalSpawn;
    #endregion
    #region Properties
    private int _currentLevelNum;
    public int CurrentLevelNum { get { return _currentLevelNum; } }
    #endregion
    #region Methods
    public void IncrementLevelCounter()
    {
        _currentLevelNum++;
        UpdateCurrentLevel();
    }
    public void DecrementLevelCounter()
    {
        _currentLevelNum--;
        UpdateCurrentLevel();
    }

    public void SetRoomSpawn(Transform roomSpawn)
    {
        _roomSpawn = roomSpawn;
    }

    public void ResetCurrentLevel()
    {
        ResetPlayer();

        Vector3 lvlTransform = _currentLevel.transform.position;
        Destroy(_levels[_currentLevelNum]);
        _levels[_currentLevelNum] = Instantiate(_levelsPrefabs[_currentLevelNum]);
        _levels[_currentLevelNum].transform.position = lvlTransform;

        UpdateCurrentLevel();
    }

    private void ResetPlayer()
    {
        PlayerManager.Instance.ChangePartInControl(_player);
        _player.transform.parent = null;
        _player.transform.position = _roomSpawn.position;
        PlayerManager.Instance.RequestTimmyState(PlayerManager.TimmyStates.S0);
        PlayerManager.Instance.EliminarObjeto();
        PlayerAccess.Instance.BoneBar.ResetBar();
    }

    private void UpdateCurrentLevel()
    {
        _currentLevel = _levels[_currentLevelNum];
        _roomSpawn = _roomSpawns[_currentLevelNum];
    }
    #endregion
    private void Awake()
    {
        _instance= this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentLevelNum = 0;
        _roomSpawn = _originalSpawn;
        _player = PlayerAccess.Instance.gameObject;
        UpdateCurrentLevel();
    }
}
