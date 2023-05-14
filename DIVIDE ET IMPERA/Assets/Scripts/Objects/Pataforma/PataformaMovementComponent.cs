using UnityEngine;

public class PataformaMovementComponent : MonoBehaviour
{

    #region Referencias
    private PataformaComponent _pataformaComponent;
    private InputController _inputController;
    private Rigidbody2D _myRigidbody2D;
    [SerializeField] GameObject _patas;
    [SerializeField] GameObject _player;
    private SpriteRenderer _patasRender;
    private InputController _playerInput;
    #endregion


    #region Parámetros
    [Tooltip("Velocidad del jugador")]
    [SerializeField]
    private float _speed;
    [Tooltip("Velocidad a la que el jugador frena cuando deja de moverse")]
    [SerializeField]
    private float _rozamientoFreno;
    #endregion

    #region Properties
    public int _pDireccion;
    public int PataformaDireccion { get { return _pDireccion; } }
    #endregion


    private void SFXMove()
    {
        if (SFXComponent.Instance != null)
        {
            if (_pataformaComponent.PataformaDireccion != 0)
            {
                if (!SFXComponent.Instance.isPlayingSFX(2))
                    SFXComponent.Instance.SFXPlayer(2);
            }
            else
            {
                SFXComponent.Instance.SFXPlayerStop(2);
            }
        }
    }

    private void Move2()
    {
        if (_pataformaComponent.PataformaDireccion == 1)
        {
            transform.Translate(Vector3.right * Time.deltaTime * _speed);

            _patasRender.flipX = false;
        }
        else if (_pataformaComponent.PataformaDireccion == -1)
        {
            transform.Translate(Vector3.left * Time.deltaTime * _speed);
            _patasRender.flipX = true;
        }

        if (_pataformaComponent._activarPataforma)
        {
            SFXMove();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _pataformaComponent = GetComponentInChildren<PataformaComponent>();
        _myRigidbody2D = GetComponent<Rigidbody2D>();
        _patasRender = _patas.GetComponent<SpriteRenderer>();
        _playerInput = _player.GetComponent<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        _pDireccion = _pataformaComponent.PataformaDireccion;
    }

    private void FixedUpdate()
    {
        Move2();
    }
}
