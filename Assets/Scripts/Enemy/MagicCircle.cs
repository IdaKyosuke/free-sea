using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
	[SerializeField] GameObject m_magic;    // 攻撃で飛ばすオブジェクト
	[SerializeField] float m_amount;    // 1度の攻撃で飛ばす量
	[SerializeField] float m_diray;		// オブジェクトを飛ばす間隔
	[SerializeField] float m_waitTime;  // 最初の１発を飛ばすまでの待ち時間
	[SerializeField] float m_fadeTime;	// フェードにかかる時間
	private float m_duration;
	private bool m_isFinish;    // 魔法を撃ち終えたか

	private ParticleSystem m_system;

    // Start is called before the first frame update
    void Start()
    {
		m_duration = 0;
		m_isFinish = false;
		m_system = GetComponent<ParticleSystem>();
	}

    // Update is called once per frame
    void Update()
    {
        m_duration += Time.deltaTime;
		// 魔法が何発発射されたか
		int count = 0;

		// 魔法陣は常にプレイヤーを向くようにする
		Vector3 dir = GameObject.FindWithTag("Player").transform.position - transform.position;
		dir.y = 90;

		transform.rotation = Quaternion.Slerp(
			transform.rotation, 
			Quaternion.LookRotation(dir, Vector3.up),
			0.2f
			);

		if(m_duration >= m_waitTime)
		{
			m_duration = 0;
			Instantiate(m_magic, this.transform.position, Quaternion.identity);
			count++;
			do
			{
				m_duration += Time.deltaTime;
				if(m_duration >= m_diray)
				{
					m_duration = 0;
					Instantiate(m_magic, transform.position, transform.rotation);
					count++;
				}

			} while (count < m_amount);

			m_isFinish = true;
		}

		if(m_isFinish)
		{
			m_duration += Time.deltaTime;
			if(m_duration >= 3.0f)
			{
				Destroy(this.gameObject);
			}
		}
		/*
		// 魔法を撃ち終えたら
		if(m_isFinish)
		{
			m_duration += Time.deltaTime;
			float t = m_duration / m_fadeTime;
			if(t >= 1.0f)
			{
				t = 1.0f;
			}

			float alpha = Mathf.Lerp(255, 0, t);
			m_system.startColor = 
		}
		*/
    }
}
