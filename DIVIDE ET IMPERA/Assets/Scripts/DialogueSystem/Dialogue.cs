using UnityEngine;

[System.Serializable] // clase entera serializable
public class Dialogue
{
    #region References

    #endregion
    #region Parameters
    // las conversaciones consisten de una interacción entre: Narrador + Timoteo + Personaje (Bob o Alubia)

    // NARRADOR
    public string _nameNarrador;    // espacio de nombre Narrador
    [TextArea(3, 10)]
    public string[] _narrador;      // frases que se irán cargando en la cola de queue - guión de Narrador

    // TIMOTEO
    public string _nameTimoteo;     // espacio de nombre Timoteo
    [TextArea(3, 10)]
    public string[] _timoteo;       // frases que se irán cargando en la cola de queue - guión de Timoteo

    // PERSONAJE
    public string _name;            // espacio de nombre de Personaje (Bob o Alubia)
    [TextArea(3, 10)]
    public string[] _personaje;     // frases que se irán cargando en la cola de queue - guión de Personaje (Bob o Alubia)
    #endregion
}
