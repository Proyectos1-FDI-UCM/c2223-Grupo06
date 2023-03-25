using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    // dialogo
    [SerializeField] public TMP_Text _dialogueText; // Texto de dialogo
    [SerializeField] private TMP_Text _interactText; // Texto de feedback para interaccion
    private DialogueTrigger _dialogueTrigger;
    private Interaction _interaction;
    private Dialogue _dialogue;
    #endregion

    #region Parameters
    // flujo
    //private Queue<string> _guion; // coleccion de strings, array circular first in first out
    public string[] _lines; // lineas del guion
    [SerializeField] private float _speedText; // velocidad de texto
    public int _index; // para saber en que linea estamos

    // mover a timoteo
    [SerializeField] private GameObject WaypointDialogo; // punto al que se mueve timoteo al inicio del dialogo
    [SerializeField] private float _speed; // velocidad a la que se mueve timoteo al waypoint de dialogo
    #endregion

    #region Methods
    #region flujo de diálogo
    // TEXTO DE INTERACCION
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject == _player)            //filtro para que solo el jugador pueda interactuar con cosas
        {
            _interactText.text = "\u2191 para hablar";  // mostrar texto de interaccion
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _player)            // filtro para que solo el jugador pueda interactuar con cosas
        {
            _interactText.text = "";                    // quitar texto de interaccion
        }
    }

    // ACTIVAR DIALOGO
    private void Activar()
    {
        // Debug.Log("ACTIMEL");
        _inputControllerDialogue._enConversacion = true;
        MoveTimoteo();
        StartDialogue();
        // if (_inputControllerDialogue.enabled) { Debug.Log("porfi porfi"); }
    }

    #region DIALOGO POCHO
    // INICIO DIALOGO
    /* public void StartDialogue(Dialogue _dialogue)
    {
        Debug.Log("Conversación con" + _dialogue._name);

        _guion.Clear(); // limpiar guion
        foreach (string _frase in _dialogue._personaje) // recorrer array de guion de personaje
        {
            _guion.Enqueue(_frase);                     // añadir a la queue
        }
        SiguienteFrase(); // al acabar pasar a la siguiente frase
    } */

    // SIGUIENTE FRASE
    /* public void SiguienteFrase()
    {
        if (_guion.Count == 0) // comprobar que queden frases en la queue
        {
            FinDialogo();
            return;
        }
        string _frase = _guion.Dequeue(); // siguiente frase en la queue 
        StopAllCoroutines(); // parar antes de empezar la nueva frase
        StartCoroutine(Letritas(_frase));
    } */

    // FIN DIALOGO
    public void FinDialogo()
    {
        if ( true ) 
        {
            _inputController.enabled = true;
            _inputControllerDialogue.enabled = false;
            _inputControllerDialogue._enConversacion = false;
            Debug.Log("ACABOSE");
        }
    }
    #endregion

    #region DIALOGO NUEVO
    // INICIO DIALOGO
    public void StartDialogue() // cuando se llame se activa la corrutina
    {
        _index = 0;
        StartCoroutine(WriteLine());
    }

    // ANIMACIÓN DE CARACTERES
    IEnumerator WriteLine() // corrutina para que se vayan excribiendo las lineas
    {
        foreach (char _letter in _lines[_index].ToCharArray()) // index aumenta segun se pasa de linea
        {
            _dialogueText.text += _letter;
            yield return new WaitForSeconds(_speedText); // proporciona el siguiente valor en la iteración

        }
    }

    public void NextLine()
    {
        if (_index < _lines.Length - 1) // -1 porque el array empieza en 0, (2 elementos - 1 = index en el array)
        {
            _index++; // si faltan lineas por escribir pasamos el index a la siguiente linea
            _dialogueText.text = string.Empty; // vaciar dialogo
            StartCoroutine(WriteLine()); // escribe nueva linea
        }
        else
        {
            gameObject.SetActive(false); // desactivar el objeto -> FIN DIALOGO
        }
    }

    #endregion

    #endregion

    #region mover timoteo
    void MoveTimoteo()  // Hace que Timoteo se mueva hacia el waypoint correspondiente con la velocidad marcada
    {
        // Debug.Log("Muevete");
        _inputController.enabled = false;
        _inputControllerDialogue.enabled = true;
        _playerTransform.position = Vector3.MoveTowards(_playerTransform.position,  // posición inicial 
            WaypointDialogo.transform.position, _speed * Time.deltaTime);           // posición final
    }
    #endregion
    #endregion

    void Start()
    {
        // player
        _inputController = PlayerAccess.Instance.InputController;
        _inputControllerDialogue = PlayerAccess.Instance.InputControllerDialogue;
        _playerTransform = PlayerAccess.Instance.Transform;
        _interaction = GetComponent<Interaction>();

        // diálogo
        _dialogueTrigger = GetComponent<DialogueTrigger>();
        // _guion = new Queue<string>(); // inicialización de _guion
    }
}
