using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
	[SerializeField] float m_speed; // �ړ�����G�t�F�N�g�̑��x
	[SerializeField] float m_time;  // ������܂ł̎���
	private Rigidbody m_rb;
	private float m_duration;   // �o�ߎ��Ԍv���p

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
		m_duration = 0;
		Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
		this.transform.LookAt(playerPos);
	}

    // Update is called once per frame
    void Update()
    {
        m_rb.velocity = this.transform.forward * m_speed;
		m_duration += Time.deltaTime;
		if(m_duration >= m_time)
		{
			Destroy(this.gameObject);
		}
    }
}
