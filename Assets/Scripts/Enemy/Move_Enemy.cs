using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Enemy : MonoBehaviour
{
	[SerializeField] GameObject m_comboManager; // コンボ加算用
	[SerializeField] GameObject m_hitEffect;	// ヒットエフェクト

	// Start is called before the first frame update
	void Start()
	{
		if (!m_comboManager)
		{
			m_comboManager = GameObject.FindWithTag("comboManager");
		}
	}

	// Update is called once per frame
	void Update()
	{

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
		}
	}
}
