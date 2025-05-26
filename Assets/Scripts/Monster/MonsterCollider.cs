using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
	private bool isCombat = false;

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			isCombat = true;
			// �v���C���[�����p�R���C�_�[���A�N�e�B�u�ɂ���
			GetComponent<SphereCollider>().enabled = false;
		}
	}

	public bool IsCombat()
	{
		return isCombat;
	}
}
