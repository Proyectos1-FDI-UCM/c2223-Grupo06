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
    #endregion

    #region Methods
    private void DialogueInput()
    {
        if (_enConversacion && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))) // UpArrow o Z para avanzar en el diálogo
        {
            Debug.Log("que se bajen que me lo llevo");
            _dialogueManager.ProcessInput();
        }
    }

    public void RegisterDialogueManager(DialogueManager dialogue)
    {
        _dialogueManager= dialogue;
    }
    #endregion

    void Start()
    {
        this.enabled = false; // empieza desactivado
        _inputController = GetComponent<InputController>();
        //_dialogueManager = GetComponent<DialogueManager>();
    }

    private void Update()
    {
        DialogueInput();
    }
}
