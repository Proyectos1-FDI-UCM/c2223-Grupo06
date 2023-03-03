using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBComponent : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _puerta;

    private SpriteRenderer _mySpriteRenderer;

    private void Activate()
    {
        /*
        if (Collision.GetComponent<WeightComponent>)
        {
            _mySpriteRenderer.color= Color.white;
            _puerta.SetActive(true);
        }
        */
    }





    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
