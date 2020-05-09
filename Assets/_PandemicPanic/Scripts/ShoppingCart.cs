using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart :MonoBehaviour, Pickupable, IObjective
{
    public LayerMask m_itemLayermask;
    public int m_cartOwner;
    private bool m_isHeld;
    public Vector3 m_castDimensions;

    private StoreManager m_storeManager;
    private ObjectPooler m_pooler;
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
    }

    public bool IsHeld()
    {
        return m_isHeld;
    }

    public void Pickup(int p_owner)
    {
        m_cartOwner = p_owner;
        m_isHeld = true;
    }

    private void OnDrawGizmos()
    {
        if (!m_isDebugging) return;
        Gizmos.color = m_debuggingColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, m_castDimensions);
    }

    public void CheckObjective()
    {
        Collider[] cols = Physics.OverlapBox(transform.position, m_castDimensions / 2, transform.rotation, m_itemLayermask);
        foreach (Collider col in cols)
        {
            m_storeManager.AddScore(m_cartOwner, col.GetComponent<AddPoints>().GetAddedPoints());
            m_pooler.ReturnToPool(col.gameObject);
        }
    }
    
}
