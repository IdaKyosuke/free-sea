using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Status_Player : MonoBehaviour
{
	// ScriptableObject
	[SerializeField] Status m_status;
	//private int m_devil;
	[SerializeField] int m_needExp = 10;

	private int m_currentExp;   // ���ݗ��܂��Ă���o���l
	private int m_nextExp;	// ���̃��x���ɕK�v�Ȍo���l

    // Start is called before the first frame update
    void Start()
    {
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
	public int GetHit() => m_status.GetStatus(Status.StatusType.Atk);
}
