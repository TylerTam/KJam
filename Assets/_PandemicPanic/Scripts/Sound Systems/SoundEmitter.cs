using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SoundEvent : UnityEngine.Events.UnityEvent { }
[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    public List<AudioClip> m_audioClips;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClip()
    {
        m_audioSource.Stop();
        if (m_audioClips.Count > 1)
        {
            m_audioSource.clip = m_audioClips[Random.Range(0, m_audioClips.Count)];
        }
        else
        {
            m_audioSource.clip = m_audioClips[0];
        }
        m_audioSource.Play();
    }

    public void PlaySpecificClip(int p_clipIndex)
    {
        m_audioSource.Stop();
        m_audioSource.clip = m_audioClips[p_clipIndex];
        m_audioSource.Play();
    }

}
