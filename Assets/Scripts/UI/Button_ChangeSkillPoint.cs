using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Button_ChangeSkillPoint : MonoBehaviour
{
	// �{�^���ŕύX����X�e�[�^�X�̎��
	[SerializeField]Status.StatusType m_statusType;
	[SerializeField] GameObject m_playerStatus;
	[SerializeField] TextMeshProUGUI m_value;	// ���݂̐��l��\������p

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
		// ���l����ɍX�V
		m_value.SetText("{0}", m_playerStatus.GetComponent<Status_Player>().GetStatus(m_statusType));
	}

	// �|�C���g������U��{�^���p
	public void AddPoint()
	{
		m_playerStatus.GetComponent<Status_Player>().SetStatus(m_statusType, 1);
		Debug.Log("plus");
	}

	// �|�C���g��ǉ�����{�^���p
	public void RemovePoint()
	{
		m_playerStatus.GetComponent<Status_Player>().ResetStatus(m_statusType, 1);
		Debug.Log("minus");
	}
}
