using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region References
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }
    #endregion
    #region Parameters

    #endregion
    #region Properties
    private int _currentLevel;
    #endregion
    #region Methods
    public void IncrementLevelCounter()
    {
        _currentLevel++;
    }
    public void DecrementLevelCounter()
    {
        _currentLevel--;
    }
    #endregion
    private void Awake()
    {
        _instance= this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
