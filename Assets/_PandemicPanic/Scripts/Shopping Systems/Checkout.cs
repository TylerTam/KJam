using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkout : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IObjective>() != null)
        {
            other.GetComponent<IObjective>().CheckObjective();
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            if (other.GetComponent<FoodObject>() != null)
            {
                other.GetComponent<FoodObject>().AddHeldObjectToScore();
            }
            return;
        }
    }
}
