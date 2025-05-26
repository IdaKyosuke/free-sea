using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
	[SerializeField] GameObject m_zoneManager;	// �Q�[�W���Z�p
	[SerializeField] float m_comboResetTime = 3.0f; // �R���{�����Z�b�g����܂ł̊Ԋu
	private float m_durationTime;	// ���Ԍv���p
	private int m_currentCombo;	// ���݂̃t���[���ł̃R���{��
	private int m_pastCombo;	// 1�t���[���O�̃R���{��

    // Start is called before the first frame update
    void Start()
    {
		if(!m_zoneManager)
		{
			m_zoneManager = GameObject.FindWithTag("zoneGaugeManager");
		}
		m_currentCombo = 0;
		m_pastCombo = 0;
		m_durationTime = 0;
	}

    // Update is called once per frame
    void Update()
    {
		// �O�t���[������R���{���������Ă��Ȃ��Ƃ�
        if(m_currentCombo == m_pastCombo)
		{
			m_durationTime += Time.deltaTime;
			// �R���{���Z�b�g���Ԃ𒴂����Ƃ�
			if(m_durationTime >= m_comboResetTime)
			{
				// �R���{�����Z�b�g
				m_currentCombo = 0;
				m_pastCombo = 0;
			}
		}
		else
		{
			// �o�ߎ��Ԃ����Z�b�g
			m_durationTime = 0;
		}

		m_pastCombo = m_currentCombo;
	}

	public void AddCombe()
	{
		m_currentCombo++;
		// �Q�[�W�����Z
		m_zoneManager.GetComponent<ZoneGaugeManager>().AddGauge();
	}

	public int GetCombe()
	{
		return m_currentCombo;
	}
}
