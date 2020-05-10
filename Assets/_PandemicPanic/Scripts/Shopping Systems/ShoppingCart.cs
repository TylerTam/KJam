using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour, Pickupable, IObjective
{
    public LayerMask m_itemLayermask;
    public int m_cartOwner;
    private bool m_isHeld;
    public Vector3 m_castDimensions;

    private StoreManager m_storeManager;
    private ObjectPooler m_pooler;
    public Transform m_dropObjectPosition;

    [Header("Checkout")]
    public float m_checkoutTime;
    private PlayerHoldUi m_playerUi;
    private bool m_inCheckout;
    private Coroutine m_checkoutCoroutine;

    [Header("Debugging")]
    public bool m_isDebugging;

    public Color m_debuggingColor = Color.red;

    private void Start()
    {
        m_storeManager = StoreManager.Instance;
        m_pooler = ObjectPooler.Instance;
    }
    public void DropObject()
    {
        m_isHeld = false;
        m_playerUi = null;
    }

    public bool IsHeld()
    {
        return m_isHeld;
    }

    public void Pickup(int p_owner)
    {
        m_cartOwner = p_owner;
        m_isHeld = true;
        m_playerUi = PlayerManager.Instance.GetPlayerProperties(p_owner).m_gameAvatar.GetComponent<PlayerHoldUi>();
        if (m_inCheckout)
        {
            ToggleInCheckout(true);
        }
    }

    private void OnDrawGizmos()
    {
        if (!m_isDebugging) return;
        Gizmos.color = m_debuggingColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, m_castDimensions);
    }

    public void ToggleInCheckout(bool p_inCheckout)
    {

        m_inCheckout = p_inCheckout;
        if (p_inCheckout)
        {
            if (Physics.OverlapBox(transform.position, m_castDimensions / 2, transform.rotation, m_itemLayermask).Length <= 0) return;
            if (m_isHeld)
            {
                m_checkoutCoroutine = StartCoroutine(CheckoutCoroutine());
            }
        }
        else
        {
            if (m_checkoutCoroutine != null)
            {
                StopCoroutine(m_checkoutCoroutine);
            }
            m_playerUi.ChangeCheckoutUi(false);
        }
    }

    private IEnumerator CheckoutCoroutine()
    {
        float timer = 0;
        m_playerUi.ChangeCheckoutUi(true);
        while (timer < m_checkoutTime)
        {
            timer += Time.deltaTime;
            m_playerUi.UpdateUI(timer / m_checkoutTime);
            yield return null;
        }
        m_playerUi.ChangeCheckoutUi(false);
        AddScore();
    }

    private void AddScore()
    {
        Collider[] cols = Physics.OverlapBox(transform.position, m_castDimensions / 2, transform.rotation, m_itemLayermask);
        foreach (Collider col in cols)
        {
            m_storeManager.AddScore(m_cartOwner, col.GetComponent<AddPoints>().GetAddedPoints());
            m_pooler.ReturnToPool(col.gameObject);
        }
    }

    public void DropFoodAboveCart(GameObject p_object)
    {
        p_object.GetComponent<Rigidbody>().velocity = Vector3.zero;
        p_object.transform.position = m_dropObjectPosition.position;

    }

    public GameObject ReturnGameObject()
    {
        return gameObject;
    }

    public void CheckObjective()
    {
        return;
    }
}
