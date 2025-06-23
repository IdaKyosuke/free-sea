using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForMinotaurEffect : MonoBehaviour
{
	// �~�m�^�E���X�̕���
	[SerializeField] GameObject m_weapon;

	// �G�t�F�N�g
	[SerializeField] GameObject m_impact;
	[SerializeField] GameObject m_slash;

	// �@�������̏Ռ�
	public void InstantiateImpact()
	{
		Instantiate(m_impact, m_weapon.transform.position, m_weapon.transform.rotation);
	}

	// �a�����΂�
	public void InstantiateSlash()
	{
		Instantiate(m_slash, m_weapon.transform.position, m_weapon.transform.rotation);
		Debug.Log(0);
	}
}
