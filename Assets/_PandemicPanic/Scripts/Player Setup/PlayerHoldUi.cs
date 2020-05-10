using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoldUi : MonoBehaviour
{
    
    public GameObject m_canvasObject;
    public float m_heightAbovePlayer;
    public Transform m_trackingPoint;

    #region Checkout UI
    public UnityEngine.UI.Image m_checkoutFill;
    private bool m_checkoutUiActive;
    #endregion

    #region Buggy UI
    private bool m_buggyUiActive;
    public UnityEngine.UI.Image m_buggyUi;
    #endregion


    private void Update()
    {
        m_canvasObject.transform.position = m_trackingPoint.position + Vector3.up * m_heightAbovePlayer;
    }

    public void ChangeCheckoutUi(bool p_activeState)
    {
        m_checkoutUiActive = p_activeState;
        m_checkoutFill.gameObject.SetActive(p_activeState);
    }

    public void UpdateUI(float p_percent)
    {
        m_checkoutFill.fillAmount = p_percent;
    }

    public void ChangeBuggyUi(bool p_activeState)
    {
        m_buggyUiActive = p_activeState;
        m_buggyUi.gameObject.SetActive(p_activeState);
    }
}
