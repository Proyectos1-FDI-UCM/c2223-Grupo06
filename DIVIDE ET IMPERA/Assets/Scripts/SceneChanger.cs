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
    #endregion

    #region Properties
    [SerializeField]
    private bool _alubiat;
    #endregion

    #region Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _player)            //filtro para que solo el jugador pueda interactuar con cosas
        {
            FadeToNextLevel();
        }
    }

    public void FadeToLevel(int _levelIndex)  // el level index es el numero que tienen las escenas en los build settings
    {
        _sceneToLoad = _levelIndex; // guarda el index en scene to load
        _animator.SetTrigger("FadeOut"); // animacion de fade out
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(_sceneToLoad); // carga nueva escena
    }
    #endregion

    private void Start()
    {
        _playerManager = PlayerAccess.Instance.PlayerManager;
        _alubiat = PlayerManager.Instance.Alubiat;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // FadeToLevel(1);
            FadeToNextLevel();
        }
    }
}
