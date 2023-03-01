using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static PlayerManager.TimmyStates UIState;

    [SerializeField] private static GameObject _craneoTorsoNO;
    [SerializeField] private static GameObject _legsNO;
    [SerializeField] private static GameObject _armsNO;

    #region methods
    static public void UpdateHud(PlayerManager.TimmyStates UIState)
    {
        if (UIState == PlayerManager.TimmyStates.S1 || UIState == PlayerManager.TimmyStates.S2)
        {
            _armsNO.SetActive(true);
            _legsNO.SetActive(false);
            _craneoTorsoNO.SetActive(false);
        }
        else if (UIState == PlayerManager.TimmyStates.S3)
        {
            _legsNO.SetActive(true);
            _armsNO.SetActive(false);
            _craneoTorsoNO.SetActive(false);
        }
        else if (UIState == PlayerManager.TimmyStates.S4)
        {
            _armsNO.SetActive(true);
            _legsNO.SetActive(true);
            _craneoTorsoNO.SetActive(false);
        }
    }


    #endregion
    void Start()
    {
        _armsNO.SetActive(false);
        _legsNO.SetActive(false);
        _craneoTorsoNO.SetActive(false);
    }
}
