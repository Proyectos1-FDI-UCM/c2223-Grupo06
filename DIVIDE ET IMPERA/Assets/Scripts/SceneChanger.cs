using System.Collections;
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
        if (collision.gameObject == _player)
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    WaitOnAudioFade(2, 1);
                    //FadeToLevel(1);
                    break;
                case 1:
                    WaitOnAudioFade(2, 2);
                    //FadeToLevel(2);
                    break;
                case 2:
                    FadeToLevel(3);
                    break;
                case 3:
                    if (PlayerManager.Instance.Alubiat || _alubiat)
                        FadeToLevel(4);
                    else 
                        FadeToLevel(5);
                    break;
                case 4:
                    FadeToLevel(6);
                    break;
                case 5:
                    FadeToLevel(8);
                    break;
                case 6:
                    FadeToLevel(7);
                    break;
                case 7:
                    FadeToLevel(8);
                    break;
            }
        }
    }

    public void FadeToLevel(int _sceneBuildIndex)  // el level index es el numero que tienen las escenas en los build settings
    {
        _sceneToLoad = _sceneBuildIndex; // guarda el index en scene to load
        SceneManager.LoadScene(_sceneToLoad);
        _animator.SetTrigger("FadeOut"); // animacion de fade out
       if (GameManager.Instance != null)
            GameManager.Instance.RequestStateChange(GameManager.GameStates.GAME);
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        if (GameManager.Instance != null)
            GameManager.Instance.RequestStateChange(GameManager.GameStates.GAME);
    }

    public void OnFadeComplete() // triggereado con el animator
    {
        SceneManager.LoadScene(_sceneToLoad); // carga nueva escena
    }

    public void WaitOnAudioFade(int i, int whatLvl)
    {
        StopAllCoroutines();
        StartCoroutine(CoroutineWaitOnFade(i, whatLvl));
        AudioManager.Instance.FadeBGM2(2);
        

    }


    IEnumerator CoroutineWaitOnFade(int i, int  whatLvl)
    {
        yield return new WaitForSecondsRealtime(i);

        FadeToLevel(whatLvl);
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
