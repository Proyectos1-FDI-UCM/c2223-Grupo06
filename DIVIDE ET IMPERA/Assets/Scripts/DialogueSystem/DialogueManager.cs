using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region References
    // referncia al script de interacción
    private Interaction _interaction;
    private Dialogue _dialogue;
    private DialogueTrigger _dialogueTrigger;
    public Text _dialogueText;
    #endregion
    #region Parameters
    private Queue<string> _guion; // colección de strings, array circular FIFO (first in first out)
    #endregion
    #region Properties

    #endregion
    #region Methods
    public void Activar()
    {
    
    }
    public void StartDialogue(Dialogue _dialogue)
    {
        Debug.Log("Conversación con" + _dialogue._name);

        _guion.Clear(); // limpiar guión
        foreach (string _frase in _dialogue._guion) // recorrer array de guión
        {
            _guion.Enqueue(_frase);                 // añadir a la queue
        }
        SiguienteFrase(); // al acabar pasar a la siguiente frase
    }

    public void SiguienteFrase()
    {
        if (_guion.Count == 0) // comprobar que queden frases en la queue
        {
            FinDialogo();
            return;
        }
        string _frase = _guion.Dequeue(); // siguiente frase en la queue 
        Debug.Log(_frase);
        _dialogueText.text = _frase;
    }

    void FinDialogo()
    {
        Debug.Log("Conversación finiquitada");
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _dialogueTrigger = GetComponent<DialogueTrigger>();
        _dialogue = GetComponent<Dialogue>();
        _interaction = GetComponent<Interaction>();
        _guion = new Queue<string>(); // inicialización de _guion
    }

    // Update is called once per frame
    void Update()
    {

    }
}
