using UnityEngine;

public class PalancaComponent : MonoBehaviour
{

    #region Referencias
    private InputController _inputController;
    private PlayerManager _playerManager;
    private MovingPlatformComponent _movingPlatform;
    private SpriteRenderer _mySpriteRenderer;
    #endregion

    #region
    // array de objetos
    [Tooltip("Array de objetos")]
    [SerializeField]
    private GameObject[] _objetos;
    private GameManager _gameManager;
    [Tooltip("Objeto que contiene todas las salas para que se tengan en cuenta todas las palancas")]
    [SerializeField]
    private GameObject _fatherGameObject;
    #endregion


    #region Properties
    // indica si la palanca hay que dejarla activada para que funcione o no (como con el boton por peso)
    [SerializeField]
    private bool _temporary;

    // palanca activada o no
    private bool _palanca;
    public bool Palanca { get { return _palanca; } }

    // brazo conectado a la palanca 
    private bool _brazoConectado = false;
    public bool BrazoConectado { get { return _brazoConectado; } set { _brazoConectado = value; } }

    // esta en el área de una palanca
    public bool _validPalancaHitbox; //no tengo ni idea de por que se llama asi,
                                     //he copiado el nombre que utilizaba antes
                                     //porque no se me ocurre otro xd

    private bool _activarObjeto;
    public bool ActivarObjeto { get { return _activarObjeto; } }

    [SerializeField]
    private int _brazoNum = 0;
    #endregion

    #region Métodos

    public void Activar()
    {
        _palanca = ActivarPalanca();
        ActivarObjetos();

        // sfx
        if (SFXComponent.Instance != null)
            SFXComponent.Instance.SFXObjects(0);
        CameraMovement.Instance.ChangeWhoToFollow(_objetos[0]);
    }

    // activa o desactiva la palanca dependiendo de su estado anterior
    public bool ActivarPalanca()
    {
        bool _lvr = !_palanca;
        _palanca = !_palanca;
        return _lvr;
    }

    // interactua con el objeto de fuera (puerta, plataforma etc)
    public void ActivarObjetos()
    {
        int i = 0;

        if (_palanca)
        {
            // para que se puedan activar mas de un objeto
            if (_objetos.Length > 0)
            {
                while (i < _objetos.Length)
                {
                    _objetos[i].GetComponent<NewPlatformMovement>().OnOff();
                    i++;
                }

            }
        }
        else if (!_palanca)
        {
            if (_objetos.Length > 0)
            {
                while (i < _objetos.Length)
                {
                    _objetos[i].GetComponent<NewPlatformMovement>().OnOff();
                    i++;
                }
            }
        }

    }

    //Conecta el brazo
    public void ConectarBrazo(bool conected)
    {
        _brazoConectado = conected;
        PlayerManager.Instance.ConnectedToLever(gameObject);
    }

    public void DesconectarBrazo()
    {
        ConectarBrazo(false);
        PlayerManager.Instance.Brazos++;
        PlayerManager.Instance.ConnectedToLever(null);
    }

    // busca los brazos/palancas ocupadas, devuelve el numero de palancas ocupadas
    private int FindArmInLever()
    {
        int i = 0, j = 0;
        Transform[] fathers = _fatherGameObject.transform.GetComponentsInChildren<Transform>();

        // busca si hay alguna palanca con brazo o no y devuelve el valor de brazos ocupados
        while (i < fathers.Length && j < 2)
        {
            // si no ha encontrado ningun brazo ocupado, no es esta palanca y no tiene brazo la palanca 
            if (j == 0 && fathers[i].GetComponent<PalancaComponent>()
                && fathers[i].GetComponent<PalancaComponent>() != this
                && fathers[i].GetComponent<PalancaComponent>().BrazoNum() > 0)
            {
                j++;
            }
            // lo mismo pero si ha encontrado la primera
            else if (j == 1 && fathers[i].GetComponent<PalancaComponent>()
                && fathers[i].GetComponent<PalancaComponent>() != this
                && fathers[i].GetComponent<PalancaComponent>().BrazoNum() > 1)
            {
                j++;
            }
            i++;
        }
        return j;
    }
    // acceso al indice de el brazo conectado a esta palanca, si es 0 no tiene brazo conectado
    public int BrazoNum()
    {
        return _brazoNum;
    }
    // settea el indice del brazo (esta por si aca)
    public void SetBrazoNum(int i)
    {
        _brazoNum = i;
    }

    // le da un indice a este brazo conectado dependiendo de los que haya conectados
    private void WhichArmNum()
    {
        int i = FindArmInLever();
        if (i == 0)
        {
            _brazoNum = 1;
        }
        else if (i == 1)
        {
            _brazoNum = 2;
        }
        else
        {
            _brazoNum = 0;
        }
    }


    #endregion


    private void Start()
    {
        _inputController = PlayerAccess.Instance.InputController;
        _playerManager = PlayerAccess.Instance.PlayerManager;
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _fatherGameObject = transform.parent.gameObject;
    }

    private void Update()
    { // Cyn: creo que no hace falta comprobar el estado en ninguna de las situaciones, pero lo he optimizado y los dejo igualmente
        // si se ha pulsado la E, el brazo está conectado y está en el estado correcto
        if (_inputController.Interactuar && _brazoConectado
            && (PlayerManager.Instance.Brazos < 2)
            && _brazoNum == _inputController.WhichArm)
        {
            Activar();
        }

        if (_temporary && !_brazoConectado && _palanca)
        {
            Activar();
        }


        // pone el indice del brazo de la palanca
        if (_brazoConectado && _brazoNum == 0)
        {
            WhichArmNum();
        }
        else if (!_brazoConectado)
        {
            _brazoNum = 0;
        }



        // aqui realmente nunca entra segun lo que he probado no se si lo sabiamos pero xd
        //-------CONECTAR BRAZO-------------------
        // se pulsa A o S y se esta cerca de la palanca
        if (_inputController.ConectarBrazo && _validPalancaHitbox
            && (PlayerManager.Instance.Brazos > 0))
        {
            // conecta el brazo
            ConectarBrazo(true);
            PlayerManager.Instance.Brazos--;
            PlayerManager.Instance.ConnectedToLever(gameObject);

        }
        // se pulsa A o S, está cerca de la palanca, está en los estados correctos y hay un brazo conectado
        else if (_inputController.RecuperarBrazo && _validPalancaHitbox
            && (PlayerManager.Instance.Brazos < 2)
            && _brazoConectado)
        {
            // desconecta el brazo
            DesconectarBrazo();
        }
    }

}
