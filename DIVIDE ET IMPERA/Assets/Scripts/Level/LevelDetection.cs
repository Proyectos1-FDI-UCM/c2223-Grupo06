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
        if (collision.gameObject == PlayerAccess.Instance.gameObject)
        {
            LevelManager.Instance.ChangeLevelIndex(_levelIndex);
        }
    }
    #endregion
}
