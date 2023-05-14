using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region references
    private Animator _myAnimator;
    private GroundCheck _myGroundCheck;
    private MovementComponent _myMovementComponent;

    private bool _isMoving;
    public bool IsMoving { get { return _isMoving; } set { _isMoving = value; } }
    #endregion 
    void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myGroundCheck = GetComponentInChildren<GroundCheck>();
        _myMovementComponent = GetComponent<MovementComponent>();
        _myAnimator.ResetTrigger("isGrounded");
        _myAnimator.ResetTrigger("isRunning");
    }

    void Update()
    {
        if (PlayerManager.Instance.Parte == PlayerManager.Partes.PIERNAS)
        {
            _myAnimator.Rebind();
        }

        if (_myAnimator == null)
        {
            _myAnimator = GetComponent<Animator>();
        }

        if (_myGroundCheck.IsGrounded)
        {
            _myAnimator.SetTrigger("isGrounded");
        }
        else
        {
            _myAnimator.ResetTrigger("isGrounded");
        }

        if ((_myMovementComponent.Direccion != 0
            && !(Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)))
            || _isMoving)
        {
            _myAnimator.SetTrigger("isRunning");
        }
        else
        {
            _myAnimator.ResetTrigger("isRunning");
        }
    }
}
