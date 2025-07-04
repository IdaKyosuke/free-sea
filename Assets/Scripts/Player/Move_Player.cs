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
	private bool m_isRun;
	private Vector3 m_moveVelocity;

	// カメラ
	[SerializeField] Camera m_cam;
	private Vector3 m_camForward;
	private Vector3 m_camRight;

	// プレイヤーのモデル
	[SerializeField] GameObject m_model;
	// ステータスを管理するオブジェクト
	[SerializeField] GameObject m_status;

	private bool m_stopMove;    // 移動できるか取得する用
	private bool m_isPlayDeathAnim; // 死亡アニメーションを1回しか呼ばない用

	// レベルアップ時のエフェクト
	[SerializeField] GameObject m_effect;

	[SerializeField] GameObject m_effectHeal;       // 回復エフェクト
	[SerializeField] AudioSource m_seHeal;		// 回復音

	// Start is called before the first frame update
	void Start()
    {
		m_charaCon = GetComponent<CharacterController>();
		m_inputX = 0;
		m_inputZ = 0;
		m_moveDirection = Vector3.zero;
		m_isRun = false;
		m_camForward = Vector3.zero;
		m_camRight = Vector3.zero;
		m_stopMove = m_model.GetComponent<Animation_Player>().GetMoveFlg();
		m_isPlayDeathAnim = false;
		m_moveVelocity = Vector3.zero ;
		if (!m_status)
		{
			m_status = GameObject.FindWithTag("playerStatus");
		}
		if(!m_cam)
		{
			m_cam = GameObject.FindWithTag("playerCam").GetComponent<Camera>();
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (m_status.GetComponent<Status_Player>().IsDeath())
		{
			// 死亡したら
			if(!m_isPlayDeathAnim)
			{
				m_model.GetComponent<Animation_Player>().DeathAnim();
				m_isPlayDeathAnim = true;
			}
			return;
		}

		// 無敵時間の管理
		m_status.GetComponent<Status_Player>().SetInvincible(m_model.GetComponent<Animation_Player>().IsInvincible());

		// フラグの更新
		m_stopMove = m_model.GetComponent<Animation_Player>().GetMoveFlg();
		// 本来の移動
		NormalMove();

		// 奈落死
		if(transform.position.y <= -100)
		{
			m_status.GetComponent<Status_Player>().Death();
		}
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
			m_camForward = Vector3.Scale(m_cam.transform.forward, new Vector3(1, 0, 1)).normalized;
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
			if (Input.GetKey("left shift"))
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

		// 移動速度の上昇補正値を上げすぎないようにする
		float spdBuff = m_status.GetComponent<Status_Player>().GetStatusValue(Status.StatusType.Spd);
		switch(spdBuff)
		{
			case <= 4:
				spdBuff *= 0.9f;
				break;

			case <= 8:
				spdBuff *= 0.7f;
				break;

			default:
				spdBuff *= 0.6f;
				break;
		}

		// キャラクターコントローラーを使った移動
		m_charaCon.Move(m_camForward * (Input.GetKey("left shift") ? m_runSpeed * spdBuff : m_walkSpeed) * m_inputZ * Time.deltaTime);
		m_charaCon.Move(m_camRight * (Input.GetKey("left shift") ? m_runSpeed * spdBuff : m_walkSpeed) * m_inputX * Time.deltaTime);

		// 重力
		if (!m_charaCon.isGrounded)
		{
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
		// カメラをいったん動かないようにする（ガクつき防止用）
		m_cam.GetComponent<Camera_Player>().TurnMoveFlg();
		transform.position = modelPos;
		m_cam.GetComponent<Camera_Player>().TurnMoveFlg();
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
		transform.rotation = Quaternion.Euler(new Vector3(m_cam.transform.rotation.x, 0, m_cam.transform.rotation.z));
	}

	// プレイヤーの移動方向を取得
	public Vector3 GetMoveDir()
	{
		return m_moveDirection;
	}

	// レベルアップ時のエフェクトを出す
	public void ActiveLevelUpEffect()
	{
		Instantiate(m_effect, this.transform.position, Quaternion.identity);
		Debug.Log("levelup!");
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("item_heal"))
		{
			Instantiate(m_effectHeal, transform.position, Quaternion.identity);
			m_seHeal.Play();
		}
	}
}
