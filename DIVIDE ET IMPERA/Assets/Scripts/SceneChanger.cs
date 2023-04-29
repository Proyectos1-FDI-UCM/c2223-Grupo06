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
                case 0:                    // case [sala actual] 
                    WaitOnAudioFade(1, 1); // WaitOnAudioFade([tiempo de espera], [sala a la que vas]) || //FadeToLevel([sala a la que vas]);
                    break;
                case 1:
                    WaitOnAudioFade(1, 2);
                    break;
                case 2:
                    WaitOnAudioFade(1, 3);
                    break;
                case 3:
                    if (GameManager.Instance != null)
                        if (GameManager.Instance.Alubiat || _alubiat)
                            WaitOnAudioFade(1, 4); // tienes alubiat
                        else
                            WaitOnAudioFade(1, 5); // no tienes alubiat
                    break;
                case 4:
                    WaitOnAudioFade(1, 6);
                    break;
                case 5:
                    WaitOnAudioFade(1, 8);
                    break;
                case 6:
                    WaitOnAudioFade(1, 7);
                    break;
                case 7:
                    WaitOnAudioFade(1, 8); // 8 es puntos creo
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
                case 0: // 1_INICIO
                    estado = GameManager.GameStates.START;
                    break;
                case 1: // 2_TUTORIAL
                    estado = GameManager.GameStates.GAME;
                    break;
                case 2: // 3_PRINCIPAL
                    estado = GameManager.GameStates.GAME;
                    break;
                case 3: // 4_ALUBIA
                    estado = GameManager.GameStates.INTRO;
                    break;
                case 4: // 5.1_BOB_BUENO
                    estado = GameManager.GameStates.INTRO;
                    break;
                case 5: // 5.2_BOB_MALO
                    estado = GameManager.GameStates.INTRO;
                    break;
                case 6: // 6_PUZLE
                    estado = GameManager.GameStates.GAME;
                    break;
                case 7: // 7_DESPEDIDA
                    estado = GameManager.GameStates.GAMEOVER;
                    break;
                case 8: // 8_PUNTOS
                    estado = GameManager.GameStates.SCORE;
                    break;
                case 9: // 9_CREDITOS
                    estado = GameManager.GameStates.CREDITS;
                    break;

                    // TESTING
                case 10: // 1T INICIO
                    estado = GameManager.GameStates.START;
                    break;   
                case 11: // 2T TUTORIAL
                    estado = GameManager.GameStates.GAME;
                    break;
                case 12: // 3T PUNTOS
                    estado = GameManager.GameStates.SCORE;
                    break;       
                    
                    // LOGO  /  WIP
                case 13: // 0 LOGO
                    estado = GameManager.GameStates.INTRO; // ???????????
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
