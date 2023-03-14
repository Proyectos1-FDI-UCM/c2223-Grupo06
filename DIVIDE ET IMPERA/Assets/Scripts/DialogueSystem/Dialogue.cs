using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // clase entera serializable
public class Dialogue 
{
    #region References

    #endregion
    #region Parameters
    // public enum Personajes { Timmy, Bob, Alubia, Narrador }; // 0 -> 3

    public string _name;

    [TextArea(3, 10)]
    public string[] _guion; // frases que se irán cargando en la cola de queue
    #endregion
}
