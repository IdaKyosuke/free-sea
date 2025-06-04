using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_MagicSphere : MonoBehaviour
{
	[SerializeField] GameObject m_hitEffect;
	[SerializeField] float m_speed;
	[SerializeField] float m_time;	// ������܂ł̎��� 

	private Rigidbody m_rb;
	private Vector3 m_direction;
	private float m_durationTime;	// �o�ߎ���

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
		// �v���C���[�̐��ʂ��擾
		GameObject playerCam = GameObject.FindWithTag("playerCam");
		m_direction = playerCam.transform.forward.normalized;
		m_durationTime = 0;
		Debug.Log(m_direction);
	}

    // Update is called once per frame
    void Update()
    {
        m_rb.velocity = m_direction * m_speed;

		m_durationTime += Time.deltaTime;
		// ��莞�Ԍo�߂Ŕj��
		if(m_durationTime >= m_time)
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
