using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.AI;

public class Move_Enemy : MonoBehaviour
{
	[SerializeField] GameObject m_comboManager; // �R���{���Z�p
	[SerializeField] GameObject m_hitEffect;    // �q�b�g�G�t�F�N�g
	[SerializeField] GameObject m_deathEffect;	// ���S���ď�����u�Ԃɏo���G�t�F�N�g
	[SerializeField] GameObject m_playerStatus; // �v���C���[�̃X�e�[�^�X�Ǘ��p�I�u�W�F�N�g
	[SerializeField] GameObject m_enemyAnimator;	// �G�̃A�j���[�V�������Ǘ�����I�u�W�F�N�g
	[SerializeField] int m_exp = 5;	// ������o���l
	[SerializeField] int m_hp = 1;  // �̗�
	[SerializeField] float m_deathEffectHeightDiff = 0.5f;  // ���S���̃G�t�F�N�g�̍�������
	[SerializeField] int m_damage;	// �v���C���[�U�����ɗ^����_���[�W

	private bool m_isDeath;
	[SerializeField] bool m_haveManyAttacks;
	

	// �ړ��p
	private NavMeshAgent m_agent;
	[SerializeField] Transform m_playerTransform;

	// Start is called before the first frame update
	void Start()
	{
		if (!m_comboManager)
		{
			m_comboManager = GameObject.FindWithTag("comboManager");
		}

		if(!m_playerStatus)
		{
			m_playerStatus = GameObject.FindWithTag("playerStatus");
		}

		m_agent = GetComponent<NavMeshAgent>();
		// �v���C���[��transform
		m_playerTransform = GameObject.FindWithTag("Player").transform;

		m_isDeath = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (m_isDeath)
		{
			if(m_enemyAnimator.GetComponent<Animation_Enemy>().FinishDeathAnim())
			{
				// ���S�A�j���[�V�������I��������
				// ������O�ɃG�t�F�N�g���o��
				Instantiate(m_deathEffect, new Vector3(transform.position.x, transform.position.y + m_deathEffectHeightDiff, transform.position.z), Quaternion.identity);
				// �I�u�W�F�N�g������
				Destroy(this.gameObject);
			}
			return;
		}

		if (m_enemyAnimator.GetComponent<Animation_Enemy>().CanMove())
		{
			m_agent.isStopped = false;
			m_agent.SetDestination(m_playerTransform.position);
		}
		else
		{
			// �U�����͈ړ��ł��Ȃ�
			m_agent.isStopped = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (m_isDeath) return;

		// �v���C���[�ɍU�����ꂽ���i�ʏ�U���j
		if (other.gameObject.CompareTag("Weapon_Player"))
		{
			// �R���{�����Z
			m_comboManager.GetComponent<ComboManager>().AddCombe();

			// HitEffect���o��
			Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
			Instantiate(m_hitEffect, hitPos, Quaternion.identity);

			// �_���[�W���󂯂�
			m_hp--;

			if(m_hp > 0)
			{
				// �܂����S���Ă��Ȃ�������U�����󂯂�A�j���[�V���������s����
				m_enemyAnimator.GetComponent<Animation_Enemy>().GetHit();
			}
			else
			{
				// ���S������
				m_isDeath = true;
				// ���S�A�j���[�V����
				m_enemyAnimator.GetComponent<Animation_Enemy>().IsDeath();
				// �v���C���[�̌o���l��ǉ�
				m_playerStatus.GetComponent<Status_Player>().AddExp(m_exp);
			}
		}

		// �K�E�Z���󂯂��Ƃ�
		if(other.gameObject.CompareTag("Attack_Special"))
		{
			m_hp = 0;
			// �v���C���[�̌o���l��ǉ�
			m_playerStatus.GetComponent<Status_Player>().AddExp(m_exp);

			// ���S�A�j���[�V����
			m_enemyAnimator.GetComponent<Animation_Enemy>().IsDeath();
			m_isDeath = true;
		}
	}

	public void RotateForPlayer()
	{
		// �v���C���[�̕�������
		transform.rotation = Quaternion.Slerp(
			transform.rotation,
			Quaternion.LookRotation(m_playerTransform.position - transform.position),
			0.2f
		);
	}

	// �^����_���[�W���R���C�_�[�ɓn������
	public int Damage()
	{
		return m_damage;
	}
}
