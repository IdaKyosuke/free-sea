using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_HpGauge : MonoBehaviour
{
	[SerializeField] Image m_gauge;
	[SerializeField] GameObject m_playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        if(!m_playerStatus)
		{
			m_playerStatus = GameObject.FindWithTag("playerStatus");
		}
    }

    // Update is called once per frame
    void Update()
    {
		// ���݂�hp���擾
		float currentValue = m_playerStatus.GetComponent<Status_Player>().CurrentHp();
		// �ő�Hp���擾
		float maxValue = m_playerStatus.GetComponent<Status_Player>().GetStatusValue(Status.StatusType.Hp);
		// �Q�[�W�̍X�V
		m_gauge.fillAmount = currentValue / maxValue;
	}
}
