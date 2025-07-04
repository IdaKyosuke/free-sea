using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵の攻撃判定用
public class AttackCollider : MonoBehaviour
{
	[SerializeField] GameObject m_playerStatus;
	[SerializeField] GameObject m_parent;   // 攻撃判定の親オブジェクト

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
		// プレイヤーにダメージを与える処理
		if(other.gameObject.CompareTag("playerGetHitCol"))
		{
			m_playerStatus.GetComponent<Status_Player>().GetHit(m_damage);
		}
	}
}
