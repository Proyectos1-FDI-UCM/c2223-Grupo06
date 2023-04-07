using UnityEngine;
using UnityEngine.Tilemaps;

public class BallComponent : MonoBehaviour
{
    private bool _start = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Tilemap>() && _start)
        {
            // sfx
            if(SFXComponent.Instance != null)
                SFXComponent.Instance.SFXObjects(2);
        }

        _start = true;
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
