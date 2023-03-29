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
    public float CurrentBoneState { get { return _currentBoneState; } }
    // el estado al m�ximo de la barra
    [SerializeField]
    private float _maxBoneState;
    public float MaxBoneState { get { return _maxBoneState; } }
    #endregion

    #region methods
    public void BoneDamage(float _damage) // aplica da�o de ca�da a la barra
    {
        _currentBoneState -= _damage;
    }

    public void SetBar(float health)
    {
        _currentBoneState = health; //Setea vida actual
    }
    #endregion

    private void Start()
    {
        SetBar(_maxBoneState);
    }

    void Update()
    {
        _boneStateBar.fillAmount = (_currentBoneState / _maxBoneState); // actualiza el estado de la barra
    }
}
