using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera_Player : MonoBehaviour
{
	[SerializeField] GameObject m_player;
	[SerializeField] GameObject m_pivot;
	[SerializeField, Range(0f, 1.0f)] float m_camRotSpeedX = 0.1f;    // �J������x����]�̃X�s�[�h
	[SerializeField, Range(1.0f, 5.0f)] float m_camRange = 1;		// �J�����ƃv���C���[�̋���
	[SerializeField, Range(1.0f, 5.0f)] float m_camSpecialRange = 1;		// �J�����ƃv���C���[�̋����i�K�E�Z�p�j
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
	private float m_camCurrentRange;    // �J�����ƃv���C���[�̌��݂̋���
	[SerializeField] float m_camChangeTime = 0.5f;  // �J�����̋�����ς���Ƃ��ɂ����鎞��
	private float m_durationTime;   // ���ԃJ�E���g�p
	private bool m_isSpecial;	// �J�����̋�����K�E�Z�p�̋����ɕύX����
	private bool m_isNormal;	// �J�����̋�����ʏ�̋����ɕύX����

	private bool m_canMove; // �J�����������邩

	// �J�������|�[�Y��ʒ������Ȃ��悤�ɂ���
	[SerializeField] GameObject m_pauseManager;

	// Start is called before the first frame update
	void Start()
	{
		m_camCurrentRange = m_camRange;
		m_durationTime = 0;
		m_isSpecial = false;
		m_isNormal = false;
		if (!m_player)
		{
			m_player = GameObject.FindWithTag("Player");
		}
		// �ŏ��̃v���C���[�̈ʒu���擾
		m_pastPos = m_player.transform.position;

		m_maxCamHeight = m_camDiffZ * -1;

		// �J�����̊J�n���̍��W��ݒ�
		Vector3 playerPos = m_player.transform.position;
		Vector3 dir = playerPos - transform.position;
		transform.position = playerPos - dir * m_camDist;

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
		//transform.position = Vector3.Lerp(transform.position, transform.position + m_diff, 1.0f);
		transform.position += m_diff;
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
		if (pos.y >= m_currentPos.y + m_maxCamHeight) pos = new Vector3(pos.x, m_currentPos.y + m_maxCamHeight, pos.z);
		if (pos.y <= m_currentPos.y + m_minCamHeight) pos = new Vector3(pos.x, m_currentPos.y + m_minCamHeight, pos.z);

		m_camDir = Vector3.Normalize(m_pivot.transform.position - pos);

		// �J�����̋�����ύX����
		if(m_isSpecial)
		{
			SetSpecialCam();
		}
		else if(m_isNormal)
		{
			ResetCam();
		}

		if (m_canMove)
		{
			transform.position = m_pivot.transform.position - (m_camDir * m_camDist * m_camCurrentRange);
		}

		// �J�����̒����_��pivot�ɂ���
		transform.LookAt(m_pivot.transform);
	}

	public void TurnMoveFlg()
	{
		// �����邩�ǂ����̃t���O�𔽓]������
		m_canMove = !m_canMove;
	}

	// �K�E�Z�p�ɃJ�����̋��������n�߂�
	public void SetCamRangeForSpecial()
	{
		m_isSpecial = true;
	}

	// �K�E�Z�������ɃJ��������������
	private void SetSpecialCam()
	{
		m_durationTime += Time.deltaTime;
		float t = m_durationTime / m_camChangeTime;
		if(t >= 1.0f)
		{
			t = 1.0f;
			m_isSpecial = false;
			m_durationTime = 0.0f;
		}

		// ������K�E�Z�p�ɂ���
		m_camCurrentRange = Mathf.Lerp(m_camCurrentRange, m_camSpecialRange, t);
	}

	// �ʏ�̃J���������ɖ߂��n�߂�
	public void SetCamRangeForNormal()
	{
		m_isNormal = true;
	}

	// �K�E�Z�I�����Ɍ��̋����ɖ߂�
	private void ResetCam()
	{
		m_durationTime += Time.deltaTime;
		float t = m_durationTime / m_camChangeTime;
		if (t >= 1.0f)
		{
			t = 1.0f;
			m_isNormal = false;
			m_durationTime = 0.0f;
		}

		// ������ʏ�ɖ߂�
		m_camCurrentRange = Mathf.Lerp(m_camCurrentRange, m_camRange, t);
	}
}
