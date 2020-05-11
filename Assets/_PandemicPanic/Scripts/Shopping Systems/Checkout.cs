using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Checkout : MonoBehaviour
{

    public static Checkout Instance;
    public SoundEvent m_checkoutEvent;

    private void Awake()
    {
        Instance = this;
    }
    public void CheckoutComplete()
    {
        m_checkoutEvent.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ShoppingCart>() != null)
        {
            other.GetComponent<ShoppingCart>().ToggleInCheckout(true);
            return;
        }

        if (other.gameObject.tag == "Object")
        {
            if (other.GetComponent<FoodObject>() != null)
            {
                other.GetComponent<FoodObject>().ToggleInCheckout(true);
                
            }
            return;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ShoppingCart>() != null)
        {
            other.GetComponent<ShoppingCart>().ToggleInCheckout(false);
            return;
        }

        if (other.gameObject.tag == "Object")
        {
            if (other.GetComponent<FoodObject>() != null)
            {
                other.GetComponent<FoodObject>().ToggleInCheckout(false);
            }
            return;
        }
    }
}
