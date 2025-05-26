using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "ScriptableObject/Create Status")]
public class Status : ScriptableObject
{
	// �X�e�[�^�X�̗v�f
	public enum StatusType
	{
		Hp,
		Atk,
		Def,
		Spd,
		SkillPoint,

		Length,
	}

	// �����X�e�[�^�X
	[SerializeField] int m_lv;	// �G�̃X�e�[�^�X�̎��͎g��Ȃ�?
	[SerializeField] int m_hp;
	[SerializeField] int m_atk;
	[SerializeField] int m_def;
	[SerializeField] int m_spd;
	[SerializeField] int m_skillPoint;  // �v���C���[��p

	// �X�e�[�^�X�̔z��
	private int[] m_status = new int[(int)StatusType.Length];

	// �X�e�[�^�X�����������ꂽ���ǂ���
	private bool m_isInitialized = false;

	// �v���C���[�p�̊֐�
	// �X�e�[�^�X�̏�����
	public void Initialize()
	{
		// ���d����������Ȃ��悤�t���O�ŊǗ�
		if(!m_isInitialized)
		{
			m_status[(int)StatusType.Hp] = m_hp;
			m_status[(int)StatusType.Atk] = m_atk;
			m_status[(int)StatusType.Def] = m_def;
			m_status[(int)StatusType.Spd] = m_spd;
			m_status[(int)StatusType.SkillPoint] = m_skillPoint;
			m_isInitialized = true;
		}
	}

	public void LvUp() { m_lv++; }

	// ---- �X�L���|�C���g����U��p ----
	public void UseSkillPoint(StatusType type, int point) 
	{
		// �C�ӂ̃X�e�[�^�X������
		m_status[(int)type] += point;
		// �X�L���|�C���g������
		m_status[(int)StatusType.SkillPoint]--; 
	}

	// �X�L���U�蒼���p
	public void ResetSkillPoint(StatusType type, int point)
	{
		// �C�ӂ̃X�e�[�^�X���㉻
		m_status[(int)type] -= point;
		// �X�L���|�C���g��ǉ�
		m_status[(int)StatusType.SkillPoint]++;
	}

	// �X�e�[�^�X�擾�p
	public int GetStatus(StatusType type)
	{
		return m_status[(int)type];
	}
}
