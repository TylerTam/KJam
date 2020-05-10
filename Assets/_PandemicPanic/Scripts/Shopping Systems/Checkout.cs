using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkout : MonoBehaviour
{
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
