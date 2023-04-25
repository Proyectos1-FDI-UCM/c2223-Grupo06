using UnityEngine;

public class InputControllerDialogue : MonoBehaviour
{
    #region References
    private InputController _inputController;
    private DialogueManager _dialogueManager;
    #endregion

    #region Properties
    [Tooltip("Se está en conversación")]
    [SerializeField]
    public bool _enConversacion = false; // booleano para saber si se está en conversación 
    public bool Conversacion { get { return _enConversacion; } }

    [Tooltip("Se ha acabado de escribir la linea")]
    [SerializeField]
    public bool _writingLine = false; // booleano para saber si se está en conversación 
    public bool WritingLine { get { return _writingLine; } }
    #endregion

    #region Methods
    private void DialogueInput()
    {
        if (_enConversacion && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))) // Space o UpArrow para avanzar en el diálogo
        {
            Debug.Log("me estoy muriendo epro de verdad");
            _dialogueManager.ProcessInput();
        }
    }

    public void RegisterDialogueManager(DialogueManager dialogue)
    {
        _dialogueManager = dialogue;
    }
    #endregion

    void Start()
    {
        this.enabled = false; // empieza desactivado
        _inputController = GetComponent<InputController>();
    }

    private void Update()
    {
        DialogueInput();
    }
}
