using UnityEngine;

public class WeightPlatform : MonoBehaviour
{

    #region references
    [SerializeField]
    private Transform _myTransform;
    private float posY;

    [SerializeField]
    Transform ptA;
    [SerializeField]
    Transform ptB;
    #endregion

    #region Properties

    Collider2D[] colliders;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //_myTransform = transform;
        posY = _myTransform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        colliders = Physics2D.OverlapAreaAll(ptA.position, ptB.position);
        int i = 0;
        while (i < colliders.Length && colliders[i].gameObject.GetComponent<WeightComponent>() == null) i++;

        if (i != colliders.Length)
        {
            _myTransform.position = new Vector2(_myTransform.position.x, _myTransform.position.y - Time.deltaTime);
        }
        else
        {
            if (_myTransform.position.y < posY)
            {
                _myTransform.position = new Vector2(_myTransform.position.x, _myTransform.position.y + Time.deltaTime);

            }
        }
        // hacer trigger detector abajo con un aREA QUE COMPRUEBE SI TOCA ALGO PA NO BAJAR
    }
}
