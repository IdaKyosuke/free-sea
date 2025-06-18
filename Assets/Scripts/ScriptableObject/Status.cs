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

	// �����X�e�[�^�X�̔z��
	private int[] m_firstStatus = new int[(int)StatusType.Length];

	// �X�e�[�^�X�̔z��
	private int[] m_status = new int[(int)StatusType.Length];

	// �v���C���[�p�̊֐�
	// �X�e�[�^�X�̏�����
	public void Initialize()
	{
		// �ύX�p�X�e�[�^�X���쐬
		m_status[(int)StatusType.Hp] = m_hp;
		m_status[(int)StatusType.Atk] = m_atk;
		m_status[(int)StatusType.Def] = m_def;
		m_status[(int)StatusType.Spd] = m_spd;
		m_status[(int)StatusType.SkillPoint] = m_skillPoint;

		// �����X�e�[�^�X��z��
		m_firstStatus[(int)StatusType.Hp] = 1;
		m_firstStatus[(int)StatusType.Atk] = 1;
		m_firstStatus[(int)StatusType.Def] = 1;
		m_firstStatus[(int)StatusType.Spd] = 1;
		m_firstStatus[(int)StatusType.SkillPoint] = 0;
	}

	public void LvUp() { m_lv++; }
	public int GetLv() {  return m_lv; }

	// ---- �X�L���|�C���g����U��p ----
	public void UseSkillPoint(StatusType type, int point) 
	{
		// �c��X�L���|�C���g��0�������Ƃ���0�܂ł̃|�C���g���g����悤����
		if (m_status[(int)StatusType.SkillPoint] - point < 0)
		{
			// �C�ӂ̃X�e�[�^�X������
			m_status[(int)type] += m_status[(int)StatusType.SkillPoint];
			// �X�L���|�C���g��0�ɍ��킹��
			m_status[(int)StatusType.SkillPoint] = 0;

			return;
		}

		// �C�ӂ̃X�e�[�^�X������
		m_status[(int)type] += point;
		// �X�L���|�C���g������
		m_status[(int)StatusType.SkillPoint]--; 
	}

	// �X�L���U�蒼���p
	public void ResetSkillPoint(StatusType type, int point)
	{
		// �����l�������ꍇ�͏����l�܂ł��������Ȃ�
		if (m_status[(int)type] - point < m_firstStatus[(int)type])
		{
			// �Œ�l�܂ŉ��������̗]��|�C���g��ǉ�
			int p = m_status[(int)type] - m_firstStatus[(int)type];
			m_status[(int)StatusType.SkillPoint] += p;
			// ���݂̃X�e�[�^�X�������l�ɍ��킹��
			m_status[(int)type] = m_firstStatus[(int)type];
			return;
		}
		// �C�ӂ̃X�e�[�^�X���㉻
		m_status[(int)type] -= point;
		// �X�L���|�C���g��ǉ�
		m_status[(int)StatusType.SkillPoint] += point;
	}

	// ���x���A�b�v�ŃX�L���|�C���g�����Z����i�_�񒆂̈������D�ރX�e�[�^�X�Ɏ����Ŋ���U����j
	public void AddSkillPoint(StatusType type, int point)
	{
		m_status[(int)type] += point;
	}

	// �X�e�[�^�X�擾�p
	public int GetStatus(StatusType type)
	{
		return m_status[(int)type];
	}
}
