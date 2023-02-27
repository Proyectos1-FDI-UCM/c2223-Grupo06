using UnityEngine;

public class LeverComponent : MonoBehaviour
{
    #region Referencias
    private InputController _inputController;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private GameObject _player;
    #endregion

    #region Parámetros
    [SerializeField]
    private bool _palanca;
    #endregion

    #region Métodos

    // activa o desactiva la palanca dependiendo de su estado anterior
    private bool ActivarPalanca()
    {
        bool _lvr = !_palanca;
        return _lvr;
    }
    #endregion

    private void OnTriggerStay2D()
    {
        if (_inputController.Interactuar)
        {
            _palanca = ActivarPalanca();
            Debug.Log(_palanca);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _inputController = _player.GetComponent<InputController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
