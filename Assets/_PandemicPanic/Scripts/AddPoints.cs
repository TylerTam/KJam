using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPoints : MonoBehaviour
{
    public int m_addedPoints  = 1;

    public virtual int GetAddedPoints()
    {
        Checkout.Instance.CheckoutComplete();
        return m_addedPoints;
    }
}
