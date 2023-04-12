using UnityEngine;
using UnityEngine.SceneManagement;  // para manejar escenas

public class SceneChanger : MonoBehaviour
{
    #region References
    public Animator _animator;
    #endregion

    #region Parameters
    private int _sceneToLoad;
    #endregion

    #region Properties

    #endregion

    #region Methods
    public void FadeToLevel(int _levelIndex)  // el level index es el numero que tienen las escenas en los build settings
    {
        _sceneToLoad = _levelIndex; // guarda el index en scene to load
        _animator.SetTrigger("FadeOut"); // animacion de fade out
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(_sceneToLoad); // carga nueva escena
    }
    #endregion

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FadeToLevel(1);
        }
    }
}