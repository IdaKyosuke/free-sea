using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.AI;

public class Move_Enemy : MonoBehaviour
{
	[SerializeField] GameObject m_comboManager; // コンボ加算用
	[SerializeField] GameObject m_hitEffect;    // ヒットエフェクト
	[SerializeField] GameObject m_deathEffect;	// 死亡して消える瞬間に出すエフェクト
	[SerializeField] GameObject m_playerStatus; // プレイヤーのステータス管理用オブジェクト
	[SerializeField] GameObject m_enemyAnimator;	// 敵のアニメーションを管理するオブジェクト
	[SerializeField] int m_exp = 5;	// 得られる経験値
	[SerializeField] int m_hp = 1;  // 体力
	[SerializeField] float m_deathEffectHeightDiff = 0.5f;  // 死亡時のエフェクトの高さ差分
	[SerializeField] int m_damage;	// プレイヤー攻撃時に与えるダメージ

	private bool m_isDeath;
	[SerializeField] bool m_haveManyAttacks;
	

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

		m_isDeath = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (m_isDeath)
		{
			if(m_enemyAnimator.GetComponent<Animation_Enemy>().FinishDeathAnim())
			{
				// 死亡アニメーションが終了したら
				// 消える前にエフェクトを出す
				Instantiate(m_deathEffect, new Vector3(transform.position.x, transform.position.y + m_deathEffectHeightDiff, transform.position.z), Quaternion.identity);
				// オブジェクトを消す
				Destroy(this.gameObject);
			}
			return;
		}

		if (m_enemyAnimator.GetComponent<Animation_Enemy>().CanMove())
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
		if (m_isDeath) return;

		// プレイヤーに攻撃された時（通常攻撃）
		if (other.gameObject.CompareTag("Weapon_Player"))
		{
			// コンボを加算
			m_comboManager.GetComponent<ComboManager>().AddCombe();

			// HitEffectを出す
			Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
			Instantiate(m_hitEffect, hitPos, Quaternion.identity);

			// ダメージを受ける
			m_hp--;

			if(m_hp > 0)
			{
				// まだ死亡していなかったら攻撃を受けるアニメーションを実行する
				m_enemyAnimator.GetComponent<Animation_Enemy>().GetHit();
			}
			else
			{
				// 死亡したら
				m_isDeath = true;
				// 死亡アニメーション
				m_enemyAnimator.GetComponent<Animation_Enemy>().IsDeath();
				// プレイヤーの経験値を追加
				m_playerStatus.GetComponent<Status_Player>().AddExp(m_exp);
			}
		}

		// 必殺技を受けたとき
		if(other.gameObject.CompareTag("Attack_Special"))
		{
			m_hp = 0;
			// プレイヤーの経験値を追加
			m_playerStatus.GetComponent<Status_Player>().AddExp(m_exp);

			// 死亡アニメーション
			m_enemyAnimator.GetComponent<Animation_Enemy>().IsDeath();
			m_isDeath = true;
		}
	}

	public void RotateForPlayer()
	{
		// プレイヤーの方を向く
		transform.rotation = Quaternion.Slerp(
			transform.rotation,
			Quaternion.LookRotation(m_playerTransform.position - transform.position),
			0.2f
		);
	}

	// 与えるダメージをコライダーに渡す処理
	public int Damage()
	{
		return m_damage;
	}
}
