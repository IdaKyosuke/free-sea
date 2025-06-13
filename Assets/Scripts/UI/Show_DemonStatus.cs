using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Show_DemonStatus : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI m_name;
	[SerializeField] TextMeshProUGUI m_faviriteStatus;
	[SerializeField] GameObject m_playerStatus;

	private string[] m_showStatusName = new string[(int)Status.StatusType.Length]
	{
		"生命力",
		"力",
		"守り",
		"俊敏さ",
		"技術"
	};

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
		// 名前を表示
		m_name.SetText("名前：" + m_playerStatus.GetComponent<Status_Player>().GetDemon().GetComponent<Demon_Status>().GetName());

		// ステータスを表示
		m_faviriteStatus.SetText("『 " + m_showStatusName[(int)m_playerStatus.GetComponent<Status_Player>().GetDemon().GetComponent<Demon_Status>().GetDemonType()] + " 』を司る");
    }
}
