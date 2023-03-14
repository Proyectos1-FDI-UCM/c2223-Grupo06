using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region References
    // referncia al script de interacci�n
    private Interaction _interaction;
    private Dialogue _dialogue;
    #endregion
    #region Parameters
    private Queue<string> _guion; // colecci�n de strings, array circular FIFO (first in first out)
    #endregion
    #region Properties

    #endregion
    #region Methods
    public void Activar()
    {
    
    }
    public void StartDialogue(Dialogue _dialogue)
    {
        Debug.Log("Conversaci�n con" + _dialogue._name);
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _guion = new Queue<string>(); // inicializaci�n de _guion
    }

    // Update is called once per frame
    void Update()
    {

    }
}
