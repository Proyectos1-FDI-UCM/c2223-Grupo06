using UnityEngine;

public class WBComponent : MonoBehaviour
{
    #region references
    [SerializeField] private GameObject _puerta;

    [SerializeField]
    Transform pointA;
    [SerializeField]
    Transform pointB;
    #endregion

    #region Properties

    Collider2D[] colliders;

    #endregion
    private SpriteRenderer _mySpriteRenderer;


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
    }

    // Update is called once per frame
    void Update()
    {

        colliders = Physics2D.OverlapAreaAll(pointA.position, pointB.position);

        int i = 0;
        while (i < colliders.Length && colliders[i].gameObject.GetComponent<WeightComponent>() == null) i++;

        if (i != colliders.Length)
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
}
