using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : AddPoints, Pickupable
{
    private int m_owner;
    private bool m_held, m_dropped;
    public float m_detectRadius;
    public LayerMask m_shoppingCartLayer;

    [Header("Checkout")]
    public float m_checkOutTime;
    private PlayerHoldUi m_playerHoldUi;
    private Coroutine m_checkoutCoroutine;
    private bool m_inCheckout;
    private bool m_canBeHeld = true;

    [Header("Cosmetic Unlock")]
    public bool m_unlocksCosmetic;
    public int m_cosmeticType;
    public GameObject m_cosmeticGlow;

    [Header("Debug Gizmos")]
    public bool m_debugGizmos;
    public Color m_gizColors = Color.blue;

    private WaitForSeconds m_searchDelay;
    private Coroutine m_searchCoroutune;

    private void Awake()
    {
        m_searchDelay = new WaitForSeconds(.025f);
    }
    public void DropObject()
    {
        m_held = false;
        m_dropped = true;
        m_searchCoroutune = StartCoroutine(DelayDetect());

        if (m_inCheckout)
        {
            if (m_checkoutCoroutine != null)
            {
                StopCoroutine(m_checkoutCoroutine);
            }
            m_playerHoldUi.ChangeCheckoutUi(false);
        }
        m_playerHoldUi.ChangeBuggyUi(false);
        m_playerHoldUi = null;
    }

    private IEnumerator DelayDetect()
    {
        yield return m_searchDelay;
        SearchForCart();
        m_searchCoroutune = null;
        m_dropped = false;
    }

    private IEnumerator DelayDetectCheckout()
    {
        yield return m_searchDelay;
        AddHeldObjectToScore();
    }

    public bool IsHeld()
    {
        if (!m_canBeHeld) return true;
        return m_held;
    }


    private void Update()
    {
        if (!m_held) return;

        m_playerHoldUi.ChangeBuggyUi(Physics.OverlapSphere(transform.position, m_detectRadius, m_shoppingCartLayer).Length > 0);

    }
    public void Pickup(int p_owner)
    {
        m_held = true;
        m_owner = p_owner;
        if (m_searchCoroutune != null)
        {
            StopCoroutine(m_searchCoroutune);
        }
        m_playerHoldUi = PlayerManager.Instance.GetPlayerProperties(m_owner).m_gameAvatar.GetComponent<PlayerHoldUi>();

        if (m_inCheckout)
        {
            ToggleInCheckout(true);
        }
    }

    private void SearchForCart()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, m_detectRadius, m_shoppingCartLayer);
        if (hits.Length > 0)
        {
            hits[0].transform.GetComponent<ShoppingCart>().DropFoodAboveCart(gameObject);
        }
    }

    public void ToggleInCheckout(bool p_inCheckout)
    {
        m_inCheckout = p_inCheckout;
        if (p_inCheckout)
        {
            if (m_held)
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
            m_playerHoldUi.ChangeCheckoutUi(false);
        }
    }

    private IEnumerator CheckoutCoroutine()
    {
        float timer = 0;
        m_playerHoldUi.ChangeCheckoutUi(true);
        while (timer < m_checkOutTime)
        {
            timer += Time.deltaTime;
            m_playerHoldUi.UpdateUI(timer / m_checkOutTime);
            yield return null;
        }
        m_playerHoldUi.ChangeCheckoutUi(false);
        m_playerHoldUi.GetComponent<APRController>().StopPickup();
        m_canBeHeld = false;
        StartCoroutine(DelayDetectCheckout());
    }

    private void AddHeldObjectToScore()
    {
        StoreManager.Instance.AddScore(m_owner, GetAddedPoints());
        GetAddedPoints();
        ObjectPooler.Instance.ReturnToPool(gameObject);
    }

    public override int GetAddedPoints()
    {
        if (m_unlocksCosmetic)
        {
            CosmeticManager.Instance.CheckCosmetic(m_owner, m_cosmeticType);
        }
        return base.GetAddedPoints();
    }

    private void OnDrawGizmos()
    {
        if (!m_debugGizmos) return;
        Gizmos.color = m_gizColors;
        Gizmos.DrawWireSphere(transform.position, m_detectRadius);
    }

    public GameObject ReturnGameObject()
    {
        return gameObject;
    }


    #region Cosmetic Item 

    public void ShowGlow(float p_time)
    {

        StartCoroutine(ShowCosmeticGlow(p_time));

    }

    private IEnumerator ShowCosmeticGlow(float p_time)
    {
        float timer = p_time;

        m_cosmeticGlow.SetActive(true);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        m_cosmeticGlow.SetActive(false);
        m_unlocksCosmetic = false;
    }
    #endregion
}
