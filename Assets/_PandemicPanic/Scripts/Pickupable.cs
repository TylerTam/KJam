using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Pickupable
{
    void Pickup(int p_owner);
    /// <summary>
    /// Return true if the object is held
    /// </summary>
    /// <returns></returns>
    bool IsHeld();

    void DropObject();

    GameObject ReturnGameObject();
}
