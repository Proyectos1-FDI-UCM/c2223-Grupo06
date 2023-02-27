using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    #region Parameters
    private bool _validHitbox = false;
    public bool ValidHitbox { get { return _validHitbox; } }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _validHitbox = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _validHitbox = false;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
