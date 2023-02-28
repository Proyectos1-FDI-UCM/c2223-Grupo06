using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoneStateBar : MonoBehaviour
{
    #region references
    [SerializeField]
    private Image _boneStateBar;
    [SerializeField]
    private float _currentBoneState;
    [SerializeField]
    private float _maxBoneState;
    #endregion

    // Update is called once per frame
    void Update()
    {
        _boneStateBar.fillAmount = _currentBoneState / _maxBoneState;
    }
}
