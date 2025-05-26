using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneGaugeManager : MonoBehaviour
{
	[SerializeField] Image m_gauge; // ƒQ[ƒW‚Ì‰æ‘œ
	[SerializeField] float m_chargeValue = 0.1f;    // UŒ‚‚ÌƒqƒbƒgŽž‚É—­‚Ü‚éƒQ[ƒW—Ê
	[SerializeField] float m_maxValue = 100.0f; // ƒQ[ƒW‚ÌÅ‘å—Ê
	private float m_currentValue;	// Œ»Ý‚ÌƒQ[ƒW—Ê

    // Start is called before the first frame update
    void Start()
    {
		m_currentValue = 0;
	}

    // Update is called once per frame
    void Update()
    {
        m_gauge.fillAmount = m_currentValue / m_maxValue;
    }

	public void AddGauge()
	{
        if (m_maxValue > m_currentValue)
        {
			m_currentValue += m_chargeValue;
			// ƒQ[ƒWÅ‘å—Ê‚ð’´‚¦‚½‚Æ‚«‚É•â³‚·‚é
			if(m_currentValue >= m_maxValue)
			{
				m_currentValue = m_maxValue;
			}
        }
	}
}
