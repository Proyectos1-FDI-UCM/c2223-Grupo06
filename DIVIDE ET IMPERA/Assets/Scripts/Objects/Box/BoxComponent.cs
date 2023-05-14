using UnityEngine;

public class BoxComponent : MonoBehaviour
{
    #region Parameters

    #endregion
    #region Properties
    private int _legsConected; //-1 sin, 0 normales, 1 alubia
    public int LegsConnected { get { return _legsConected; } }
    [SerializeField]
    private bool _canInteract;
    #endregion
    #region Methods
    public void ConnectOrDisconnectLegs(int legs)
    {
        if (_canInteract)
        {
            _legsConected = legs;
            if (_legsConected == -1)
            {
                if (!PlayerManager.Instance.Alubiat)
                    PlayerManager.Instance.RecogerAlubiat();
                else
                    PlayerManager.Instance.HolaPiernas();
            }
            if (_legsConected == 0)
            {
                PlayerManager.Instance.AdiosPiernas();
            }
            if (_legsConected == 1)
            {
                PlayerManager.Instance.SoltarAlubiat();
            }

        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _legsConected = -1;
    }
}
