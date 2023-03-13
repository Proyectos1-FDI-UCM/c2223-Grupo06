using UnityEngine;

public class StayOnPataforma : MonoBehaviour
{
    private InputController _inputController;

    #region Methods

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PataformaComponent>()
            || collision.gameObject.GetComponent<MovingPlatformComponent>())
        {
            if(gameObject.transform.parent == null && _inputController.enabled)
            {
                // cambia el padre de timmy (no alubia, otro)
                gameObject.transform.SetParent(collision.gameObject.transform, true);
            }
        }

    }


    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PataformaComponent>()
            || collision.gameObject.GetComponent<MovingPlatformComponent>())
        {
            // cambia el padre de timmy (no alubia, otro)
            gameObject.transform.parent = null;
        }

    }


    #endregion


    private void Start()
    {
        _inputController = GetComponent<InputController>();
    }
}
