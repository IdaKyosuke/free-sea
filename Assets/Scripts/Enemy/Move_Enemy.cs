using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Enemy : MonoBehaviour
{
	[SerializeField] GameObject m_comboManager; // �R���{���Z�p
	[SerializeField] GameObject m_hitEffect;	// �q�b�g�G�t�F�N�g

	// Start is called before the first frame update
	void Start()
	{
		if (!m_comboManager)
		{
			m_comboManager = GameObject.FindWithTag("comboManager");
		}
	}

	// Update is called once per frame
	void Update()
	{

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
		}
	}
}
