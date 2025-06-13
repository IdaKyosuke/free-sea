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
		"������",
		"��",
		"���",
		"�r�q��",
		"�Z�p"
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
		// ���O��\��
		m_name.SetText("���O�F" + m_playerStatus.GetComponent<Status_Player>().GetDemon().GetComponent<Demon_Status>().GetName());

		// �X�e�[�^�X��\��
		m_faviriteStatus.SetText("�w " + m_showStatusName[(int)m_playerStatus.GetComponent<Status_Player>().GetDemon().GetComponent<Demon_Status>().GetDemonType()] + " �x���i��");
    }
}
