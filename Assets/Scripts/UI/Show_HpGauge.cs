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
		// 現在のhpを取得
		float currentValue = m_playerStatus.GetComponent<Status_Player>().CurrentHp();
		// 最大Hpを取得
		float maxValue = m_playerStatus.GetComponent<Status_Player>().GetStatusValue(Status.StatusType.Hp);
		// ゲージの更新
		m_gauge.fillAmount = currentValue / maxValue;
	}
}
