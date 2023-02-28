using UnityEngine;

public class PalancaComponent : MonoBehaviour
{

    #region Referencias
    private InputController _inputController;
    private CollisionManager _collisionManager;
    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private GameObject _player;
    #endregion

    #region Parámetros
    [SerializeField]
    private bool _palanca;
    [SerializeField]
    private bool _brazoConectado = false;
    [SerializeField]
    private GameObject _puerta;
    #endregion



    #region Métodos

    // activa o desactiva la palanca dependiendo de su estado anterior
    private bool ActivarPalanca()
    {
        bool _lvr = !_palanca;
        _puerta.SetActive(_palanca);
        return _lvr;
    }
    #endregion



    private void Start()
    {
        _inputController = _player.GetComponent<InputController>();
        _collisionManager = _player.GetComponent<CollisionManager>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // si se ha pulsado la E, el brazo no esta conectado y se esta cerca de la palanca
        if (_inputController.Interactuar && !_brazoConectado && _collisionManager.ValidPalancaHitbox)
        {
            _palanca = ActivarPalanca();
           
        }
        // si se ha pulsado la E y el brazo está conectado
        else if (_inputController.Interactuar && _brazoConectado)
        {
            _palanca = ActivarPalanca();
        }

        //-------CONECTAR BRAZO-------------------
        // se pulsa R y se esta cerca de la palanca
        if (_inputController.ConectarParte && _collisionManager.ValidPalancaHitbox)
        {
            _brazoConectado = true;
            _mySpriteRenderer.color = Color.blue;
        }
        // se pulsa T y se esta lejos de la palanca
        else if (_inputController.RecuperarParte && _collisionManager.ValidPalancaHitbox)
        {
            _brazoConectado = false;
            _mySpriteRenderer.color = Color.white;
        }
    }

}
