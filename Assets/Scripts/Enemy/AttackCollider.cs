using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�̍U������p
public class AttackCollider : MonoBehaviour
{
	[SerializeField] GameObject m_playerStatus;
	[SerializeField] GameObject m_parent;   // �U������̐e�I�u�W�F�N�g

	[SerializeField] int m_damage;

	void Start()
	{
		if(!m_playerStatus)
		{
			m_playerStatus = GameObject.FindWithTag("playerStatus");
		}

		if(m_parent)
		{
			m_damage = m_parent.GetComponent<Move_Enemy>().Damage();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		// �v���C���[�Ƀ_���[�W��^���鏈��
		if(other.gameObject.CompareTag("playerGetHitCol"))
		{
			m_playerStatus.GetComponent<Status_Player>().GetHit(m_damage);
		}
	}
}
