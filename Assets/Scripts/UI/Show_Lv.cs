using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Show_Lv : MonoBehaviour
{
	[SerializeField] GameObject m_playerStatus;
	[SerializeField] TextMeshProUGUI m_lv;

	private void Update()
	{
		m_lv.SetText("{0}", m_playerStatus.GetComponent<Status_Player>().GetLv());
	}
}
