using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region references
    private Animator _myAnimator;
    private GroundCheck _myGroundCheck;
    private MovementComponent _myMovementComponent;
    private ThrowComponent _myThrowComponent;
    #endregion 
    void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myGroundCheck = GetComponentInChildren<GroundCheck>();
        _myMovementComponent = GetComponent<MovementComponent>();
        _myThrowComponent = GetComponent<ThrowComponent>();
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

        if (_myMovementComponent.Direccion != 0)
        {
            _myAnimator.SetTrigger("isRunning");
        }
        else
        {
            _myAnimator.ResetTrigger("isRunning");
        }
    }

    /*
    public void LanzameEsta()
    {
        _myAnimator.SetTrigger("isThrowing");
        Debug.Log("a mi si queme triggerea esto");
    }

    public void DeslanzameEsta()
    {
        _myAnimator.ResetTrigger("isThrowing");
        Debug.Log("DIOSSS joder ya HOSTIA");
    }

    private void LateUpdate()
    {
        if (_myAnimator.GetCurrentAnimatorStateInfo(0).length <= _myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime 
            && _myAnimator.GetBool("isThrowing") 
            && (_myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base.Throw") 
            || _myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Throw.IdleThrow")
            || _myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Throw.RunThrow")
            || _myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Throw.JumpThrow")))
        {
            DeslanzameEsta();
            _myThrowComponent.IsThrowing = false;
        }
    }
    */
}
