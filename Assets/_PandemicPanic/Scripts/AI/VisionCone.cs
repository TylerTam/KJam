using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class VisionEvent : UnityEngine.Events.UnityEvent { }
public class VisionCone : MonoBehaviour
{
    public LayerMask m_obscuringLayer;

    private List<GameObject> m_objectsInVision = new List<GameObject>();
    public VisionEvent m_playerDetectedEvent;
    public bool CanSee(GameObject p_target)
    {
        if (m_objectsInVision.Contains(p_target))
        {
            if (!Physics.Linecast(transform.position, p_target.transform.position, m_obscuringLayer))
            {
                return true;
            }
        }
        return false;
    }

    public Transform GetFirstDetectedPlayer()
    {
        return m_objectsInVision[0].transform;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!m_objectsInVision.Contains(other.transform.gameObject))
        {
            m_objectsInVision.Add(other.transform.gameObject);
            m_playerDetectedEvent.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (m_objectsInVision.Contains(other.transform.gameObject))
        {
            m_objectsInVision.Remove(other.transform.gameObject);
        }
    }
}
