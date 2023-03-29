using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDetection : MonoBehaviour
{
    #region Parameters
    [SerializeField]
    private int _levelIndex;
    #endregion
    #region Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager.Instance.ChangeLevelIndex(_levelIndex);
    }
    #endregion
}
