using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall_Hit : MonoBehaviour
{
	[SerializeField] GameObject m_playerStatus;
	[SerializeField] float m_damage;

    // Start is called before the first frame update
    void Start()
    {
        if(!m_playerStatus)
		{
			m_playerStatus = GameObject.FindWithTag("playerStatus");
		}
    }
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			m_playerStatus.GetComponent<Status_Player>().GetHit(m_damage);
		}
	}
}
