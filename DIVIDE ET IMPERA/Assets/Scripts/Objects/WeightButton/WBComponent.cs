using UnityEngine;
using UnityEngine.UIElements;

public class WBComponent : MonoBehaviour
{
    #region references
    [SerializeField] private GameObject _objeto;
    private MovingPlatformComponent _movingPlatform;

    [SerializeField]
    Transform pointA;
    [SerializeField]
    Transform pointB;

    [SerializeField]
    private GameObject _objetos;
    #endregion

    #region Properties

    Collider2D[] colliders;

    #endregion
    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private bool _permanente;
    private bool _move = false;
    [SerializeField]
    private bool _priority;
    public bool Priority { get { return _priority; } }

    #region Methods
    /*
    private void ActivarGeneral(bool _act)
    {
        _move = true;
        Activar(_act);
    }
    private void Activar(bool _act)
    {
        if (_permanente && _move)
        {
            ActivarObjetosPerm();
        }
        else
        {
            ActivarObjetos(_act);
        }

    }

    // interactua con el objeto de fuera (puerta, plataforma etc)
    private void ActivarObjetosPerm()
    {
        if (!_movingPlatform.enabled)
        {
            _movingPlatform.enabled = true;
        }

    }

    private void ActivarObjetos(bool _act)
    {
        if (!_movingPlatform.enabled && _act)
        {
            _movingPlatform.enabled = true;
        }
        else if (!_act)
        {
            _movingPlatform.enabled = false;
        }
    }*/

    private void Activar()
    {
        _objeto.GetComponent<NewPlatformMovement>().OnOff(true);
        
    }

    private void Desactivar()
    {
        _objeto.GetComponent<NewPlatformMovement>().OnOff(false);
    }

    private bool ActivatingPriority(bool _aux)
    {
        _priority = _aux;
        return _priority;
    }

    private bool IsPriority()
    {
        bool aux = false;
        int i = 0;
        Transform []child = _objetos.GetComponentsInChildren<Transform>();
        // mientras haya hijos en y no se haya encontrado uno con prioridad
        while (i < child.Length && !aux)
        {
            if (child[i].GetComponent<WBComponent>()
                && child[i].GetComponent<WBComponent>().Priority)
            {
                aux = true;
                //Debug.Log("holi");
            }
            i++;
        }

        return aux;
    }
    #endregion

    /*
      private void OnTriggerStay2D(Collider2D collision)
      {
          if (collision.gameObject.GetComponent<WeightComponent>() != null)
          {
              _mySpriteRenderer.color = Color.white;
              _puerta.SetActive(false);
          }
          else 
          {
              _mySpriteRenderer.color = Color.magenta;
              _puerta.SetActive(true);
          }
      }
      */


    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        _movingPlatform = _objeto.GetComponent<MovingPlatformComponent>();
    }



    // Update is called once per frame
    void Update()
    {

        colliders = Physics2D.OverlapAreaAll(pointA.position, pointB.position);

        int i = 0;
        while (i < colliders.Length && colliders[i].gameObject.GetComponent<WeightComponent>() == null) i++;

        if (_permanente)
        {
            if (i != colliders.Length)
            {
                _mySpriteRenderer.color = Color.white;
                Activar();
                //ActivarGeneral(true);
            }
        }
        else
        {
            if (i != colliders.Length)
            {
                _mySpriteRenderer.color = Color.white;
                if (!_objeto.GetComponent<NewPlatformMovement>().isActive())
                {
                    Activar();
                    _priority = true;
                    //Debug.Log("activar");
                }
                    
                //ActivarGeneral(true);
            }
            else
            {
                _mySpriteRenderer.color = Color.magenta;
                if(_objeto.GetComponent<NewPlatformMovement>().isActive()
                    && !IsPriority())
                {
                    Desactivar();
                    //Debug.Log("desactivar");
                }
                if (IsPriority() && _priority)
                {
                    _priority = false;
                } 
            }
        }
        
    }
}
