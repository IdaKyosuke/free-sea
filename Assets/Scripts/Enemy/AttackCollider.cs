using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// “G‚ÌUŒ‚”»’è—p
public class AttackCollider : MonoBehaviour
{
	[SerializeField] GameObject m_playerStatus;

	void Start()
	{
		if(!m_playerStatus)
		{
			m_playerStatus = GameObject.FindWithTag("playerStatus");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			m_playerStatus.SetActive(false);
		}
	}
}
