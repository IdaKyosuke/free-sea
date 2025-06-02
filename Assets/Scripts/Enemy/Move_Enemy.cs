using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.AI;

public class Move_Enemy : MonoBehaviour
{
	[SerializeField] GameObject m_comboManager; // コンボ加算用
	[SerializeField] GameObject m_hitEffect;    // ヒットエフェクト
	[SerializeField] GameObject m_playerStatus; // プレイヤーのステータス管理用オブジェクト
	[SerializeField] GameObject m_enemyAnimator;	// 敵のアニメーションを管理するオブジェクト
	[SerializeField] int m_exp = 5;	// 得られる経験値
	[SerializeField] int m_hp = 1;  // 体力

	// 移動用
	private NavMeshAgent m_agent;
	[SerializeField] Transform m_playerTransform;

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

		m_agent = GetComponent<NavMeshAgent>();
		// プレイヤーのtransform
		m_playerTransform = GameObject.FindWithTag("Player").transform;
	}

	// Update is called once per frame
	void Update()
	{
		// 体力管理
		if (m_hp == 0)
		{
			m_playerStatus.GetComponent<Status_Player>().AddExp(m_exp);
			m_hp = 1;
		}

		if (m_enemyAnimator.GetComponent<Animation_Enemy_Angel>().CanMove())
		{
			m_agent.isStopped = false;
			m_agent.SetDestination(m_playerTransform.position);
		}
		else
		{
			// 攻撃中は移動できない
			m_agent.isStopped = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		// プレイヤーに攻撃された時
		if (other.gameObject.CompareTag("Weapon_Player"))
		{
			// コンボを加算
			m_comboManager.GetComponent<ComboManager>().AddCombe();

			// HitEffectを出す
			Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
			Instantiate(m_hitEffect, hitPos, Quaternion.identity);

			// ダメージを受ける
			m_hp--;
		}
	}
}
