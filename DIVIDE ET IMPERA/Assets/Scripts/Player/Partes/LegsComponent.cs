using UnityEngine;

public class LegsComponent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
    /*
        if (collision.gameObject.TryGetComponent<PataformaComponent>(out var pataforma))
        {
            //pataforma.PiernasConectadas = true;
            Destroy(gameObject);
        }
    */
    }
}
