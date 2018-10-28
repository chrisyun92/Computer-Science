using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressurizedTubeScript : MonoBehaviour {

    public Slider m_Slider;
    public float m_MaxWaterLevel = 100f;
    public float m_TimeToActivate = 1f;
    public float m_FillVelocity = 0.0f;
    public AudioSource aud;
    public bool m_isEmpty = false;
    public bool isFull = true;

    public void Activate()
    {
        StartCoroutine(ActivateTube(m_isEmpty));
        m_isEmpty = !m_isEmpty;
    }

    IEnumerator ActivateTube(bool shouldFill)
    {
        float elapsedTime = 0f;
        while (elapsedTime < m_TimeToActivate)
        {
            elapsedTime += Time.deltaTime;
            if (shouldFill)
            {
                m_Slider.value = 100f * elapsedTime / m_TimeToActivate;
                isFull = true;
            }
            else
            {
                m_Slider.value = 100f * (1 - elapsedTime / m_TimeToActivate);
                isFull = false;
            }
            yield return null;
        }
    }

    public bool IsTubeFull()
    {
        return isFull;
    }
}
