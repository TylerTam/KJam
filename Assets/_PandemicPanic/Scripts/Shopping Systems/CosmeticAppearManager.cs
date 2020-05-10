using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticAppearManager : MonoBehaviour
{

    public List<CosmeticAppearTimes> m_cosmeticAppearTimes;
    [System.Serializable]
    public struct CosmeticAppearTimes
    {
        public int m_numberOfItems;
        public float m_spawnTime;
        public float m_despawnTime;
    }

    private int m_currentTimeIndex = 0;
    private float m_startTime;

    private void Start()
    {
        m_startTime = StoreManager.Instance.m_gameTime;
    }
    public void CheckCosmeticAppearTime(float p_time)
    {
        if (m_currentTimeIndex >= m_cosmeticAppearTimes.Count) return;
        if (m_startTime - p_time >= m_cosmeticAppearTimes[m_currentTimeIndex].m_spawnTime)
        {
            ShowCosmeticItems(m_currentTimeIndex);
            m_currentTimeIndex++;
        }
        
    }

    private void ShowCosmeticItems(int p_index)
    {
        foreach(FoodObject food in StoreManager.Instance.GetRandomFoodObjects(m_cosmeticAppearTimes[p_index].m_numberOfItems))
        {
            food.ShowGlow(m_cosmeticAppearTimes[p_index].m_despawnTime);
        }
    }
}
