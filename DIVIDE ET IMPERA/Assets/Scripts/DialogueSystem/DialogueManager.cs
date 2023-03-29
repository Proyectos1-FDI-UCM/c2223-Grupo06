using System.Collections;
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
    [SerializeField] private TMP_Text _dialogueText;  // Texto de dialogo
    [SerializeField] private TMP_Text _interactText; // Texto de feedback para interaccion
    #endregion

    #region Parameters
    // flujo
    [Tooltip("Líneas de diálogo")]
    public string[] _lines;                    // lineas del guion
    [Tooltip("Velocidad a la que se escribe el texto")]
    [SerializeField] private float _speedText; // velocidad de texto
    int _index;                                // para saber en que linea estamos

    // mover a timoteo
    [Tooltip("Punto al que se mueve Timoteo al inicio de la conversación")]
    [SerializeField] private GameObject WaypointDialogo; // punto al que se mueve timoteo al inicio del dialogo
    [Tooltip("Velocidad a la que se mueve Timoteo al waypoint")]
    [SerializeField] private float _speed;               // velocidad a la que se mueve timoteo al waypoint de dialogo
    #endregion

    #region Methods
    #region flujo de diálogo
    #region interact text
    // TEXTO DE INTERACCION
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _player)            //filtro para que solo el jugador pueda interactuar con cosas
        {
            _interactText.text = "\u2191 para hablar";  // mostrar texto de interaccion
            PlayerAccess.Instance.InputControllerDialogue.RegisterDialogueManager(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _player)            // filtro para que solo el jugador pueda interactuar con cosas
        {
            _interactText.text = "";                    // quitar texto de interaccion
        }
    }
    #endregion

    // ACTIVAR DIALOGO
    private void Activar()
    {
        _inputControllerDialogue._enConversacion = true;
        MoveTimoteo();
        StartDialogue();
    }

    // INICIO DIALOGO
    public void StartDialogue() // cuando se llame se activa la corrutina
    {
        _index = 0;
        StopAllCoroutines();
        StartCoroutine(WriteLine());
    }

    // ANIMACIÓN DE CARACTERES
    IEnumerator WriteLine() // corrutina para que se vayan excribiendo las lineas
    {
        _dialogueText.text = "";
        foreach (char _letter in _lines[_index].ToCharArray()) // index aumenta segun se pasa de linea
        {
            _dialogueText.text += _letter;
            // yield return null;
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
            Debug.Log("desactivate buen hombre");
            _dialogueText.text = string.Empty;
            enabled = false; // desactivar el objeto -> FIN DIALOGO
        }
    }

    public void ProcessInput()
    {
        // Debug.Log("creen ustedes que esto funcione");

        if (_dialogueText.text == _lines[_index]) // siguiente linea
        {
            Debug.Log("siguiente linea con dos cojones");
            NextLine();
        }
        else // fin dialogo
        {
            StopAllCoroutines();
            // _dialogueText.text = _lines[_index]; // no hace falta ,':·/

            _dialogueText.text = "";
            _inputController.enabled = true;
            _inputControllerDialogue.enabled = false;
            _inputControllerDialogue._enConversacion = false;
        }
    }
    #endregion

    #region mover timoteo
    void MoveTimoteo()  // Hace que Timoteo se mueva hacia el waypoint correspondiente con la velocidad marcada
    {
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
    }
}
