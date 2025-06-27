using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
	[SerializeField] float m_speed; // 移動するエフェクトの速度
	[SerializeField] float m_time;  // 消えるまでの時間
	private Rigidbody m_rb;
	private float m_duration;   // 経過時間計測用

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
