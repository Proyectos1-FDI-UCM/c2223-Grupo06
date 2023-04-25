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
    [SerializeField] private TMP_Text _dialogueText; // Texto de dialogo
    [SerializeField] private TMP_Text _interactText; // Texto de feedback para interaccion
    #endregion

    #region Parameters
    // flujo
    [Tooltip("Líneas de diálogo")]
    public string[] _lines;                     // lineas del guion
    [Tooltip("Cadencia a la que se escribe el texto")]
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
            if (PlayerAccess.Instance != null)
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

    // ANIMACION DE CARACTERES
    IEnumerator WriteLine() // corrutina para que se vayan excribiendo las lineas
    {
        _dialogueText.text = "";
        foreach (char _letter in _lines[_index].ToCharArray()) // index aumenta segun se pasa de linea
        {
            _dialogueText.text += _letter;

            /*if (_letter == 'T')
            {
                _dialogueText.color = Color.blue;
            }
            else if (_letter == 'B')
            {
                _dialogueText.color = Color.red;
            }
            else
            { _dialogueText.color = Color.white; }*/
            // yield return null;
            yield return new WaitForSeconds(_speedText); // proporciona el siguiente valor en la iteración

            // sfx
            if (SFXComponent.Instance != null)
                SFXComponent.Instance.SFXDialogue(0);
        }
    }

    /*protected void SpeakersText() // lista para modificaciones texto de personajes
    {
        foreach () 
        {
            var _speaker;
            switch (_speaker)
            {
                case Speaker.Timoteo:

            }
        }
    }*/

    // Parametros:
    // _lines -> el texto que se esta escribiendo
    // _letter -> el caracter que se esta escribiendo
    // _index -> indice del caracter dentro de _lines
    protected void CheckTag(string _lines, char _letter, int _index, ref bool _inTag) // tags para saber quién está hablando - a ver si funciona
    {
        if (_letter == '<')
        {
            // Si el caracter es '<' significa que hemos entrado en un tag -> activamos bandera
            _inTag = true;

            char _next = _lines[_index + 1]; // next -> siguiente caracter

            if (_next != '/')
            {
                // entrar a tag
                switch (_next)
                {                                                         // Personaje: <entrada> y <salida>
                    case 't': _dialogueText.color = Color.blue; break;    // Timoteo: <t> y </t>
                    case 'a': _dialogueText.color = Color.red; break;     // Alubia: <a> y </a>
                    case 'b': _dialogueText.color = Color.yellow; break;  // Bob: <b> y </b>
                    case 'n': _dialogueText.color = Color.white; break;   // Narrador: <n> y </n>
                }
            }
            else
            {
                // salir de tag
                _dialogueText.color = Color.green;
            }
        }
        else if ((_index > 0) && (_lines[_index - 1] == '>'))
        {
            // Anterior caracter era '>' significa que hemos salido de un tag
            _inTag = false;
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
        if (_dialogueText.text == _lines[_index]) // siguiente linea
        {
            NextLine();
        }
        else // fin dialogo
        {
            StopAllCoroutines();
            // _dialogueText.text = _lines[_index]; // no hace falta ,':·/
            _inputController.enabled = true;
            _inputControllerDialogue.enabled = false;
            _dialogueText.text = "";
            _inputControllerDialogue._enConversacion = false;
            Debug.Log("acabose");
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
