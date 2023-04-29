using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;  // para manejar escenas

public class SceneChanger : MonoBehaviour
{
    #region References
    public Animator _animator;
    private PlayerManager _playerManager;
    [SerializeField] private GameObject _player;
    private InputController _inputController;
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
            //Debug.Log(SceneManager.GetActiveScene().buildIndex);
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    WaitOnAudioFade(1, 1);
                    break;
                case 1:                    // case [sala actual] 
                    WaitOnAudioFade(1, 2); // WaitOnAudioFade([tiempo de espera], [sala a la que vas]) || //FadeToLevel([sala a la que vas]);
                    break;
                case 2:
                    WaitOnAudioFade(1, 3);
                    break;
                case 3:
                    WaitOnAudioFade(1, 4);
                    break;
                case 4:
                    if (GameManager.Instance != null)
                        if (GameManager.Instance.Alubiat || _alubiat)
                            WaitOnAudioFade(1, 5); // tienes alubiat
                        else
                            WaitOnAudioFade(1, 6); // no tienes alubiat
                    break;
                case 5:
                    WaitOnAudioFade(1, 7);
                    break;
                case 6:
                    WaitOnAudioFade(1, 8);
                    break;
                case 7:
                    WaitOnAudioFade(1, 8);
                    break;
                case 8:
                    WaitOnAudioFade(1, 9); // 9 es puntos creo
                    break;

                // TESTING
                case 11:
                    WaitOnAudioFade(1, 12);
                    break;
            }

        }
    }

    public void FadeToLevel(int _sceneBuildIndex)  // el level index es el numero que tienen las escenas en los build settings
    {
        _sceneToLoad = _sceneBuildIndex;           // guarda el index en scene to load
        SceneManager.LoadScene(_sceneToLoad);
        _animator.SetTrigger("FadeOut");           // animacion de fade out
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

        StartCoroutine(CoroutineWaitOnFadeToGetVolumeBack(i));
    }

    IEnumerator CoroutineWaitOnFadeToGetVolumeBack(float i)
    {
        // cosas de corrutinas, espera la cantidad de tiempo indicada
        yield return new WaitForSecondsRealtime(i);

        // setea de nuevo el bgm
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetVolumeAfterFade();
    }

    IEnumerator CoroutineWaitOnFade(int i, int whatLvl)
    {
        // cosas de corrutinas, espera la cantidad de tiempo indicada
        yield return new WaitForSecondsRealtime(i);

        // cambia de escena
        FadeToLevel(whatLvl);

        // Siguiendo el índice de las escenas en la build, he aqui una chapuza
        if (GameManager.Instance != null) // cambiar después de tal 
        {
            GameManager.GameStates estado = GameManager.GameStates.START;
            switch (whatLvl)
            {
                case 0: // 0 LOGO 
                    estado = GameManager.GameStates.INTRO;
                    break;
                case 1: // 1 INICIO
                    estado = GameManager.GameStates.START;
                    break;
                case 2: // 2 TUTORIAL
                    estado = GameManager.GameStates.GAME;
                    break;
                case 3: // 3 PRINCIPAL
                    estado = GameManager.GameStates.GAME;
                    break;
                case 4: // 4 ALUBIA
                    estado = GameManager.GameStates.INTRO;
                    break;
                case 5: // 5.1 BOB BUENO
                    estado = GameManager.GameStates.INTRO;
                    break;
                case 6: // 5.2 BOB MALO
                    estado = GameManager.GameStates.INTRO;
                    break;
                case 7: // 6 PUZLE
                    estado = GameManager.GameStates.GAME;
                    break;
                case 8: // 7 DESPEDIDA
                    estado = GameManager.GameStates.GAMEOVER;
                    break;
                case 9: // 8 PUNTOS
                    estado = GameManager.GameStates.SCORE;
                    break;
                case 10: // CRÉDITOS
                    estado = GameManager.GameStates.CREDITS;
                    break;

                // TESTING
                case 11: // 1T INCIO
                    estado = GameManager.GameStates.START;
                    break;
                case 12: // 2T TUTORIAL
                    estado = GameManager.GameStates.GAME;
                    break;       
                case 13: // 3T PUTNOS
                    estado = GameManager.GameStates.SCORE; // ???????????
                    break;
            }
            GameManager.Instance.RequestStateChange(estado);
        }
    }
    #endregion

    private void Start()
    {
        if (PlayerManager.Instance != null)
        {
            _playerManager = PlayerAccess.Instance.PlayerManager;
            if (GameManager.Instance != null) _alubiat = GameManager.Instance.Alubiat;
        }

        _inputController = PlayerAccess.Instance.InputController;
    }
}
