using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Button_ChangeSkillPoint : MonoBehaviour
{
	// ボタンで変更するステータスの種類
	[SerializeField]Status.StatusType m_statusType;
	[SerializeField] GameObject m_playerStatus;
	[SerializeField] TextMeshProUGUI m_value;	// 現在の数値を表示する用

    // Start is called before the first frame update
    void Start()
    {
        if(!m_playerStatus)
		{
			m_playerStatus = GameObject.FindWithTag("playerStatus");
		}
    }

	private void Update()
	{
		// 数値を常に更新
		m_value.SetText("{0}", m_playerStatus.GetComponent<Status_Player>().GetStatus(m_statusType));
	}

	// ポイントを割り振るボタン用
	public void AddPoint()
	{
		m_playerStatus.GetComponent<Status_Player>().SetStatus(m_statusType, 1);
		Debug.Log("plus");
	}

	// ポイントを追加するボタン用
	public void RemovePoint()
	{
		m_playerStatus.GetComponent<Status_Player>().ResetStatus(m_statusType, 1);
		Debug.Log("minus");
	}
}
