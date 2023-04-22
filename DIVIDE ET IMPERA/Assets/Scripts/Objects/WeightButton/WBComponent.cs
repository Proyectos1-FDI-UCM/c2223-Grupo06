using UnityEngine;

public class WBComponent : MonoBehaviour
{
    #region references
    [Tooltip("objetos que van a ser activados/desactivados")]
    [SerializeField] private GameObject _objeto;
    private MovingPlatformComponent _movingPlatform;

    [SerializeField]
    Transform pointA;
    [SerializeField]
    Transform pointB;

    [Tooltip("objeto padre de todos los objetos de la sala")]
    [SerializeField]
    private GameObject _objetos;
    #endregion

    #region Properties

    Collider2D[] colliders;

    #endregion
    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private bool _permanente;
    //private bool _move = false;
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

    // metodo para activar el objeto
    private void Activar()
    {
        _objeto.GetComponent<NewPlatformMovement>().OnOff();  // true

    }

    // metodo para desactivar el objeto
    private void Desactivar()
    {
        _objeto.GetComponent<NewPlatformMovement>().OnOff();  // false
    }

    // metodo que busca si hay algun otro boton que haya activado el mismo objeto 
    // basicamente sirve para que no se queden pillados entre ellos, asi todos
    // los botones que conecten a un mismo objeto pueden modificarlo
    private bool IsPriority()
    {
        bool aux = false;
        int i = 0;
        Transform[] child = _objetos.GetComponentsInChildren<Transform>();
        // mientras haya hijos en y no se haya encontrado uno con prioridad
        while (i < child.Length && !aux)
        {
            // si tiene wbcomponent, si controlan el mismo objeto y el objeto tiene prioridad (lo ha activado)
            if (child[i].GetComponent<WBComponent>()
                && child[i].GetComponent<WBComponent>()._objeto == this._objeto
                && child[i].GetComponent<WBComponent>().Priority)
            {
                aux = true;
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

        // si es permanente da igual lo que pase despues siempre se va a quedar activado
        if (_permanente)
        {
            if (i != colliders.Length)
            {
                _mySpriteRenderer.color = Color.white;
                Activar();
                //ActivarGeneral(true);
            }
        }
        // si no es permanente depende de si tiene algo encima o no
        else
        {
            if (i != colliders.Length)
            {
                _mySpriteRenderer.color = Color.white;
                // si no hay nada activado lo activa y le da la prioridad al boton correspondiente
                if (!_objeto.GetComponent<NewPlatformMovement>().isActive())
                {
                    Activar();
                    _priority = true;
                }

                //ActivarGeneral(true);
            }
            else
            {
                // si no hay ningun objeto con prioridad desactiva el objeto
                _mySpriteRenderer.color = Color.magenta;
                if (_objeto.GetComponent<NewPlatformMovement>().isActive()
                    && !IsPriority())
                {
                    Desactivar();
                }
                // si hay alguno con prioridad y este quiere tenerlo tambien se fasitidia y le quita la prioridad
                if (IsPriority() && _priority)
                {
                    _priority = false;
                }
            }
        }

    }
}
