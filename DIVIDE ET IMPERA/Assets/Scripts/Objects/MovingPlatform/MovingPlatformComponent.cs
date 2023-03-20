using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovingPlatformComponent : MonoBehaviour
{
    #region References
    private Transform _transform;
    #endregion

    #region Parameters
    [SerializeField]
    private GameObject[] waypoints = new GameObject[2];
    [SerializeField]
    private float _speed;
    [SerializeField]
    private bool _ciclica;
    private int target;
    private bool _outOfBounds = false;
    #endregion

    #region Methods
    // Mueve la propia plataforma 
    void MovePlatform()
    {
        // Hace que la plataforma se mueva hacia el waypoint correspondiente con la velocidad marcada
        _transform.position = Vector3.MoveTowards(_transform.position, waypoints[target].transform.position, _speed * Time.deltaTime);
    }

    // CICLAR
    void IsCiclica()
    {
        // Si esta en el último waypoint va al primero
        if (target == waypoints.Length - 1)
        {
            target = 0;
        }
        // Si no está en el último waypoint va al siguiente
        else
        {
            target++;
        }
    }

    void IsNotCiclica()
    {
        // si es menor que el tope sigue
        if (target < waypoints.Length-1)
        {
            target++;
        }
        else
        {
            // indica si el array se ha salido de tope
            _outOfBounds = true;
        }
    }

    // Marca hacia qué waypoint va
    void WhichWaypoint()
    {
        // si la plataforma esta en la posicion del waypoint correspondiente
        if (_transform.position == waypoints[target].transform.position)
        {
            // si se marca como cíclica
            if (_ciclica)
            {
                IsCiclica();
            }
            // si no se marca como cíclica
            else
            {
                IsNotCiclica();
            }
            
        }
    }
    #endregion

    void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        if (!_outOfBounds)
        {
            MovePlatform();
        }
        
    }

    private void FixedUpdate()
    {
        WhichWaypoint();
    }
}
