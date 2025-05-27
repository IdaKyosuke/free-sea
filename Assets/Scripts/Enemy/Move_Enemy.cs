using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Enemy : MonoBehaviour
{
	[SerializeField] GameObject m_comboManager; // �R���{���Z�p
	[SerializeField] GameObject m_hitEffect;    // �q�b�g�G�t�F�N�g
	[SerializeField] GameObject m_playerStatus;	// �v���C���[�̃X�e�[�^�X�Ǘ��p�I�u�W�F�N�g
	[SerializeField] int m_hp = 1;	// �̗�

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
	}

	// Update is called once per frame
	void Update()
	{
		if(m_hp == 0)
		{
			m_playerStatus.GetComponent<Status_Player>().AddExp(5);
			m_hp = 1;
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
