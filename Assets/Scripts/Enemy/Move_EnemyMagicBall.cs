using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_EnemyMagicBall : MonoBehaviour
{
	[SerializeField] GameObject m_hitEffect;
	[SerializeField] float m_speed;
	[SerializeField] float m_time;  // 消えるまでの時間 

	private Rigidbody m_rb;
	private Vector3 m_direction;
	private float m_durationTime;   // 経過時間

	// Start is called before the first frame update
	void Start()
    {
		m_rb = GetComponent<Rigidbody>();
		m_durationTime = 0;
		Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
		m_direction = (playerPos - transform.position).normalized;
	}

    // Update is called once per frame
    void Update()
    {
		m_rb.velocity = m_direction * m_speed;

		m_durationTime += Time.deltaTime;
		// 一定時間経過で破壊
		if (m_durationTime >= m_time)
		{
			Destroy(this.gameObject);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		// 衝突位置の取得
		Vector3 hitPos = other.contacts[0].point;
		// 衝突位置にエフェクトを表示
		Instantiate(m_hitEffect, hitPos, Quaternion.identity);
		// 自分を破壊
		Destroy(this.gameObject);
	}
}
