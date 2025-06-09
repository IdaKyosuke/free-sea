using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Status_Player : MonoBehaviour
{
	// �v���C���[�̃X�e�[�^�X��������ScriptableObject
	[SerializeField] Status m_status;
	[SerializeField] int m_needExp = 10;
	[SerializeField] int m_getSkillPoint = 2;	// ���x���A�b�v�ł��炦��X�L���|�C���g
	private GameObject m_demon;	// �_�񒆂̈���

	private Status.StatusType m_statusType;
	private int m_currentExp;   // ���ݗ��܂��Ă���o���l
	private int m_nextExp;  // ���̃��x���ɕK�v�Ȍo���l
	private float[] m_statusValue = new float[(int)Status.StatusType.Length - 1];	// �X�e�[�^�X�̎����l�̔z��

	// Start is called before the first frame update
	void Start()
    {
		m_status.Initialize();
		m_currentExp = 0;
		m_nextExp = m_needExp;
		if(!m_demon)
		{
			m_demon = GameObject.FindWithTag("demon_blue");
		}
	}

	private void Update()
	{
		// �X�e�[�^�X�̎����l���v�Z����
		for(int i = 0; i < (int)Status.StatusType.Length - 1; i++)
		{
			m_statusValue[i] = 
				GetStatus((Status.StatusType)i) * m_demon.GetComponent<Move_Demon>().GetMag((Status.StatusType)i);
		}
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
				// �X�L���|�C���g�̉��Z
				GetSkillPoint();
			} while (m_currentExp >= m_nextExp);
		}
	}

	// �X�L���|�C���g�̉��Z
	private void GetSkillPoint()
	{
		// �����̍D�ޔ\�͂ɂ������񊄂�U����
		m_status.AddSkillPoint(m_demon.GetComponent<Move_Demon>().GetDemonType(), m_getSkillPoint);
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

	// �X�e�[�^�X�擾�i���x���j
	public int GetStatus(Status.StatusType type)
	{
		int value = m_status.GetStatus(type);
		return value;
	}

	// �X�e�[�^�X�̎����l���擾
	public float GetStatusValue(Status.StatusType type)
	{
		return m_statusValue[(int)type];
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

	// �����ƌ_�񂷂�
	public void SetDemon(GameObject demon)
	{
		m_demon = demon;
	}

	//�_�񒆂̈������������擾
	public GameObject GetDemon()
	{
		return m_demon;
	}
}
