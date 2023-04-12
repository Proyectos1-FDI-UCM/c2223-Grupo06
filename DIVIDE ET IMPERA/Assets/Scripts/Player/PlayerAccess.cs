using UnityEngine;

public class PlayerAccess : MonoBehaviour
{
    private static PlayerAccess _instance;                                       // VARIABLES PRIVADAS: 
    private Transform _transform;                                                   // todo son referencias a los componentes del player
    private Rigidbody _rigidbody;
    private Animator _animator;
    private InputController _inputController;
    private InputControllerDialogue _inputControllerDialogue;
    private MovementComponent _movementComponent;
    private JumpComponent _jumpComponent;
    private CollisionManager _collisionManager;
    private PlayerManager _playerManager;
    private BoneStateBar _boneBar;
    private FallDamage _fallDamage;
    // ... más según vayan existiendo más componentes en player

    public static PlayerAccess Instance { get { return _instance; } }            // MÉTODOS GETTERS PÚBLICOS: devuelven las variables privadas
    public Transform Transform { get { return _transform; } }
    public Rigidbody Rigidbody { get { return _rigidbody; } }
    public Animator Animator { get { return _animator; } }
    public InputController InputController { get { return _inputController; } }
    public InputControllerDialogue InputControllerDialogue { get { return _inputControllerDialogue; } }
    public MovementComponent MovementComponent { get { return _movementComponent; } }
    public JumpComponent JumpComponent { get { return _jumpComponent; } }
    public CollisionManager CollisionManager { get { return _collisionManager; } }
    public PlayerManager PlayerManager { get { return _playerManager; } }
    public BoneStateBar BoneBar { get { return _boneBar; } }
    public FallDamage FallDamage { get { return _fallDamage; } }
    // ... más según vayan existiendo más componentes en player

    private void Awake()                                                        // INICIALIZACIÓN DE LAS VARIABLES PRIVADAS
    {
        _instance = this;
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _inputController = GetComponent<InputController>();
        _inputControllerDialogue = GetComponent<InputControllerDialogue>();
        _movementComponent = GetComponent<MovementComponent>();
        _jumpComponent = GetComponent<JumpComponent>();
        _collisionManager = GetComponent<CollisionManager>();
        _playerManager = GetComponent<PlayerManager>();
        _boneBar = GetComponent<BoneStateBar>();
        _fallDamage = GetComponent<FallDamage>();
        // ... más según vayan existiendo más componentes en player

        // SE ACCEDEN A TRAVÉS DE PLAYERACCESS.INSTANCE.(lo que sea);
    }
}
