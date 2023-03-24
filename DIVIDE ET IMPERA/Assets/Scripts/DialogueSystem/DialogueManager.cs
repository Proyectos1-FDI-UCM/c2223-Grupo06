using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region References
    // player
    private InputController _inputController;
    private InputControllerDialogue _inputControllerDialogue;
    private Transform _playerTransform;
    [SerializeField] private GameObject _player;

    // di�logo
    [SerializeField] private TMP_Text _dialogueText; // Texto de di�logo
    [SerializeField] private TMP_Text _interactText; // Texto de feedback para interacci�n
    private DialogueTrigger _dialogueTrigger;
    private Interaction _interaction;
    private Dialogue _dialogue;
    #endregion

    #region Parameters
    // flujo
    private Queue<string> _guion; // colecci�n de strings, array circular first in first out

    // mover a timoteo
    [SerializeField] private GameObject WaypointDialogo; // punto al que se mueve timoteo al inicio del di�logo
    [SerializeField] private float _speed; // velocidad a la que se mueve timoteo al waypoint de di�logo
    #endregion

    #region Properties
    // en conversaci�n 
    [SerializeField]
    private bool _enConversacion = false;
    public bool Conversacion { get { return _enConversacion; } }
    #endregion

    #region Methods
    #region flujo de di�logo
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject == _player)                  //filtro para que solo el jugador pueda interactuar con cosas
        {
            _interactText.text = "Pulsa 'M' para conversar";  // mostrar texto de interacci�n
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _player)                  //filtro para que solo el jugador pueda interactuar con cosas
        {
            _interactText.text = "";                          // quitar texto de interacci�n
        }
    }

    private void Activar()
    {
        Debug.Log("ACTIMEL");
        _enConversacion = true;
        _inputController._enConversacion = true;
        MoveTimoteo();
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

    // FIN DI�LOGO
    public void FinDialogo()
    {
        Debug.Log("Conversaci�n finiquitada");
        _inputController.enabled = true;
        _enConversacion = false;
        _inputController._enConversacion = false;
    }

    // ANIMACI�N DE CARACTERES
    IEnumerator Letritas(string _frase) // para que se vaya escribiendo la frase letra por letra
    {
        _dialogueText.text = "";
        foreach (char _letra in _frase.ToCharArray()) // convierte de string a array de caracteres
        {
            _dialogueText.text += _letra;
            yield return null; // proporciona el siguiente valor en la iteraci�n
        }
    }
    #endregion

    #region mover timoteo
    void MoveTimoteo()
    {
        Debug.Log("Muevete");
        _inputController.enabled = false;
        // Hace que Timoteo se mueva hacia el waypoint correspondiente con la velocidad marcada
        _playerTransform.position = Vector3.MoveTowards(_playerTransform.position,  // posici�n inicial 
            WaypointDialogo.transform.position, _speed * Time.deltaTime);           // posici�n final
    }
    #endregion
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // player
        _inputController = PlayerAccess.Instance.InputController;
        //_inputControllerDialogue = PlayerAccess.Instance.InputControllerDialogue;
        _playerTransform = PlayerAccess.Instance.Transform;
        _interaction = GetComponent<Interaction>();

        // di�logo
        _dialogueTrigger = GetComponent<DialogueTrigger>();
        _guion = new Queue<string>(); // inicializaci�n de _guion
    }

    // Update is called once per frame
    void Update()
    {

    }
}
