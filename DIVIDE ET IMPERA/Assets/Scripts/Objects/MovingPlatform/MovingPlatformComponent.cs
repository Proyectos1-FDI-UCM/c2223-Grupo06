using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformComponent : MonoBehaviour
{
    #region References
    private Transform _transform;
    #endregion

    [SerializeField]
    private GameObject[] waypoints = new GameObject[2];
    [SerializeField]
    private float _speed;
    public float PlatformSpeed { get { return _speed; } }
    private int target;



    #region Methods
    // mueve la propia plataforma 
    void MovePlatform()
    {
        // hace que la plataforma se mueva hacia el waypoint correspondiente con la velocidad marcada
        _transform.position = Vector3.MoveTowards(_transform.position,
            waypoints[target].transform.position, _speed * Time.deltaTime);
    }

    // marca hacia qué waypoint va
    void WhichWaypoint()
    {
        // si la plataforma esta en la posicion del waypoint correspondiente
        if (_transform.position == waypoints[target].transform.position)
        {
            // si esta en el último waypoint va al primero
            if (target == waypoints.Length - 1)
            {
                target = 0;
            }
            // si no está en el último waypoint va al siguiente
            else
            {
                target++;
            }
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }

    private void FixedUpdate()
    {
        WhichWaypoint();
    }
}
