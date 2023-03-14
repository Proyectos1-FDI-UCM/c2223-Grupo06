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

    [TextArea(3, 10)]
    public string[] _parrafos; // parrafos que se ir�n cargando en la cola de queue
    #endregion
}
