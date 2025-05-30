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

	// キーの入力を取得
	private float m_inputX;
	private float m_inputZ;
	private Vector3 m_moveDirection;
	private Vector3 m_moveVelocity;
	private bool m_isRun;

	// カメラ
	[SerializeField] Camera m_cam;
	private Vector3 m_camForward;
	private Vector3 m_camRight;

	// プレイヤーのモデル
	[SerializeField] GameObject m_model;

	private bool m_stopMove;    // 移動できるか取得する用

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
		// フラグの更新
		m_stopMove = m_model.GetComponent<Animation_Player>().GetMoveFlg();
		// 本来の移動
		NormalMove();
	}

	// 移動
	private void NormalMove()
	{
		if(!m_stopMove)
		{
			// キーの入力
			m_inputX = Input.GetAxis("Horizontal");
			m_inputZ = Input.GetAxis("Vertical");

			// カメラの向き
			m_camForward = m_cam.transform.forward;
			m_camRight = m_cam.transform.right;

			// カメラの正面方向のy軸を調整
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
			// キャラクターコントローラーを使った移動
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

			// プレイヤーの回転
			transform.rotation = Quaternion.LookRotation(m_moveDirection);
		}
		else
		{
			m_isRun = false;
		}

		// 重力
		if (!m_charaCon.isGrounded)
		{
			// 着地していないとき
			m_moveVelocity.y += Physics.gravity.y * Time.deltaTime;
		}
		else
		{
			m_moveVelocity.y = 0;
		}
		m_charaCon.Move(m_moveVelocity * Time.deltaTime);
	}

	// 移動を止めるときに使う
	private void SetZero()
	{
		// キーの入力
		m_inputX = 0;
		m_inputZ = 0;

		// カメラの向き
		m_camForward = Vector3.zero;
		m_camRight = Vector3.zero;
	}

	// 位置合わせ
	public void SetPos(Vector3 modelPos)
	{
		// characterControllerを無効にしないと動かない?
		m_charaCon.enabled = false;
		// カメラをいったん動かないようにする（ガクつき防止用）
		m_cam.GetComponent<Camera_Player>().TurnMoveFlg();
		transform.position = modelPos;
		m_cam.GetComponent<Camera_Player>().TurnMoveFlg();
		m_charaCon.enabled = true;
		// modelのローカルポジションを0にする
		m_model.transform.localPosition = Vector3.zero;
	}

	// 走っているかどうか
	public bool IsRun()
	{
		return m_isRun;
	}

	// プレイヤーの向きをカメラの正面方向に合わせる
	public void SetCamFront()
	{
		// プレイヤーの回転
		transform.rotation = Quaternion.LookRotation(m_cam.transform.forward);
	}
}
