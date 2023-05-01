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
    private MovementComponent _movementComponent;
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
    int _index = 0;                                // para saber en que linea estamos

    // mover a timoteo
    [Tooltip("Punto al que se mueve Timoteo al inicio de la conversación")]
    [SerializeField] private GameObject WaypointDialogo; // punto al que se mueve timoteo al inicio del dialogo
    [Tooltip("Velocidad a la que se mueve Timoteo al waypoint")]
    [SerializeField] private float _speed;               // velocidad a la que se mueve timoteo al waypoint de dialogo

    // escribir la linea 
    [Tooltip("Se ha acabado de escribir la linea")]
    [SerializeField]
    public bool _writingLine = false; // booleano para saber si se esta escribiendo la linea
    public bool WritingLine { get { return _writingLine; } }
    #endregion

    private bool _moveTimmy;
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
        if (_inputController.Direccion == 0) // si el timmy esta quieto
        {
            _inputControllerDialogue._enConversacion = true;
            _moveTimmy = true;
            StartDialogue();
        }
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
        _writingLine = true;
        _dialogueText.text = "";
        foreach (char _letter in _lines[_index].ToCharArray()) // index aumenta segun se pasa de linea
        {
            _dialogueText.text += _letter;

            yield return new WaitForSeconds(_speedText); // proporciona el siguiente valor en la iteración

            // sfx
            if (SFXComponent.Instance != null)
                SFXComponent.Instance.SFXDialogue(0);
        }

        _writingLine = false;
    }

    // DIFERENCIACION ENTRE PERSNAJES EN CONVERSACIÓN -> MEJORA NO IMPLEMENTADA
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
    }

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
    }*/

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
            //Debug.Log("desactivate buen hombre");
            _dialogueText.text = string.Empty;
            enabled = false; // desactivar el objeto -> FIN DIALOGO
        }
    }

    public void ProcessInput()
    {
        // checkea si esta en la ultima linea (ya escrita) y (si la linea actual es la corresponiente[caso en el que
        // no ha cancelado que se escriba la linea] o que se estuviera escribiendo la linea), por lo que si ha
        // acabado de escribir, estaba a medias y no esta en la ultma linea, escirbe la siguiente
        if (_lines.Length > _index && _writingLine) // siguiente linea
        {

            StopAllCoroutines(); // para todas las corrutinas para que no se dupliquen las lineas
            _dialogueText.text = _lines[_index]; // escribe la linea
            _writingLine = false; // deja de escribir la linea

        }
        else if (_lines.Length > _index + 1 && _dialogueText.text == _lines[_index]) // si ya ha acabado de escribir
        {
            StopAllCoroutines(); // para todas las corrutinas
            NextLine(); // pasa a la siguiente linea
        }
        else // fin dialogo
        {
            StopAllCoroutines();
            _writingLine = false;
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
        if (_playerTransform.position.x < WaypointDialogo.transform.position.x - 0.05 || _playerTransform.position.x > WaypointDialogo.transform.position.x + 0.05)
        {
            // quita el input del player
            _inputController.enabled = false;
            _inputControllerDialogue.enabled = true;

            // activa la animacion de timmy corriendo  
            _player.GetComponent<PlayerAnimationController>().IsMoving = true;
            // flippea al timmy
            FlipTimoteoBeforeSpeaking();
            // mueve a timmy al waypoint
            _playerTransform.position = Vector3.MoveTowards(_playerTransform.position,  // posición inicial 
                WaypointDialogo.transform.position, _speed * Time.deltaTime);           // posición final
        }
        else
        {
            // desactiva la animacion de timmy corriendo
            _player.GetComponent<PlayerAnimationController>().IsMoving = false;
            // cambia el sentido del player para que mire al npc
            FlipTimoteoBeforeSpeaking();
            //_player.transform.localScale = new Vector2(1f, 1f);
            _moveTimmy = false;
        }
    }

    /// <summary>
    /// Solo funciona si el waypoint esta a la izquierda del npc asi que cuidao
    /// </summary>
    private void FlipTimoteoBeforeSpeaking()
    {
        // si la diferencia entre el player y el waypoint es mayor que 0 significa que esta a la derecha
        if (0.1f < _player.transform.position.x - WaypointDialogo.transform.position.x)
        {
            _player.transform.localScale = new Vector2(-1f, 1f);
        }
        else
        // si la diferencia entre el player y el waypoint es menor que 0 significa que esta a la izquierda
        {
            _player.transform.localScale = new Vector2(1f, 1f);
        }
    }
    #endregion
    #endregion

    void Start()
    {
        // access player
        _inputController = PlayerAccess.Instance.InputController;
        _inputControllerDialogue = PlayerAccess.Instance.InputControllerDialogue;
        _playerTransform = PlayerAccess.Instance.Transform;
        _movementComponent = PlayerAccess.Instance.MovementComponent;
    }
    private void Update()
    {
        if (_moveTimmy)
            MoveTimoteo();
    }
}
