using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneGaugeManager : MonoBehaviour
{
	[SerializeField] Image m_gauge; // �Q�[�W�̉摜
	[SerializeField] float m_chargeValue = 0.1f;    // �U���̃q�b�g���ɗ��܂�Q�[�W��
	[SerializeField] float m_maxValue = 100.0f; // �Q�[�W�̍ő��
	private float m_currentValue;	// ���݂̃Q�[�W��

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
			// �Q�[�W�ő�ʂ𒴂����Ƃ��ɕ␳����
			if(m_currentValue >= m_maxValue)
			{
				m_currentValue = m_maxValue;
			}
        }
	}
}
