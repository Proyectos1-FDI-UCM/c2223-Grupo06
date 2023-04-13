using UnityEngine;
using UnityEngine.SceneManagement;  // para manejar escenas

public class SceneChanger : MonoBehaviour
{
    #region References
    public Animator _animator;
    private PlayerManager _playerManager;
    [SerializeField] private GameObject _player;
    #endregion

    #region Parameters
    private int _sceneToLoad;
    Scene _activeScene;
    #endregion

    #region Properties
    [SerializeField]
    private bool _alubiat;
    #endregion

    #region Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _player && SceneManager.GetActiveScene().buildIndex == 0)
        {
            FadeToLevel(1);
        }
        else if (collision.gameObject == _player && SceneManager.GetActiveScene().buildIndex == 1) 
        {
            FadeToLevel(2);
        }
        else if (collision.gameObject == _player && SceneManager.GetActiveScene().buildIndex == 2)
        {
            FadeToLevel(3);
        }
        else if (collision.gameObject == _player && SceneManager.GetActiveScene().buildIndex == 3 && _alubiat)
        {
            FadeToLevel(4);
        }
        else if (collision.gameObject == _player && SceneManager.GetActiveScene().buildIndex == 3)
        {
            FadeToLevel(5);
        }
        else if (collision.gameObject == _player && SceneManager.GetActiveScene().buildIndex == 5)
        {
            FadeToLevel(8);
        }
        else if (collision.gameObject == _player && SceneManager.GetActiveScene().buildIndex == 4)
        {
            FadeToLevel(6);
        }
        else if (collision.gameObject == _player && SceneManager.GetActiveScene().buildIndex == 6)
        {
            FadeToLevel(7);
        }
        else if (collision.gameObject == _player && SceneManager.GetActiveScene().buildIndex == 7)
        {
            FadeToLevel(8);
        }
        else if (collision.gameObject == _player && SceneManager.GetActiveScene().buildIndex == 4)
        {
            FadeToLevel(6);
        }
    }

    public void FadeToLevel(int _sceneBuildIndex)  // el level index es el numero que tienen las escenas en los build settings
    {
        _sceneToLoad = _sceneBuildIndex; // guarda el index en scene to load
        SceneManager.LoadScene(_sceneToLoad);
        _animator.SetTrigger("FadeOut"); // animacion de fade out
       GameManager.Instance.RequestStateChange(GameManager.GameStates.GAME);
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Instance.RequestStateChange(GameManager.GameStates.GAME);
    }

    public void OnFadeComplete() // triggereado con el animator
    {
        SceneManager.LoadScene(_sceneToLoad); // carga nueva escena
    }
    #endregion

    private void Start()
    {
        if (PlayerManager.Instance != null)
        {
            _playerManager = PlayerAccess.Instance.PlayerManager;
            _alubiat = PlayerManager.Instance.Alubiat;
        }
    }
}
