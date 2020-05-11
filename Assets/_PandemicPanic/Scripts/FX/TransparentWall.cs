using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWall : MonoBehaviour
{
    public Material m_defaultMaterial, m_transparentMaterial;
    private List<GameObject> m_playerObjects = new List<GameObject>();
    private MeshRenderer m_renderer;

    private void Start()
    {
        m_renderer = transform.parent.GetComponent<MeshRenderer>();
    }
    private void ToggleMaterial(bool p_active)
    {
        
        m_renderer.material = (p_active) ? m_transparentMaterial : m_defaultMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        m_playerObjects.Add(other.gameObject);
        ToggleMaterial(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_playerObjects.Contains(other.gameObject))
        {
            m_playerObjects.Remove(other.gameObject);
        }
        if (m_playerObjects.Count == 0)
        {
            ToggleMaterial(false);
        }
    }
}
