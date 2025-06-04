using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyHeight_Anim : MonoBehaviour
{
	[SerializeField] float m_changeAmount = 0.5f;   // çÇÇ≥ÇÃïœâªó 
	[SerializeField] float m_moveTime = 0.4f;	// 1ï˚å¸Ç…çÇÇ≥ÇïœâªÇ≥ÇπÇÈéûä‘
	private bool m_isUp;    // çÇÇ≥Ç™è„Ç™ÇÈÇ©â∫Ç™ÇÈÇ©
	private float m_durationTime;

	private float height;

	private bool m_isDeath;

    // Start is called before the first frame update
    void Start()
    {
        m_isUp = true;
		m_durationTime = 0;
		height = transform.position.y;
		m_isDeath = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (m_isDeath) return;

		// è„â∫Ç…óhÇÁÇ∑
		m_durationTime += Time.deltaTime;
		float t = m_durationTime / m_moveTime;

		if(m_durationTime >= m_moveTime)
		{
			t = 1.0f;
		}
		// çÇÇ≥ÇïœÇ¶ÇÈ
		float y = Mathf.Lerp(height, height + m_changeAmount * (m_isUp ? 1 : -1), t);

		transform.position = new Vector3(transform.position.x, y, transform.position.z);

		if(t == 1.0f)
		{
			m_durationTime = 0;
			m_isUp = !m_isUp;
			height = transform.position.y;
		}
	}

	public void IsDeath()
	{
		m_isDeath = true;
	}
}
