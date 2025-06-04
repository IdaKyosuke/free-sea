using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;

public class Animation_Enemy_Angel : MonoBehaviour
{
	[SerializeField] GameObject m_wingAnim;	// 羽のアニメーション
	private Animator m_anim;
	private float m_duration;
	private bool m_canMove;
	private bool m_isAttacked;

	private bool m_finishDeathAnim;
	private bool m_isGetHit;
	private bool m_isDeath;

    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
		m_duration = 0;
		m_canMove = true;
		m_isAttacked = false;
		m_finishDeathAnim = false;
		m_isGetHit = false;
		m_isDeath = false;

		// AnimatorからObservableStateMachineTriggerの参照を取得
		ObservableStateMachineTrigger trigger =
			m_anim.GetBehaviour<ObservableStateMachineTrigger>();

		// Stateの開始イベント
		IDisposable enterState = trigger
			.OnStateEnterAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;
				if(info.IsName("Base Layer.attack"))
				{
					// 攻撃中は移動できないようにする
					m_canMove = false;
				}

				// 死亡アニメーション開始時
				if(info.IsName("Base Layer.Death"))
				{
					m_wingAnim.GetComponent<Animation_AngelWing>().IsDeath();
					GetComponent<FlyHeight_Anim>().IsDeath();
				}
				
			}).AddTo(this);

		// Stateの終了イベント
		IDisposable exitState = trigger
			.OnStateExitAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;
				if (info.IsName("Base Layer.attack"))
				{
					// 攻撃終了後に動けるようにする
					m_canMove = true;
				}

				// ヒットアニメーション
				if(info.IsName("Base Layer.GetHit"))
				{
					m_isGetHit = false;
					m_canMove = !m_isGetHit;
				}

			}).AddTo(this);
	}

    // Update is called once per frame
    void Update()
    {
		if (m_isDeath) return;

		if(!m_isGetHit && m_isAttacked)
		{
			m_duration += Time.deltaTime;

			if (m_duration >= 3.0f)
			{
				m_duration = 0;
				m_isAttacked = false;
			}
		}
    }

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			m_canMove = false;

			// 死亡している || 攻撃を受けている時は何もしない
			if (m_isDeath || m_isGetHit) return;

			// 攻撃を受けていない && 攻撃のリキャスト時間でない
			if(!m_isAttacked)
			{
				m_anim.SetTrigger("attack");
				m_isAttacked = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			m_canMove = true;
		}
	}

	// 移動できるか取得
	public bool CanMove()
	{
		return m_canMove;
	}

	// 死亡アニメーションを実行する && 死亡したフラグを立てる
	public void IsDeath()
	{
		m_anim.SetTrigger("death");
		m_isDeath = true;
	}

	// ダメージを受けるアニメーションを実行する
	public void GetHit()
	{
		m_anim.SetTrigger("getHit");
		m_isGetHit = true;
		m_canMove = !m_isGetHit;
	}

	// 死亡アニメーションの終了フラグを立てる
	public void SetFinishAnimFlg()
	{
		m_finishDeathAnim = true;
	}

	// 死亡アニメーションの終了を取得
	public bool FinishDeathAnim()
	{
		return m_finishDeathAnim;
	}
}
