using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Player : MonoBehaviour
{
	[SerializeField] GameObject m_player;
	[SerializeField] GameObject m_pivot;
	[SerializeField, Range(0f, 1.0f)] float m_camRotSpeedX = 0.1f;    // �J������x����]�̃X�s�[�h
	[SerializeField, Range(1.0f, 5.0f)] float m_camRange = 1;		// �J�����ƃv���C���[�̋���
	private float m_maxCamHeight;   // �J�����̍����̍ő�l
	[SerializeField] float m_minCamHeight = 0.2f;   // �J�����̍����̍ŏ��l
	[SerializeField] float m_camDiffZ = -4.0f;  // �Q�[���J�n���̃v���C���[��Z���W�ƃJ������Z���W�̍���
	[SerializeField] float m_camDiffY = 2.2f;   // �Q�[���J�n���̃v���C���[��Y���W�ƃJ������Y���W�̍���

	// �J�����̈ʒu
	private Vector3 m_currentPos;
	private Vector3 m_pastPos;
	private Vector3 m_camDir;   // �J��������pivot�ւ̕����x�N�g��
	[SerializeField] float m_camDist = 4.0f;    // �J������pivot�̋���
	private Vector3 m_diff; // �ړ�����

	private bool m_canMove; // �J�����������邩

	// �J�������|�[�Y��ʒ������Ȃ��悤�ɂ���
	[SerializeField] GameObject m_pauseManager;

	// Start is called before the first frame update
	void Start()
	{
		if (!m_player)
		{
			m_player = GameObject.FindWithTag("Player");
		}
		// �ŏ��̃v���C���[�̈ʒu���擾
		m_pastPos = m_player.transform.position;

		m_maxCamHeight = m_camDiffZ * -1;

		// �J�����̊J�n���̍��W��ݒ�
		Vector3 playerPos = m_player.transform.position;
		transform.position = new Vector3(playerPos.x, playerPos.y + m_camDiffY, playerPos.z + m_camDiffZ);

		m_camDir = Vector3.Normalize(m_pivot.transform.position - transform.position);

		m_canMove = true;
	}

	// Update is called once per frame
	void Update()
	{
		if(m_pauseManager.GetComponent<PauseSceneManager>().IsPause())
		{
			return;
		}

		// �v���C���[�̌��ݒn���擾
		m_currentPos = m_player.transform.position;

		m_diff = m_currentPos - m_pastPos;
		// �J�������v���C���[�̈ړ���������������
		transform.position = Vector3.Lerp(transform.position, transform.position + m_diff, 1.0f);
		m_pastPos = m_currentPos;

		// �J���������ɉ�]������
		float mouseX = Input.GetAxisRaw("Mouse X");

		// X�����Ɉ��ȏ�ړ����Ă���Ή��ɉ�]
		if (Mathf.Abs(mouseX) > 0.01f)
		{
			// ���[���h���W��y����ŉ�]
			transform.RotateAround(m_player.transform.position, Vector3.up, mouseX);
			m_pivot.transform.Rotate(new Vector3(0, mouseX, 0));
		}

		// �J�����̍�����ς��ďc��]������
		Vector3 pos = new Vector3(transform.position.x, transform.position.y + Input.GetAxisRaw("Mouse Y") * m_camRotSpeedX * -1, transform.position.z);

		// �J�����̍����𐧌�����
		if (pos.y >= m_maxCamHeight) pos = new Vector3(pos.x, m_maxCamHeight, pos.z);
		if (pos.y <= m_minCamHeight) pos = new Vector3(pos.x, m_minCamHeight, pos.z);

		m_camDir = Vector3.Normalize(m_pivot.transform.position - pos);

		if (m_canMove)
		{
			transform.position = m_pivot.transform.position - (m_camDir * m_camDist * m_camRange);
		}

		// �J�����̒����_��pivot�ɂ���
		transform.LookAt(m_pivot.transform);
	}

	public void TurnMoveFlg()
	{
		// �����邩�ǂ����̃t���O�𔽓]������
		m_canMove = !m_canMove;
	}
}
