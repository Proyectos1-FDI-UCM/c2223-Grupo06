using UnityEngine;
using UnityEngine.UI;

public class BoneStateBar : MonoBehaviour
{
    #region references
    [SerializeField]
    // referencia a la imagen de la barra
    private Image _boneStateBar;
    // referencia al da�o de ca�da
    public FallDamage _fallDamage;
    #endregion
    #region parameters
    // el estado actual de la barra
    [SerializeField]
    private float _currentBoneState;
    // el estado al m�ximo de la barra
    [SerializeField]
    private float _maxBoneState;
    #endregion

    #region methods
    public void BoneDamage(float _damage) // aplica da�o de ca�da a la barra
    {
        _currentBoneState -= _damage;
    }

    public void ResetBar()
    {
        _currentBoneState = _maxBoneState; // al inicio siempre se tiene la barra al m�ximo
    }
    #endregion

    private void Start()
    {
        ResetBar();
    }

    void Update()
    {
        _boneStateBar.fillAmount = (_currentBoneState / _maxBoneState); // actualiza el estado de la barra
    }
}
