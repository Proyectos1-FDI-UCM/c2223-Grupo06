using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class DialogueManager : MonoBehaviour
{
    #region References
    // referncia al script de interacci�n
    private InputController _inputController;
    private Interaction _interaction;
    private Dialogue _dialogue;
    private DialogueTrigger _dialogueTrigger;
    [SerializeField] private TMP_Text _dialogueText;
    private Transform _transform;
    private SpriteRenderer _mySpriteRenderer;
    #endregion

    #region Parameters
    private Queue<string> _guion; // colecci�n de strings, array circular FIFO (first in first out)
    // mover a Timoteo
    [SerializeField]
    private GameObject WaypointDialogo;
    [SerializeField]
    private float _speed; // velocidad a la que se mueve Timoteo al waypoint de di�logo

    #endregion

    #region Properties
    public bool _validNPCHitbox; // est� en el �rea de un NPC
    // en conversaci�n 
    private bool _enConversacion = false;
    public bool Conversacion { get { return _enConversacion; } }
    #endregion

    #region Methods
    public void Activar()
    {
        MoveTimoteo();
        Dialogo();
    }
    #region flujo de di�logo


    private void Dialogo()
    {
        _inputController.enabled = false;


    }
    public void EnConversacion(bool conversando) 
    {
        _enConversacion = conversando; // si se est� en conversaci�n
    }

    // INICIO DI�LOGO
    public void StartDialogue(Dialogue _dialogue)
    {
        Debug.Log("Conversaci�n con" + _dialogue._name);

        _guion.Clear(); // limpiar gui�n
        foreach (string _frase in _dialogue._personaje) // recorrer array de gui�n de personaje
        {
            _guion.Enqueue(_frase);                     // a�adir a la queue
        }
        SiguienteFrase(); // al acabar pasar a la siguiente frase
    }

    // SIGUIENTE FRASE
    public void SiguienteFrase()
    {
        if (_guion.Count == 0) // comprobar que queden frases en la queue
        {
            FinDialogo();
            return;
        }
        string _frase = _guion.Dequeue(); // siguiente frase en la queue 
        StopAllCoroutines(); // parar antes de empezar la nueva frase
        StartCoroutine(Letritas(_frase));
    }

    // ANIMACI�N MAOMENO
    IEnumerator Letritas (string _frase) // para que se vaya escribiendo la frase letra por letra
    {
        _dialogueText.text = "";
        foreach (char _letra in _frase.ToCharArray()) // convierte de string a array de caracteres
        {
            _dialogueText.text += _letra;
            yield return null; // proporciona el siguiente valor en la iteraci�n
        }
    }

    // FIN DI�LOGO
    void FinDialogo() 
    {
        Debug.Log("Conversaci�n finiquitada");
    }

    #endregion

    #region mover timoteo
    void MoveTimoteo()
    {
        // Hace que Timoteo se mueva hacia el waypoint correspondiente con la velocidad marcada
        _transform.position = Vector3.MoveTowards(_transform.position, // pos inicial 
            WaypointDialogo.transform.position, _speed * Time.deltaTime); // pos final
    }
    #endregion


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _inputController = PlayerAccess.Instance.InputController;
        _transform = transform;
        _dialogueTrigger = GetComponent<DialogueTrigger>();
        _interaction = GetComponent<Interaction>();
        _inputController = GetComponent<InputController>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _guion = new Queue<string>(); // inicializaci�n de _guion
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputController.Conversar && _validNPCHitbox)
        {
            EnConversacion(true);
            _mySpriteRenderer.color = Color.blue;
        }

        Activar();
    }
}
