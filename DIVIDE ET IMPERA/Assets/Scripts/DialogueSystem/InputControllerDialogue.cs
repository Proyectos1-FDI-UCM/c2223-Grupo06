using UnityEngine;

public class InputControllerDialogue : MonoBehaviour
{
    #region References
    private InputController _inputController;
    private DialogueManager _dialogueManager;
    #endregion

    #region Properties
    [SerializeField]
    public bool _enConversacion = false; // booleano para saber si se está en conversación 
    public bool Conversacion { get { return _enConversacion; } }
    #endregion

    #region Methods
    private void DialogueInput()
    {
        if (_enConversacion && (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))) // UpArrow o Z para avanzar en el diálogo
        {
            Debug.Log("que se bajen que me lo llevo");
            _dialogueManager.ProcessInput();


            /* if (_dialogueManager._dialogueText.text == _dialogueManager._lines[_dialogueManager._index]) // siguiente linea
            {
                _dialogueManager.NextLine();
            }
            else // fin dialogo
            {
                StopAllCoroutines();
                _dialogueManager._dialogueText.text = _dialogueManager._lines[_dialogueManager._index];

                _inputController.enabled = true;
                enabled = false;
                _enConversacion = false;
            } */
        }
    }
    #endregion

    void Start()
    {
        this.enabled = false; // empieza desactivado
        _inputController = GetComponent<InputController>();
        _dialogueManager = GetComponent<DialogueManager>();
    }

    private void Update()
    {
        DialogueInput();
    }
}
