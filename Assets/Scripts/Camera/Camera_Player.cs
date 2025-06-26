using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera_Player : MonoBehaviour
{
	[SerializeField] GameObject m_player;
	[SerializeField] GameObject m_pivot;
	[SerializeField, Range(0f, 1.0f)] float m_camRotSpeedX = 0.1f;    // カメラのx軸回転のスピード
	[SerializeField, Range(1.0f, 5.0f)] float m_camRange = 1;		// カメラとプレイヤーの距離
	[SerializeField, Range(1.0f, 5.0f)] float m_camSpecialRange = 1;		// カメラとプレイヤーの距離（必殺技用）
	private float m_maxCamHeight;   // カメラの高さの最大値
	[SerializeField] float m_minCamHeight = 0.2f;   // カメラの高さの最小値
	[SerializeField] float m_camDiffZ = -4.0f;  // ゲーム開始時のプレイヤーのZ座標とカメラのZ座標の差分
	[SerializeField] float m_camDiffY = 2.2f;   // ゲーム開始時のプレイヤーのY座標とカメラのY座標の差分

	// カメラの位置
	private Vector3 m_currentPos;
	private Vector3 m_pastPos;
	private Vector3 m_camDir;   // カメラからpivotへの方向ベクトル
	[SerializeField] float m_camDist = 4.0f;    // カメラとpivotの距離
	private Vector3 m_diff; // 移動距離
	private float m_camCurrentRange;    // カメラとプレイヤーの現在の距離
	[SerializeField] float m_camChangeTime = 0.5f;  // カメラの距離を変えるときにかかる時間
	private float m_durationTime;   // 時間カウント用
	private bool m_isSpecial;	// カメラの距離を必殺技用の距離に変更する
	private bool m_isNormal;	// カメラの距離を通常の距離に変更する

	private bool m_canMove; // カメラが動けるか

	// カメラをポーズ画面中動かないようにする
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
		// 最初のプレイヤーの位置を取得
		m_pastPos = m_player.transform.position;

		m_maxCamHeight = m_camDiffZ * -1;

		// カメラの開始時の座標を設定
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

		// プレイヤーの現在地を取得
		m_currentPos = m_player.transform.position;

		m_diff = m_currentPos - m_pastPos;
		// カメラをプレイヤーの移動差分だけ動かす
		//transform.position = Vector3.Lerp(transform.position, transform.position + m_diff, 1.0f);
		transform.position += m_diff;
		m_pastPos = m_currentPos;

		// カメラを横に回転させる
		float mouseX = Input.GetAxisRaw("Mouse X");

		// X方向に一定以上移動していれば横に回転
		if (Mathf.Abs(mouseX) > 0.01f)
		{
			// ワールド座標のy軸基準で回転
			transform.RotateAround(m_player.transform.position, Vector3.up, mouseX);
			m_pivot.transform.Rotate(new Vector3(0, mouseX, 0));
		}

		// カメラの高さを変えて縦回転を実装
		Vector3 pos = new Vector3(transform.position.x, transform.position.y + Input.GetAxisRaw("Mouse Y") * m_camRotSpeedX * -1, transform.position.z);

		// カメラの高さを制限する
		if (pos.y >= m_currentPos.y + m_maxCamHeight) pos = new Vector3(pos.x, m_currentPos.y + m_maxCamHeight, pos.z);
		if (pos.y <= m_currentPos.y + m_minCamHeight) pos = new Vector3(pos.x, m_currentPos.y + m_minCamHeight, pos.z);

		m_camDir = Vector3.Normalize(m_pivot.transform.position - pos);

		// カメラの距離を変更する
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

		// カメラの注視点をpivotにする
		transform.LookAt(m_pivot.transform);
	}

	public void TurnMoveFlg()
	{
		// 動けるかどうかのフラグを反転させる
		m_canMove = !m_canMove;
	}

	// 必殺技用にカメラの距離を取り始める
	public void SetCamRangeForSpecial()
	{
		m_isSpecial = true;
	}

	// 必殺技発動時にカメラを遠くする
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

		// 距離を必殺技用にする
		m_camCurrentRange = Mathf.Lerp(m_camCurrentRange, m_camSpecialRange, t);
	}

	// 通常のカメラ距離に戻し始める
	public void SetCamRangeForNormal()
	{
		m_isNormal = true;
	}

	// 必殺技終了時に元の距離に戻す
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

		// 距離を通常に戻す
		m_camCurrentRange = Mathf.Lerp(m_camCurrentRange, m_camRange, t);
	}
}
