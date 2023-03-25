using UnityEngine;

public class LegsComponent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PataformaComponent>())
        {
            Destroy(gameObject);
        }
    }
}
