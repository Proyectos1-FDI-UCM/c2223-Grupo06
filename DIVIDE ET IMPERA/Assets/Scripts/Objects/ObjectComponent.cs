using UnityEngine;

public class ObjectComponent : MonoBehaviour
{
    #region references
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    #endregion


    #region properties
    [SerializeField]
    private bool _staticObject;
    public bool StaticObject { get { return _staticObject; } set { _staticObject = value; } }
    #endregion

    private void isStaticObject()
    {
        if (_staticObject)
        {
            _spriteRenderer.color = Color.black;

        }
        else
        {
            _spriteRenderer.color = Color.white;
        }
    }





    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isStaticObject();
    }
}
