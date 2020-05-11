using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObject : MonoBehaviour, Pickupable
{
    private bool m_isHeld;
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
        m_rb.isKinematic = false;
        m_isHeld = true;
        
    }

    public GameObject ReturnGameObject()
    {
        return gameObject;
    }

    private Rigidbody m_rb;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }


}
