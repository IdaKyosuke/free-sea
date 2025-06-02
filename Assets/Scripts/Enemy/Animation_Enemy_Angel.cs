using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;

public class Animation_Enemy_Angel : MonoBehaviour
{
	private Animator m_anim;
	private float m_duration;
	private bool m_canMove;
	private bool m_isAttacked;

    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
		m_duration = 0;
		m_canMove = true;
		m_isAttacked = false;

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

			}).AddTo(this);
	}

    // Update is called once per frame
    void Update()
    {
		if(m_isAttacked)
		{
			m_duration += Time.deltaTime;

			if (m_duration >= 3.0f)
			{
				m_duration = 0;
				m_isAttacked = false;
			}
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		/*
		if(!m_isAttacked && other.gameObject.CompareTag("Player"))
		{
			m_isAttacked = true;
			m_anim.SetTrigger("attack");
		}
		*/
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			m_canMove = false;
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

	public bool CanMove()
	{
		return m_canMove;
	}
}
