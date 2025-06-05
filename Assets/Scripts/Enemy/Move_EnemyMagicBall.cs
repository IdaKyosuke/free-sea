using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_EnemyMagicBall : MonoBehaviour
{
	[SerializeField] GameObject m_hitEffect;
	[SerializeField] float m_speed;
	[SerializeField] float m_time;  // ������܂ł̎��� 

	private Rigidbody m_rb;
	private Vector3 m_direction;
	private float m_durationTime;   // �o�ߎ���

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
		// ��莞�Ԍo�߂Ŕj��
		if (m_durationTime >= m_time)
		{
			Destroy(this.gameObject);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		// �Փˈʒu�̎擾
		Vector3 hitPos = other.contacts[0].point;
		// �Փˈʒu�ɃG�t�F�N�g��\��
		Instantiate(m_hitEffect, hitPos, Quaternion.identity);
		// ������j��
		Destroy(this.gameObject);
	}
}
