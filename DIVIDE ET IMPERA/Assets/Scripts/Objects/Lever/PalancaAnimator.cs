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

        if (_myAnimator.GetCurrentAnimatorStateInfo(0).length > _myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            if (PlayerManager.Instance.Brazos == 1) // si todavía tiene un brazo
            {
                PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.BRAZO2); // siempre, cuando quitas el primer brazo, se desactiva el brazo2 de la hud. por tanto, para que se vea aquí el cambio, se lo asigna a ese
            }
            else if (PlayerManager.Instance.Brazos == 0) // si no tiene brazos
            {
                PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.BRAZO1); // para simplificar
            }
            // si tiene los dos brazos, da igual
        }
        else
        {
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.CABEZA); // solo puede volver a la cabeza (si estás controlando las piernas, no puedes activar brazos)
        }
    }
}
