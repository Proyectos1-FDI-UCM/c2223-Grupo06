using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewPlatformMovement : MonoBehaviour
{
    #region References
    [SerializeField]
    private Transform[] _waypoints;
    #endregion

    #region Parameters
    [SerializeField]
    private float _speed;
    [SerializeField]
    private bool _isCiclic;
    #endregion

    #region Properties
    [SerializeField]
    private bool _active;
    private int _currentWaypoint;
    #endregion


    #region Methods
    public void OnOff(bool onoff)
    {
        _active = onoff;
    }

    public bool isActive()
    {
        return _active;
    }

    private void Move() 
    {
        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypoint].position, _speed * Time.deltaTime); //mueve plataforma
    }
    
    private void CiclicMovement()
    {
        if (_active)
        {
            Move();
            if (transform.position == _waypoints[_currentWaypoint].position) //has llegado al waypoint actual
            {
                if (_currentWaypoint == _waypoints.Length - 1) //es el ultimo asi que reinicias
                {
                    _currentWaypoint = 0;
                }
                else
                {
                    _currentWaypoint++; //no es el ultimo asi que vas al siguiente
                }
            }
        }
        else
        {
            Move();
            if (transform.position == _waypoints[_currentWaypoint].position && _currentWaypoint != 0) //si no esta activada se mueve hacia atras hasta el principio
            {
                _currentWaypoint--;
            }
        }
    }

    private void NotCiclicMovement()
    {
        if (_active)
        {
            Move();
            if (transform.position == _waypoints[_currentWaypoint].position && _currentWaypoint != _waypoints.Length - 1) //si llegas al waypoint actual y no es el ultimo incrementa
            {
                _currentWaypoint++;
            }
        }
        else //si no esta activada
        {
            Move();
            if (transform.position == _waypoints[_currentWaypoint].position && _currentWaypoint != 0) //llegas al waypoint  y no has vuelto al principio aun decrementa
            {
                _currentWaypoint--;
            }
        }
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _active = false;
        _currentWaypoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isCiclic)
        {
            CiclicMovement();
        }
        else
        {
            NotCiclicMovement();
        }
    }
}
