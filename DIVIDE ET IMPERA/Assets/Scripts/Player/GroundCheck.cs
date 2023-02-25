using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    #region References
    #endregion
    #region Parameters
    #endregion
    #region Properties 
    public bool _isGrounded;
    #endregion
    #region Methods
    //Cuando los pies del jugador (o sea el Ground Check) toca el suelo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isGrounded = true;
    }
    //Cuando los pies del jugador (o sea el Ground Check) dejan de tocar el suelo
    private void OnTriggerExit2D(Collider2D collision)
    {
        _isGrounded = false;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
