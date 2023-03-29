using UnityEngine;

public class InputControllerDialogue : MonoBehaviour
{
    #region References
    private InputController _inputController;
    private DialogueManager _dialogueManager;
    #endregion

    #region Properties
    [Tooltip("Se est� en conversaci�n")]
    [SerializeField]
    public bool _enConversacion = false; // booleano para saber si se est� en conversaci�n 
    public bool Conversacion { get { return _enConversacion; } }
    #endregion

    #region Methods
    private void DialogueInput()
    {
        if (_enConversacion && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))) // UpArrow o Z para avanzar en el di�logo
        {
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
