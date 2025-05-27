using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Status_Player : MonoBehaviour
{
	// ScriptableObject
	[SerializeField] Status m_status;
	//private int m_devil;
	[SerializeField] int m_needExp = 10;

	private Status.StatusType m_statusType;
	private int m_currentExp;   // ���ݗ��܂��Ă���o���l
	private int m_nextExp;  // ���̃��x���ɕK�v�Ȍo���l

	// Start is called before the first frame update
	void Start()
    {
		m_status.Initialize();
		m_currentExp = 0;
		m_nextExp = m_needExp;
	}

	// �o���l�̉��Z -> ���x���A�b�v
	public void AddExp(int exp)
	{
		m_currentExp += exp;

		// ���x���A�b�v����
		if (m_currentExp >= m_nextExp)
		{
			// �オ�镪�͂܂Ƃ߂Ă�����
			do
			{
				// ���ӂꂽ���̌o���l�͎����z��
				m_currentExp -= m_nextExp;
				// ���̃��x���ɕK�v�Ȍo���l�𑝂₷
				m_nextExp = (int)(m_nextExp * 1.3f);
				// ���x���A�b�v
				m_status.LvUp();
			} while (m_currentExp >= m_nextExp);
		}
	}

	// �_���[�W����
	public int GetHit()
	{
		int damage = m_status.GetStatus(Status.StatusType.Atk);
		return damage;
	}

	// ���x�����擾
	public int GetLv() 
	{
		int l = m_status.GetLv();
		return l;
	}

	// �X�e�[�^�X�擾
	public int GetStatus(Status.StatusType type)
	{
		int value = m_status.GetStatus(type);
		return value;
	}

	// ---- �X�e�[�^�X�ύX ----
	// ����U��
	public void SetStatus(Status.StatusType type, int point)
	{
		m_status.UseSkillPoint(type, point);
	}

	// �U�蒼��
	public void ResetStatus(Status.StatusType type, int point)
	{
		m_status.ResetSkillPoint(type, point);
	}
}
