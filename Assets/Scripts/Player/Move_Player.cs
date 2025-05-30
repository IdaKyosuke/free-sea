using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using UnityEngine;

public class Move_Player : MonoBehaviour
{
	[SerializeField] float m_walkSpeed = 4.0f;
	[SerializeField] float m_runSpeed = 4.0f;
	private CharacterController m_charaCon;

	// �L�[�̓��͂��擾
	private float m_inputX;
	private float m_inputZ;
	private Vector3 m_moveDirection;
	private Vector3 m_moveVelocity;
	private bool m_isRun;

	// �J����
	[SerializeField] Camera m_cam;
	private Vector3 m_camForward;
	private Vector3 m_camRight;

	// �v���C���[�̃��f��
	[SerializeField] GameObject m_model;

	private bool m_stopMove;    // �ړ��ł��邩�擾����p

	// Start is called before the first frame update
	void Start()
    {
        m_charaCon = GetComponent<CharacterController>();
		m_inputX = 0;
		m_inputZ = 0;
		m_moveDirection = Vector3.zero;
		m_moveVelocity = Vector3.zero;
		m_isRun = false;
		m_camForward = Vector3.zero;
		m_camRight = Vector3.zero;
		m_stopMove = m_model.GetComponent<Animation_Player>().GetMoveFlg();
	}

    // Update is called once per frame
    void Update()
    {
		// �t���O�̍X�V
		m_stopMove = m_model.GetComponent<Animation_Player>().GetMoveFlg();
		// �{���̈ړ�
		NormalMove();
	}

	// �ړ�
	private void NormalMove()
	{
		if(!m_stopMove)
		{
			// �L�[�̓���
			m_inputX = Input.GetAxis("Horizontal");
			m_inputZ = Input.GetAxis("Vertical");

			// �J�����̌���
			m_camForward = m_cam.transform.forward;
			m_camRight = m_cam.transform.right;

			// �J�����̐��ʕ�����y���𒲐�
			m_camForward.y = 0;
			m_camRight.y = 0;
		}
		else
		{
			SetZero();
		}

		m_moveDirection = m_camForward * m_inputZ + m_camRight * m_inputX;

		if (m_moveDirection != Vector3.zero)
		{
			// �L�����N�^�[�R���g���[���[���g�����ړ�
			m_charaCon.Move(m_camForward * (Input.GetKey("left shift") ? m_runSpeed : m_walkSpeed) * m_inputZ * Time.deltaTime);
			m_charaCon.Move(m_camRight * (Input.GetKey("left shift") ? m_runSpeed : m_walkSpeed) * m_inputX * Time.deltaTime);
		
			if(Input.GetKey("left shift"))
			{
				m_isRun = true;
			}
			else
			{
				m_isRun = false;
			}

			// �v���C���[�̉�]
			transform.rotation = Quaternion.LookRotation(m_moveDirection);
		}
		else
		{
			m_isRun = false;
		}

		// �d��
		if (!m_charaCon.isGrounded)
		{
			// ���n���Ă��Ȃ��Ƃ�
			m_moveVelocity.y += Physics.gravity.y * Time.deltaTime;
		}
		else
		{
			m_moveVelocity.y = 0;
		}
		m_charaCon.Move(m_moveVelocity * Time.deltaTime);
	}

	// �ړ����~�߂�Ƃ��Ɏg��
	private void SetZero()
	{
		// �L�[�̓���
		m_inputX = 0;
		m_inputZ = 0;

		// �J�����̌���
		m_camForward = Vector3.zero;
		m_camRight = Vector3.zero;
	}

	// �ʒu���킹
	public void SetPos(Vector3 modelPos)
	{
		// characterController�𖳌��ɂ��Ȃ��Ɠ����Ȃ�?
		m_charaCon.enabled = false;
		// �J�������������񓮂��Ȃ��悤�ɂ���i�K�N���h�~�p�j
		m_cam.GetComponent<Camera_Player>().TurnMoveFlg();
		transform.position = modelPos;
		m_cam.GetComponent<Camera_Player>().TurnMoveFlg();
		m_charaCon.enabled = true;
		// model�̃��[�J���|�W�V������0�ɂ���
		m_model.transform.localPosition = Vector3.zero;
	}

	// �����Ă��邩�ǂ���
	public bool IsRun()
	{
		return m_isRun;
	}

	// �v���C���[�̌������J�����̐��ʕ����ɍ��킹��
	public void SetCamFront()
	{
		// �v���C���[�̉�]
		transform.rotation = Quaternion.LookRotation(m_cam.transform.forward);
	}
}
