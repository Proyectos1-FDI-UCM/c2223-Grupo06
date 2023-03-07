using UnityEngine;

public class PalancaAnimator : MonoBehaviour
{
    private PalancaComponent _palancaComponent;
    private Animator _myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        _palancaComponent = GetComponent<PalancaComponent>();
        _myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_palancaComponent.BrazoConectado)
        {
            _myAnimator.SetTrigger("tieneBrazo");
        }
        else
        {
            _myAnimator.ResetTrigger("tieneBrazo");
        }

        if (_palancaComponent.Palanca)
        {
            _myAnimator.SetTrigger("estaActivada");
        }
        else
        {
            _myAnimator.ResetTrigger("estaActivada");
        }
    }
}
