using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region References
    // referncia al script de interacci�n
    private InputController _inputController;
    private Interaction _interaction;
    private Dialogue _dialogue;
    private DialogueTrigger _dialogueTrigger;
    public Text _dialogueText;
    #endregion
    #region Parameters
    private Queue<string> _guion; // colecci�n de strings, array circular FIFO (first in first out)
    #endregion
    #region Properties
    public bool _validNPCHitbox; // est� en el �rea de un NPC
    // en conversaci�n 
    private bool _enConversacion = false;
    public bool Conversacion { get { return _enConversacion; } }
    #endregion
    #region Methods
    public void Activar()
    {
        
    }
    public void EnConversacion(bool conversando)
    {
        _enConversacion = conversando;
        Debug.Log("En conversaci�n");
    }
    public void StartDialogue(Dialogue _dialogue)
    {
        Debug.Log("Conversaci�n con" + _dialogue._name);

        _guion.Clear(); // limpiar gui�n
        foreach (string _frase in _dialogue._personaje) // recorrer array de gui�n de personaje
        {
            _guion.Enqueue(_frase);                     // a�adir a la queue
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
        Debug.Log("Conversaci�n finiquitada");
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _inputController = PlayerAccess.Instance.InputController;
        _dialogueTrigger = GetComponent<DialogueTrigger>();
        _interaction = GetComponent<Interaction>();
        _guion = new Queue<string>(); // inicializaci�n de _guion
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputController.Conversar && _validNPCHitbox)
        {
            EnConversacion(true);
        }
    }
}
