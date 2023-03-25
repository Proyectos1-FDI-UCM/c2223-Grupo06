using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    #region References
    public Dialogue _dialogue;
    private DialogueManager _dialogueManager;
    #endregion
    #region Parameters

    #endregion
    #region Properties

    #endregion
    #region Methods
    /* public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(_dialogue);
    } */
    # endregion
    void Start()
    {
        _dialogueManager = GetComponent<DialogueManager>();
    }
}
