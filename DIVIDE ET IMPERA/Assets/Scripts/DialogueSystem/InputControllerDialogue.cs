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
    private void DialogueInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z)) // UpArrow o Z para avanzar en el diálogo
        {
            // _dialogueManager.SiguienteFrase();
            _dialogueManager.FinDialogo(); // debug para probar el input nuevo
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
        if (_enConversacion) { Debug.Log("porfi porfi");  enabled = true; }
        DialogueInput();

    }
}
