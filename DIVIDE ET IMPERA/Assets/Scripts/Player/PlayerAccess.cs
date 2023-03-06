using UnityEngine;

public class PlayerAccess : MonoBehaviour
{
    private static PlayerAccess _instance;                                       // VARIABLES PRIVADAS: 
    private Transform _transform;                                                   // todo son referencias a los componentes del player
    private Rigidbody _rigidbody;
    private Animator _animator;
    private InputController _inputController;
    private MovementComponent _movementComponent;
    private JumpComponent _jumpComponent;
    private CollisionManager _collisionManager;
    private PlayerManager _playerManager;
    // ... m�s seg�n vayan existiendo m�s componentes en player

    public static PlayerAccess Instance { get { return _instance; } }            // M�TODOS GETTERS P�BLICOS: devuelven las variables privadas
    public Transform Transform { get { return _transform; } }
    public Rigidbody Rigidbody { get { return _rigidbody; } }
    public Animator Animator { get { return _animator; } }
    public InputController InputController { get { return _inputController; } }
    public MovementComponent MovementComponent { get { return _movementComponent; } }
    public JumpComponent JumpComponent { get { return _jumpComponent; } }
    public CollisionManager CollisionManager { get { return _collisionManager; } }
    public PlayerManager PlayerManager { get { return _playerManager; } }
    // ... m�s seg�n vayan existiendo m�s componentes en player

    private void Awake()                                                        // INICIALIZACI�N DE LAS VARIABLES PRIVADAS
    {
        _instance = this;
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _inputController = GetComponent<InputController>();
        _movementComponent = GetComponent<MovementComponent>();
        _jumpComponent = GetComponent<JumpComponent>();
        _collisionManager = GetComponent<CollisionManager>();
        _playerManager = GetComponent<PlayerManager>();
        // ... m�s seg�n vayan existiendo m�s componentes en player

        // SE ACCEDEN A TRAV�S DE PLAYERACCESS.INSTANCE.(lo que sea);
    }
}
