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
                    WaitOnAudioFade(2, 3);
                    //FadeToLevel(3);
                    break;
                case 3:
                    if (PlayerManager.Instance.Alubiat || _alubiat)
                    {
                        WaitOnAudioFade(2, 4);
                        //FadeToLevel(4);
                    }   
                    else
                    {
                        WaitOnAudioFade(2, 5);
                        //FadeToLevel(5);
                    }
                        
                    break;
                case 4:
                    WaitOnAudioFade(2, 6);
                    //FadeToLevel(6);
                    break;
                case 5:
                    WaitOnAudioFade(2, 8);
                    //FadeToLevel(8);
                    break;
                case 6:
                    WaitOnAudioFade(2, 7);
                    //FadeToLevel(7);
                    break;
                case 7:
                    WaitOnAudioFade(2, 8);
                    //FadeToLevel(8);
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
        // para todas las corrutinas
        StopAllCoroutines();

        // empieza la corrutina para esperar para cambiar de escena
        StartCoroutine(CoroutineWaitOnFade(i, whatLvl));

        // empieza la corrutina para el fade out
        AudioManager.Instance.FadeBGM(i);
        

    }


    IEnumerator CoroutineWaitOnFade(int i, int  whatLvl)
    {
        // cosas de corrutinas, espera la cantidad de tiempo indicada
        yield return new WaitForSecondsRealtime(i);

        // cambia de escena
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
