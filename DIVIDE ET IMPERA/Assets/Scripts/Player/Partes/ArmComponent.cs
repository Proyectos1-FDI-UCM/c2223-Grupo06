using UnityEngine;
using UnityEngine.Tilemaps;

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
    #endregion

    #region Methods

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Tilemap>())
        {
            // sfx
            if (SFXComponent.Instance != null)
            {
                SFXComponent.Instance.SFXObjects(2);
                SFXComponent.Instance.SFXPlayer(10);
            }
                
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        _colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        int i = 0;
        _leverFound = false;

        while (i < _colliders.Length && !_leverFound)
        {
            if (_colliders[i].gameObject.GetComponent<PalancaComponent>() != null)
            {
                _leverFound = true;
                _touchedLever = _colliders[i].gameObject;
            }
            else
                i++;
        }
        if (_leverFound && !_touchedLever.GetComponent<PalancaComponent>().BrazoConectado)
        {
            _leverFound = false;
            _touchedLever.GetComponent<PalancaComponent>().ConectarBrazo(true);
            _touchedLever = null;
            Destroy(gameObject);
        }
    }
}
