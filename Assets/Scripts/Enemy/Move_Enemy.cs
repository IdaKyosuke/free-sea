using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Enemy : MonoBehaviour
{
	[SerializeField] GameObject m_comboManager; // コンボ加算用
	[SerializeField] GameObject m_hitEffect;    // ヒットエフェクト
	[SerializeField] GameObject m_playerStatus;	// プレイヤーのステータス管理用オブジェクト
	[SerializeField] int m_hp = 1;	// 体力

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
	}

	// Update is called once per frame
	void Update()
	{
		if(m_hp == 0)
		{
			m_playerStatus.GetComponent<Status_Player>().AddExp(5);
			m_hp = 1;
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
