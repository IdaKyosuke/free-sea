using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�̍U������p
public class AttackCollider : MonoBehaviour
{
	[SerializeField] GameObject m_playerStatus;
	[SerializeField] GameObject m_parent;	// �U������̐e�I�u�W�F�N�g

	void Start()
	{
		if(!m_playerStatus)
		{
			m_playerStatus = GameObject.FindWithTag("playerStatus");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		// �v���C���[�Ƀ_���[�W��^���鏈��
		if(other.gameObject.CompareTag("Player"))
		{
			m_playerStatus.GetComponent<Status_Player>().GetHit(m_parent.GetComponent<Move_Enemy>().Damage());
		}
	}
}
