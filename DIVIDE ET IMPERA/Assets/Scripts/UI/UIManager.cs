using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region references
    [SerializeField] private GameObject _StartMenu;
    [SerializeField] private GameObject _HUD;

    // imagenes dentro del ui
    [SerializeField] private Image[] _images;
    // sprites en los assets
    [SerializeField] private Sprite[] _sprites;
    #endregion

    #region properties
    private GameManager.GameStates _activeMenu;          // Menú actual
    private GameObject[] _menus;                         // Array de menús totales

    private int _posCabeza;
    private int _posBrazo1;
    private int _posBrazo2;
    private int _posPiernas;
    private int _posCostillas;
    private int _posAlubiat;
    #endregion

    // MENUS
    public void SetMenu(GameManager.GameStates newMenu)  // Desactiva el menú anterior, actualiza el actual y lo activa
    {
        _menus[(int)_activeMenu].SetActive(false);
        _activeMenu = newMenu;
        _menus[(int)_activeMenu].SetActive(true);
    }

    // PARTES
    public void SetPartes(PlayerManager.TimmyStates state) // Inicializa el HUD
    {
        bool brazo1 = false;
        bool brazo2 = false;
        bool piernas = false;

        if (_images[_posCabeza].sprite != _sprites[_posCabeza * 2 + 1]) // Esto es placeholder, cambio eventual
        {
            _images[_posCabeza].sprite = _sprites[_posCabeza * 2 + 1];
        }

        switch (state)
        { // +1 SI EN ACTIVO, NADA SI INACTIVO
            case PlayerManager.TimmyStates.S0: // todo
                brazo1 = true;
                brazo2 = true;
                piernas = true;
                break;
            case PlayerManager.TimmyStates.S1: // 1 brazo y piernas
                brazo1 = true;
                brazo2 = false;
                piernas = true;
                break;
            case PlayerManager.TimmyStates.S2: // piernas
                brazo1 = false;
                brazo2 = false;
                piernas = true;
                break;
            case PlayerManager.TimmyStates.S3: // dos brazos
                brazo1 = true;
                brazo2 = true;
                piernas = false;
                break;
            case PlayerManager.TimmyStates.S4: // un brazo
                brazo1 = true;
                brazo2 = false;
                piernas = false;
                break;
            case PlayerManager.TimmyStates.S5: // nada
                brazo1 = false;
                brazo2 = false;
                piernas = false;
                break;
        }

        _images[_posBrazo1].sprite  = _sprites[_posBrazo1 * 2 + (brazo1 ? 1 : 0)];
        _images[_posBrazo2].sprite  = _sprites[_posBrazo2 * 2 + (brazo2 ? 1 : 0)];
        _images[_posPiernas].sprite = _sprites[_posPiernas * 2 + (piernas ? 1 : 0)];
    }

    // OBJETOS
    public void SetObject(PlayerManager.Objetos objeto) // no necesita de un metodo reset porque .NADA es 3
    {
        _images[_posCostillas].sprite = _sprites[_posCostillas * 2 + (int)objeto];
    }

    // ALUBIAT
    public bool TieneAlubiat() // si el sprite de alubiat está actualizado
    {
        if (_images[_posAlubiat].sprite == _sprites[(_posAlubiat + 1) * 2] || _images[_posAlubiat].sprite == _sprites[(_posAlubiat + 1) * 2 + 1])
        {
            return true;
        } else return false;
    }

    public void SetAlubiat(bool activo) // asigna el sprite de alubiat según esté a activo o no
    {
        _images[_posAlubiat].sprite = _sprites[(_posAlubiat + 1) * 2 + (activo ? 1 : 0)];
    }

    public void ResetAlubiat() // lo resetea a vacío
    {
        _images[_posAlubiat].sprite = _sprites[(_posAlubiat + 1) * 2 - 1]; // el sprite de vacío está justo uno antes que los de alubiat
    }

    // BUCLE
    void Start()
    {
        _menus = new GameObject[4]; // creación del array de menús y asignación
        //_menus[0] = _StartMenu;
        _menus[1] = _HUD; // habrá que poner más segun añadamos menuses
        _activeMenu = GameManager.Instance.CurrentState; // asocia el menú actual con el estado actual

        _posCabeza      = 0; // posiciones concretas de cada parte en el array de imágenes
        _posBrazo1      = 1;
        _posBrazo2      = 2;
        _posPiernas     = 3;
        _posCostillas   = 4;
        _posAlubiat     = 5;

        GameManager.Instance.RegisterUIManager(this);
        PlayerManager.Instance.RegisterUIManager(this);
    }

    void Update()
    {
        // aquí no debería haber nada pero lo dejo porsiaca
    }
}
