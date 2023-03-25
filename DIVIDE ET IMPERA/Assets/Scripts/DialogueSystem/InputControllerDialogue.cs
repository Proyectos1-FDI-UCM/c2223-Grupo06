using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControllerDialogue : MonoBehaviour
{
    #region References
    private InputController _inputController;
    private DialogueManager _dialogueManager;
    #endregion

    #region Properties
    //------------DIÁLOGO----------------------------
    // booleano para saber si se está en conversación 
    [SerializeField]
    public bool _enConversacion = false;
    public bool Conversacion { get { return _enConversacion; } }
    #endregion

    #region Methods
    public void DialogueInput()
    {
        // if (_enConversacion) { Debug.Log("bezoya"); }
        if (_enConversacion && (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.UpArrow))) // UpArrow o Z para avanzar en el diálogo
        {
            Debug.Log("que se bajen que me lo llevo");
            // _dialogueManager.FinDialogo(); // debug para probar el input nuevo
            _inputController.enabled = true;
            enabled = false;
            _enConversacion = false;
            _inputController._enConversacion = false;
            _dialogueManager._enConversacion = false;
            // _dialogueManager.SiguienteFrase();
        }
    }
    #endregion

    void Start()
    {
        this.enabled = false;
        _inputController = GetComponent<InputController>();
        _dialogueManager = GetComponent<DialogueManager>();
    }

    private void Update()
    {
        DialogueInput();
    }
}
