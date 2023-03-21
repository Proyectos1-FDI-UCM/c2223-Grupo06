using UnityEngine;

[System.Serializable] // clase entera serializable
public class Dialogue
{
    #region References

    #endregion
    #region Parameters
    // las conversaciones consisten de una interacci�n entre: Narrador + Timoteo + Personaje (Bob o Alubia)

    // NARRADOR
    public string _nameNarrador;    // espacio de nombre Narrador
    [TextArea(3, 10)]
    public string[] _narrador;      // frases que se ir�n cargando en la cola de queue - gui�n de Narrador

    // TIMOTEO
    public string _nameTimoteo;     // espacio de nombre Timoteo
    [TextArea(3, 10)]
    public string[] _timoteo;       // frases que se ir�n cargando en la cola de queue - gui�n de Timoteo

    // PERSONAJE
    public string _name;            // espacio de nombre de Personaje (Bob o Alubia)
    [TextArea(3, 10)]
    public string[] _personaje;     // frases que se ir�n cargando en la cola de queue - gui�n de Personaje (Bob o Alubia)
    #endregion
}
