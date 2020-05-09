using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderFollow : MonoBehaviour
{
    public Transform m_playerFollow;

    private void Update()
    {
        transform.position = m_playerFollow.position;
    }
}
