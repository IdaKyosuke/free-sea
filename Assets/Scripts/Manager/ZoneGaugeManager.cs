using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneGaugeManager : MonoBehaviour
{
	[SerializeField] Image m_gauge; // ゲージの画像
	[SerializeField] float m_chargeValue = 0.1f;    // 攻撃のヒット時に溜まるゲージ量
	[SerializeField] float m_maxValue = 100.0f; // ゲージの最大量
	private float m_currentValue;   // 現在のゲージ量

	private bool m_isMax;	// ゲージが最大か

    // Start is called before the first frame update
    void Start()
    {
		m_currentValue = 0;
		m_isMax = false;
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
			// ゲージ最大量を超えたときに補正する
			if(m_currentValue >= m_maxValue)
			{
				m_currentValue = m_maxValue;
				m_isMax = true;
			}
        }
	}

	public void ResetGauge()
	{
		m_isMax = false;
		m_currentValue = 0;
	}

	public bool IsMax()
	{
		return m_isMax;
	}
}
