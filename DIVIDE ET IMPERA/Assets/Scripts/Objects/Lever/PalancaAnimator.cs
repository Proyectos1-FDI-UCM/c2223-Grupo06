using UnityEngine;
using static PlayerManager;

public class PalancaAnimator : MonoBehaviour
{
    private PalancaComponent _palancaComponent;
    private Animator _myAnimator;
    private SpriteRenderer _mySpriteRenderer;

    [SerializeField]
    private Sprite[] _sprites;

    private bool _brazo = false;
    private bool _activada = false;
    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _palancaComponent = GetComponent<PalancaComponent>();
        _myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Parámetros del animator
        if (_palancaComponent.BrazoConectado)
        {
            _myAnimator.SetTrigger("tieneBrazo");
            _brazo = true;
        }
        else
        {
            _myAnimator.ResetTrigger("tieneBrazo");
            _brazo = false;
        }

        if (_palancaComponent.Palanca)
        {
            _myAnimator.SetTrigger("estaActivada");
            _activada = true;
        }
        else
        {
            _myAnimator.ResetTrigger("estaActivada");
            _activada = false;
        }

        // Lógica de sprites
        if (_brazo && _activada) 
        { 
            _mySpriteRenderer.sprite = _sprites[3];
        }
        else if (_brazo && !_activada)
        {
            _mySpriteRenderer.sprite = _sprites[2];
        }
        else if (!_brazo && _activada)
        {
            _mySpriteRenderer.sprite = _sprites[1];
        }
        else if (!_brazo && !_activada)
        {
            _mySpriteRenderer.sprite = _sprites[0];
        }

        // Todo esto es para el HUD
        if (_myAnimator.GetCurrentAnimatorStateInfo(0).length > _myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        { // si NO ha acabado la animación que está reproduciendo (sea la que sea)
            if (PlayerManager.Instance.Brazos == 1 &&
                PlayerManager.Instance.Parte != PlayerManager.Partes.BRAZO2)
            // si todavía tiene un brazo y no se ha cambiado el control
            {
                PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.BRAZO2);
                //Debug.Log("PARTE: BRAZO 2");
                // siempre, cuando quitas el primer brazo, se desactiva el brazo2 de la hud. por tanto, para que se vea aquí el cambio, se lo asigna a ese
            }
            else if (PlayerManager.Instance.Brazos == 0 &&
                PlayerManager.Instance.Parte != PlayerManager.Partes.BRAZO1)
            // si no tiene brazos y no se ha cambiado el control
            {
                PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.BRAZO1);
                //Debug.Log("PARTE: BRAZO 1");
                // para simplificar
            }
            // si tiene los dos brazos, da igual
        }
        else if (PlayerManager.Instance.Parte == PlayerManager.Partes.BRAZO1 || PlayerManager.Instance.Parte == PlayerManager.Partes.BRAZO2) // si todavía no ha vuelto del brazo *importante*
        { // si SÍ la ha acabado y el control viene de un brazo
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.CABEZA); // solo puede volver a la cabeza (si estás controlando las piernas, no puedes activar brazos)
            //Debug.Log("PARTE: CABEZA");
        }
    }
}
