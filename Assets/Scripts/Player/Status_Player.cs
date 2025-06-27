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

	// ----- �����֘A -----
	private GameObject m_demon; // �_�񒆂̈���
	[SerializeField] GameObject[] m_demonList;  // �_��\�Ȉ����̃��X�g
	private int m_demonIndex;   // �_�񒆂̈�����index�ԍ�
	[SerializeField] GameObject m_changeSmoke;  // ������ύX���鎞�̉�
	[SerializeField] float m_waitChangeTime;    // �����̌����ڂ�ύX����܂ł̎���
	[SerializeField] string m_upKey;	// ������Index�ԍ����グ��p
	[SerializeField] string m_downKey;  // ������Index�ԍ���������p
	private bool m_isDemonChange;		// ���݈�����ύX����

	private int m_currentExp;   // ���ݗ��܂��Ă���o���l
	private int m_nextExp;  // ���̃��x���ɕK�v�Ȍo���l
	private float[] m_statusValue = new float[(int)Status.StatusType.Length - 1];   // �X�e�[�^�X�̎����l�̔z��

	private float m_hp; // ���݂̗̑�
	private bool m_isDeath; // ���񂾂�
	private bool m_isInvincible;	// ���ݖ��G���Ԃ�

	// Start is called before the first frame update
	void Start()
    {
		m_status.Initialize();
		m_currentExp = 0;
		m_nextExp = m_needExp;
		m_isDeath = false;
		m_demonIndex = 0;
		m_isDemonChange = false;
		if (!m_demon)
		{
			m_demon = GameObject.FindWithTag("demon_blue");
		}
		// �X�e�[�^�X�̎����l���v�Z����
		for (int i = 0; i < (int)Status.StatusType.Length - 1; i++)
		{
			m_statusValue[i] =
				GetStatus((Status.StatusType)i) * m_demon.GetComponent<Demon_Status>().GetMag((Status.StatusType)i);
		}
		// hp��������
		m_hp = m_statusValue[(int)Status.StatusType.Hp];
	}

	private void Update()
	{
		// ���񂾂����f
		if(m_hp <= 0)
		{
			m_isDeath = true;
			return;
		}
		// �X�e�[�^�X�̎����l���v�Z����
		for(int i = 0; i < (int)Status.StatusType.Length - 1; i++)
		{
			m_statusValue[i] = 
				GetStatus((Status.StatusType)i) * m_demon.GetComponent<Demon_Status>().GetMag((Status.StatusType)i);


			// hp�Ɋւ���X�e�[�^�X���������Ƃ��A���݂�hp����������猻�݂�hp��␳����
			if ((Status.StatusType)i == Status.StatusType.Hp)
			{
				if(m_statusValue[i] < m_hp)
				{
					m_hp = m_statusValue[i];
				}
			}
		}

		// ���݈�����ύX���łȂ���
		if(!m_isDemonChange)
		{
			// ������ύX����p�̃L�[�������ꂽ��
			if(Input.GetKeyDown(m_upKey))
			{
				StartCoroutine(ChangeDemon(1));
			}
			else if(Input.GetKeyDown(m_downKey))
			{
				StartCoroutine(ChangeDemon(-1));
			}
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
				// ���x���A�b�v�G�t�F�N�g
				ActiveLevelUpEffect();
			} while (m_currentExp >= m_nextExp);
		}
	}

	// �X�L���|�C���g�̉��Z
	private void GetSkillPoint()
	{
		// �����̍D�ޔ\�͂ɂ������񊄂�U����
		m_status.AddSkillPoint(m_demon.GetComponent<Demon_Status>().GetDemonType(), m_getSkillPoint);
	}

	// �_���[�W����
	public void GetHit(float damage)
	{
		// ���G���Ԓ��͏������΂�
		if (!m_isInvincible)
		{
			m_hp -= damage;
			if (m_hp <= 0)
			{
				// �Œ�l��0�ɂ���
				m_hp = 0;
			}
		}
	}

	// �U������
	public int TakeHit()
	{
		int damage = m_status.GetStatus(Status.StatusType.Atk);
		return damage;
	}


	// ----- �X�e�[�^�X���擾 -----
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

	// ���݂�Hp���擾
	public float CurrentHp()
	{
		return m_hp;
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

	// ���S�t���O��Ԃ�
	public bool IsDeath()
	{
		return m_isDeath;
	}

	// ���G���Ԃ��ǂ������肷��
	public void SetInvincible(bool flg)
	{
		m_isInvincible = flg;
	}

	// �_�񒆂̈�����ύX����(upkey=>1, downkey=>-1)
	private IEnumerator ChangeDemon(int key)
	{
		// ������ύX���̃t���O�𗧂Ă�
		m_isDemonChange = true;

		// �C���f�b�N�X�ԍ���ύX
		m_demonIndex += key;

		// �ԍ��̕␳
		if (m_demonList.Length <= m_demonIndex)
		{
			m_demonIndex = 0;
		}
		else if(m_demonIndex < 0)
		{
			m_demonIndex = m_demonList.Length - 1;
		}

		// �ύX�O�ɉ����o��
		Instantiate(m_changeSmoke, m_demon.transform.position, Quaternion.EulerAngles(new Vector3(-90, 0, 0)));

		// �w�肵�����ԑ҂�
		yield return new WaitForSeconds(m_waitChangeTime);

		// �ύX�O�̈������A�N�e�B�u��
		m_demon.SetActive(false);

		// �_�񂷂鈫����ύX
		SetDemon(m_demonList[m_demonIndex]);

		// �_���̈������A�N�e�B�u��
		m_demon.SetActive(true);

		// �����̕ύX���I�������̂Ńt���O��܂�
		m_isDemonChange = false;

		yield return null;
	}

	// �Q�[���I�����ɑ���֐�
	private void OnApplicationQuit()
	{
		// �e�X�g�Œl�𔽉f������
		m_status.Save();
	}

	// ���x���A�b�v���ɃG�t�F�N�g���o��
	private void ActiveLevelUpEffect()
	{
		GameObject.FindWithTag("Player").GetComponent<Move_Player>().ActiveLevelUpEffect();
	}
}
