using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region References
    // referncia al script de interacción
    private InputController _inputController;
    private Interaction _interaction;
    private Dialogue _dialogue;
    private DialogueTrigger _dialogueTrigger;
    [SerializeField] private TMP_Text _dialogueText;
    private Transform _transform;
    private SpriteRenderer _mySpriteRenderer;
    [SerializeField] private GameObject _player;

    // Feedback de interacción
    [SerializeField] private TMP_Text _interactText;
    #endregion

    #region Parameters
    private Queue<string> _guion; // colección de strings, array circular FIFO (first in first out)
    // mover a Timoteo
    [SerializeField]
    private GameObject WaypointDialogo;
    [SerializeField]
    private float _speed; // velocidad a la que se mueve Timoteo al waypoint de diálogo

    #endregion

    #region Properties
    public bool _validNPCHitbox; // está en el área de un NPC
    // en conversación 
    private bool _enConversacion = false;
    public bool Conversacion { get { return _enConversacion; } }
    #endregion

    #region Methods
    private void Activar()
    {
        /*if (_inputController.Conversar && _validNPCHitbox)
        {
            EnConversacion(true);
            _mySpriteRenderer.color = Color.blue;
        }*/
        Debug.Log("ACTIMEL");
        MoveTimoteo();
        Dialogo();
    }
    #region flujo de diálogo
    private void OnTriggerEnter2D(Collider2D collision) // mostrar texto de interacción
    {
        if (collision.gameObject == _player) //filtro para que solo el jugador pueda interactuar con cosas
        {
            _interactText.text = "Pulsa 'M' para conversar";
        }
    }
    private void OnTriggerExit2D(Collider2D collision) // mostrar texto de interacción
    {
        if (collision.gameObject == _player) //filtro para que solo el jugador pueda interactuar con cosas
        {
            _interactText.text = "";
        }
    }
    private void Dialogo()
    {
        _inputController.enabled = false;
    }
    public void EnConversacion(bool conversando)
    {
        _enConversacion = conversando; // si se está en conversación
    }

    // INICIO DIÁLOGO
    public void StartDialogue(Dialogue _dialogue)
    {
        Debug.Log("Conversación con" + _dialogue._name);

        _guion.Clear(); // limpiar guión
        foreach (string _frase in _dialogue._personaje) // recorrer array de guión de personaje
        {
            _guion.Enqueue(_frase);                     // añadir a la queue
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

    // ANIMACIÓN DE CARACTERES
    IEnumerator Letritas(string _frase) // para que se vaya escribiendo la frase letra por letra
    {
        _dialogueText.text = "";
        foreach (char _letra in _frase.ToCharArray()) // convierte de string a array de caracteres
        {
            _dialogueText.text += _letra;
            yield return null; // proporciona el siguiente valor en la iteración
        }
    }

    // FIN DIÁLOGO
    void FinDialogo()
    {
        Debug.Log("Conversación finiquitada");
    }

    #endregion

    #region mover timoteo
    void MoveTimoteo()
    {
        Debug.Log("Muevete");
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
        _guion = new Queue<string>(); // inicialización de _guion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
