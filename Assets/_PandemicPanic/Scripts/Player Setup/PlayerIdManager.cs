using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdManager : MonoBehaviour
{
    public int m_playerType;
    public Transform m_helmet, m_rightShoulder, m_leftShoulder, m_chestPlate, m_rightKnee, m_leftKnee;
    [HideInInspector]
    public GameObject m_helmetObj, m_rightShoulderObj, m_leftShoulderObj, m_chestPlateObj, m_rightKneeObj, m_leftKneeObj;

    private ObjectPooler m_pooler;
    private void Start()
    {
        m_pooler = ObjectPooler.Instance;
    }

    public void AssignCosmetics(GameObject p_helmet, GameObject p_rShould, GameObject p_lShould, GameObject p_chestPlate, GameObject p_rightKnee, GameObject p_leftKnee)
    {

        AssignHelmet(p_helmet);
        AssignLeftShoulder(p_lShould);
        AssignRightShoulder(p_rShould);
        AssignChest(p_chestPlate);
        AssignRightKnee(p_rightKnee);
        AssignLeftKnee(p_leftKnee);
    }

    public void AssignHelmet(GameObject p_helmet)
    {
        if (p_helmet == null) return;
        m_helmetObj = ObjectPooler.Instance.NewObject(p_helmet, m_helmet.position, m_helmet.rotation);
        m_helmetObj.transform.localScale = Vector3.one;
        m_helmetObj.transform.parent = m_helmet;
    }

    public void AssignLeftShoulder(GameObject p_leftShoulder)
    {

        if (p_leftShoulder != null)
        {
            m_leftShoulderObj = ObjectPooler.Instance.NewObject(p_leftShoulder, m_leftShoulder.position, m_leftShoulder.rotation);
            m_leftShoulderObj.transform.localScale = Vector3.one;
            m_leftShoulderObj.transform.parent = m_leftShoulder;

        }
    }
    public void AssignRightShoulder(GameObject p_rightShoulder)
    {
        if (p_rightShoulder != null)
        {
            m_rightShoulderObj = ObjectPooler.Instance.NewObject(p_rightShoulder, m_rightShoulder.position, m_rightShoulder.rotation);
            m_rightShoulderObj.transform.localScale = Vector3.one;
            m_rightShoulderObj.transform.parent = m_rightShoulder;

        }
    }

    public void AssignChest(GameObject p_chestPlate)
    {
        if (p_chestPlate == null) return;
        m_chestPlateObj = ObjectPooler.Instance.NewObject(p_chestPlate, m_chestPlate.position, m_chestPlate.rotation);
        m_chestPlateObj.transform.localScale = Vector3.one;
        m_chestPlateObj.transform.parent = m_chestPlate;
    }

    public void AssignRightKnee(GameObject p_knee)
    {
        if (p_knee == null) return;
        m_rightKneeObj = ObjectPooler.Instance.NewObject(p_knee, m_rightKnee.position, m_rightKnee.rotation);
        m_rightKneeObj.transform.localScale = Vector3.one;
        m_rightKneeObj.transform.parent = m_rightKnee;
    }
    public void AssignLeftKnee(GameObject p_knee)
    {
        if (p_knee == null) return;
        m_leftKneeObj = ObjectPooler.Instance.NewObject(p_knee, m_leftKnee.position, m_leftKnee.rotation);
        m_leftKneeObj.transform.localScale = Vector3.one;
        m_leftKneeObj.transform.parent = m_leftKnee;
    }




    #region REmove Cosmetics
    public void RemoveHelmet()
    {
        if (m_helmetObj == null) return;
        m_helmetObj.transform.parent = null;
        m_pooler.ReturnToPool(m_helmetObj);
        m_helmetObj = null;
    }
    public void RemoveLeftShoulder()
    {
        if (m_leftShoulderObj == null) return;
        m_leftShoulderObj.transform.parent = null;
        m_pooler.ReturnToPool(m_leftShoulderObj);
        m_leftShoulderObj = null;
    }
    public void RemoveRightShoulder()
    {

        if (m_rightShoulderObj == null) return;
        m_rightShoulderObj.transform.parent = null;
        m_pooler.ReturnToPool(m_rightShoulderObj);
        m_rightShoulderObj = null;
    }

    public void RemoveChest()
    {
        if (m_chestPlateObj == null) return;
        m_chestPlateObj.transform.parent = null;
        m_pooler.ReturnToPool(m_chestPlateObj);
        m_chestPlateObj = null;
    }

    public void RemoveRightKneepad()
    {
        if (m_rightKneeObj == null) return;
        m_rightKneeObj.transform.parent = null;
        m_pooler.ReturnToPool(m_rightKneeObj);
        m_rightKneeObj = null;
    }

    public void RemoveLeftKneepad()
    {
        if (m_leftKneeObj == null) return;
        m_leftKneeObj.transform.parent = null;
        m_pooler.ReturnToPool(m_leftKneeObj);
        m_leftKneeObj = null;
    }
    #endregion
}
