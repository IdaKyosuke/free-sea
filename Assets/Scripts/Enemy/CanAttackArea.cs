using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAttackArea : MonoBehaviour
{
	private bool m_canAttack;

	private void Start()
	{
		m_canAttack = false;
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			m_canAttack = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			m_canAttack = false;
		}
	}

	public bool GetCanAttack()
	{
		return m_canAttack;
	}
}
