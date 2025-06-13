using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move_Demon : MonoBehaviour
{
	[SerializeField] Transform m_demonPos;
	private NavMeshAgent m_agent;

	private void Start()
	{
		m_agent = GetComponent<NavMeshAgent>();
		if(!m_demonPos)
		{
			m_demonPos = GameObject.FindWithTag("Player").transform.GetChild(0);
		}
	}

	private void Update()
	{
		m_agent.SetDestination(m_demonPos.position);
	}
}
