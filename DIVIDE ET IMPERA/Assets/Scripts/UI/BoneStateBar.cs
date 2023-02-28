using UnityEngine;
using UnityEngine.UI;

public class BoneStateBar : MonoBehaviour
{
    #region references
    [SerializeField]
    // referencia a la imagen de la barra
    private Image _boneStateBar;
    public FallDamage _fallDamage;
    #endregion
    #region parameters
    // el estado actual de la barra
    [SerializeField]
    private float _currentBoneState;
    // el estado al máximo de la barra
    [SerializeField]
    private float _maxBoneState;
    #endregion

    #region methods
    public void BoneDamage(float _damage) // aplica daño de caída a la barra
    {
        _currentBoneState -= _damage;
    }
    #endregion

    // Start is called beore the first frame update
    private void Start()
    {
        _currentBoneState = _maxBoneState; // al inicio siempre se tiene la barra al máximo
    }

    // Update is called once per frame
    void Update()
    {
        _boneStateBar.fillAmount = _currentBoneState / _maxBoneState; // actualiza el estado de la barra
    }
}
