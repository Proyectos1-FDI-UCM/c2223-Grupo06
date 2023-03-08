using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region references
    private Animator _myAnimator;
    private GroundCheck _myGroundCheck;
    private MovementComponent _myMovementComponent;
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

        if (_myMovementComponent.Direccion != 0)
        {
            _myAnimator.SetTrigger("isRunning");
        }
        else
        {
            _myAnimator.ResetTrigger("isRunning");
        }
    }
}
