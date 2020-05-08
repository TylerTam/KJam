using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdManager : MonoBehaviour
{
    public Transform m_helmet, m_rightShoulder, m_leftShoulder, m_chestPlate;
    [HideInInspector]
    public GameObject m_helmetObj, m_rightShoulderObj, m_leftShoulderObj, m_chestPlateObj;

    private ObjectPooler m_pooler;
    private void Start()
    {
        m_pooler = ObjectPooler.Instance;
    }

    public void AssignHelmet(GameObject p_helmet)
    {
        if (p_helmet == null) return;
        m_helmetObj = ObjectPooler.Instance.NewObject(p_helmet, m_helmet.position, m_helmet.rotation);
        m_helmetObj.transform.localScale = Vector3.one;
        m_helmetObj.transform.parent = m_helmet;
    }

    public void AssignShoulders(GameObject p_rightShoulder, GameObject p_leftShoulder)
    {
        if (p_rightShoulder != null)
        {
            m_rightShoulderObj = ObjectPooler.Instance.NewObject(p_rightShoulder, m_rightShoulder.position, m_rightShoulder.rotation);
            m_rightShoulderObj.transform.localScale = Vector3.one;
            m_rightShoulderObj.transform.parent = m_rightShoulder;

        }
        if (p_leftShoulder != null)
        {
            m_leftShoulderObj = ObjectPooler.Instance.NewObject(p_leftShoulder, m_leftShoulder.position, m_leftShoulder.rotation);
            m_leftShoulderObj.transform.localScale = Vector3.one;
            m_leftShoulderObj.transform.parent = m_leftShoulder;

        }
    }

    public void AssignChest(GameObject p_chestPlate)
    {
        if (p_chestPlate == null) return;
        m_chestPlateObj = ObjectPooler.Instance.NewObject(p_chestPlate, m_chestPlate.position, m_chestPlate.rotation);
        m_chestPlateObj.transform.localScale = Vector3.one;
        m_chestPlateObj.transform.parent = m_chestPlate;
    }

    public void AssignCosmetics(GameObject p_helmet, GameObject p_rShould, GameObject p_lShould, GameObject p_chestPlate)
    {

        AssignHelmet(p_helmet);
        AssignShoulders(p_rShould, p_lShould);
        AssignChest(p_chestPlate);
    }

    public void RemoveHelmet()
    {
        if (m_helmetObj == null) return;
        m_helmetObj.transform.parent = null;
        m_pooler.ReturnToPool(m_helmetObj);
        m_helmetObj = null;
    }
    public void RemoveShoulders()
    {
        if (m_leftShoulderObj == null) return;
        if (m_rightShoulderObj == null) return;
        m_leftShoulderObj.transform.parent = null;
        m_rightShoulderObj.transform.parent = null;
        m_pooler.ReturnToPool(m_rightShoulderObj);
        m_pooler.ReturnToPool(m_leftShoulderObj);
        m_leftShoulderObj = m_rightShoulderObj = null;
    }

    public void RemoveChest()
    {
        if (m_chestPlateObj == null) return;
        m_chestPlateObj.transform.parent = null;
        m_pooler.ReturnToPool(m_chestPlateObj);
        m_chestPlateObj = null;
    }
}
