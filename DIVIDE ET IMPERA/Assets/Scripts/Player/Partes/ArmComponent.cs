using UnityEngine;

public class ArmComponent : MonoBehaviour
{
    #region References

    #endregion
    #region Parameters

    #endregion
    #region Properties
    private Collider2D[] _colliders;
    private bool _leverFound;
    private GameObject _touchedLever;
    private PlayerManager.TimmyStates _currentState;
    #endregion
    #region Methods

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _currentState = PlayerManager.State;
    }

    // Update is called once per frame
    void Update()
    {
        _colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        int i = 0;
        _leverFound = false;

        while(i < _colliders.Length && !_leverFound)
        {
            if (_colliders[i].gameObject.GetComponent<PalancaComponent>() != null)
            {
                _leverFound = true;
                _touchedLever = _colliders[i].gameObject;
            }
            else
                i++;
        }
        if(_leverFound && !_touchedLever.GetComponent<PalancaComponent>().BrazoConectado)
        {
            _leverFound = false;
            _touchedLever.GetComponent<PalancaComponent>().ConectarBrazo(true);
            _touchedLever = null;
            Destroy(gameObject);
        }
    }
}
