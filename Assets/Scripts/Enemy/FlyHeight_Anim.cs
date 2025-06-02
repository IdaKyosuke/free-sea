using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyHeight_Anim : MonoBehaviour
{
	[SerializeField] float m_changeAmount = 0.5f;   // �����̕ω���
	[SerializeField] float m_moveTime = 0.4f;	// 1�����ɍ�����ω������鎞��
	private bool m_isUp;    // �������オ�邩�����邩
	private float m_durationTime;

	private float height;

    // Start is called before the first frame update
    void Start()
    {
        m_isUp = true;
		m_durationTime = 0;
		height = transform.position.y;
	}

    // Update is called once per frame
    void Update()
    {
		// �㉺�ɗh�炷
		m_durationTime += Time.deltaTime;
		float t = m_durationTime / m_moveTime;

		if(m_durationTime >= m_moveTime)
		{
			t = 1.0f;
		}
		// ������ς���
		float y = Mathf.Lerp(height, height + m_changeAmount * (m_isUp ? 1 : -1), t);

		transform.position = new Vector3(transform.position.x, y, transform.position.z);

		if(t == 1.0f)
		{
			m_durationTime = 0;
			m_isUp = !m_isUp;
			height = transform.position.y;
		}
	}
}
