using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : AddPoints, Pickupable
{

    private bool m_held;

    public float m_detectRadius;
    public LayerMask m_shoppingCartLayer;

    [Header("Cosmetic Unlock")]
    public bool m_unlocksCosmetic;
    public int m_cosmeticType;

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
        m_searchCoroutune = StartCoroutine(DelayDetect());
    }

    private IEnumerator DelayDetect()
    {
        yield return m_searchDelay;
        SearchForCart();
        m_searchCoroutune = null;
    }
    public bool IsHeld()
    {
        return m_held;
    }

    public void Pickup(int p_owner)
    {
        m_held = true;
        if(m_searchCoroutune != null)
        {
            StopCoroutine(m_searchCoroutune);
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

    public override int GetAddedPoints()
    {
        if (m_unlocksCosmetic)
        {
            CosmeticManager.Instance.CheckCosmetic(m_cosmeticType);
        }
        return base.GetAddedPoints();
    }

    private void OnDrawGizmos()
    {
        if (!m_debugGizmos) return;
        Gizmos.color = m_gizColors;
        Gizmos.DrawWireSphere(transform.position, m_detectRadius);
    }
}
