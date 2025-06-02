using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.AI;

public class Move_Enemy : MonoBehaviour
{
	[SerializeField] GameObject m_comboManager; // �R���{���Z�p
	[SerializeField] GameObject m_hitEffect;    // �q�b�g�G�t�F�N�g
	[SerializeField] GameObject m_playerStatus; // �v���C���[�̃X�e�[�^�X�Ǘ��p�I�u�W�F�N�g
	[SerializeField] GameObject m_enemyAnimator;	// �G�̃A�j���[�V�������Ǘ�����I�u�W�F�N�g
	[SerializeField] int m_exp = 5;	// ������o���l
	[SerializeField] int m_hp = 1;  // �̗�

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
	}

	// Update is called once per frame
	void Update()
	{
		// �̗͊Ǘ�
		if (m_hp == 0)
		{
			m_playerStatus.GetComponent<Status_Player>().AddExp(m_exp);
			m_hp = 1;
		}

		if (m_enemyAnimator.GetComponent<Animation_Enemy_Angel>().CanMove())
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
		// �v���C���[�ɍU�����ꂽ��
		if (other.gameObject.CompareTag("Weapon_Player"))
		{
			// �R���{�����Z
			m_comboManager.GetComponent<ComboManager>().AddCombe();

			// HitEffect���o��
			Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
			Instantiate(m_hitEffect, hitPos, Quaternion.identity);

			// �_���[�W���󂯂�
			m_hp--;
		}
	}
}
