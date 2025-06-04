using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Animation_Player : MonoBehaviour
{
	[SerializeField] GameObject m_player;
	[SerializeField] BoxCollider m_weaponCol;
	[SerializeField] GameObject m_trail;	// TrailRendererが入ったオブジェクト
	// ポーズ画面管理用オブジェクト
	[SerializeField] GameObject m_pauseManager;

	private Animator m_anim;

	// 移動アニメーション用
	private Vector3 m_pastPos;

	// 移動制限用
	private bool m_stopMove;

	// 攻撃アニメーションが動いているか
	const int AttackAnimNum = 3;
	private bool[] m_attackAnimFlg = new bool[AttackAnimNum];

	// 魔法攻撃用のオブジェクト
	[SerializeField] GameObject m_magic;
	[SerializeField] GameObject m_magicPoint;

	// Start is called before the first frame update
	void Start()
    {
        m_anim = GetComponent<Animator>();
		m_pastPos = transform.position;
		m_stopMove = false;
		m_weaponCol.enabled = false;

		// TrailRendererを無効にする
		m_trail.GetComponent<TrailRenderer>().emitting = false;

		for (int i = 0; i < AttackAnimNum; i++)
		{
			m_attackAnimFlg[i] = false;
		}

		// AnimatorからObservableStateMachineTriggerの参照を取得
		ObservableStateMachineTrigger trigger =
			m_anim.GetBehaviour<ObservableStateMachineTrigger>();

		// Stateの開始イベント
		IDisposable enterState = trigger
			.OnStateEnterAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;

				// 連撃用フラグ管理
				if (info.IsName("Base Layer.Attack1"))
				{
					m_attackAnimFlg [0] = true;
				}
				if (info.IsName("Base Layer.Attack2"))
				{
					m_attackAnimFlg[1] = true;
				}
				if (info.IsName("Base Layer.Attack3"))
				{
					m_attackAnimFlg[2] = true;
				}

				// 魔法アニメーション
				if(info.IsName("Base Layer.Spell_Magic"))
				{
					m_player.GetComponent<Move_Player>().SetCamFront();
				}
			}).AddTo(this);

		// Stateの終了イベント
		IDisposable exitState = trigger
			.OnStateExitAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;
				if (info.IsName("Base Layer.Attack1"))
				{
					m_attackAnimFlg[0] = false;
					if (!m_attackAnimFlg[1])
					{
						CanMove();
						InactiveCol();
					}
				}
				if (info.IsName("Base Layer.Attack2"))
				{
					if (!m_attackAnimFlg[2])
					{
						CanMove();
						InactiveCol();
					}
					m_attackAnimFlg[1] = false;
				}
				if (info.IsName("Base Layer.Attack3"))
				{
					// アニメーションの動きに合わせて位置合わせ
					SetModelPos();

					if (!m_attackAnimFlg[0])
					{
						CanMove();
						InactiveCol();
					}
					m_attackAnimFlg[2] = false;
				}

				// 位置合わせ
				if (info.IsName("Base Layer.Rolling"))
				{
					SetModelPos();
					CanMove();
				}

				if(info.IsName("Base Layer.Attack_Spell"))
				{
					CanMove();
				}

			}).AddTo(this);
	}

    // Update is called once per frame
    void Update()
    {
		if(m_pauseManager.GetComponent<PauseSceneManager>().IsPause())
		{
			return;
		}

		Move();

		AttackAnim();
	}

	// 移動アニメーション
	private void Move()
	{
		// 走っているかどうか
		if(Input.GetKey("left shift"))
		{
			m_anim.SetBool("run", true);
		}
		else
		{
			m_anim.SetBool("run", false);
		}

		// 移動アニメーション
		if (m_player.GetComponent<Move_Player>().GetMoveDir() != Vector3.zero)
		{
			m_anim.SetBool("walk", true);
		}
		else
		{
			m_anim.SetBool("walk", false);
		}

		// 魔法アニメーション
		if(Input.GetMouseButtonDown(1))
		{
			m_anim.SetTrigger("spell");
		}

		/*
		if(Input.GetKeyDown("space"))
		{
			m_anim.SetTrigger("rolling");
		}
		 */

		m_pastPos = transform.position;
	}

	// 攻撃アニメーション
	private void AttackAnim()
	{
		if(Input.GetMouseButtonDown(0))
		{
			// 左クリックで攻撃
			m_anim.SetTrigger("attack");
		}
	}

	// 移動できる状態か取得
	public bool GetMoveFlg()
	{
		return m_stopMove;
	}

	// ---- 移動できないアニメーションにつける関数 ----
	public void StopMove()
	{
		if(!m_stopMove)
		{
			// 移動を制限
			m_stopMove = true;
		}
	}
	public void CanMove()
	{
		if(m_stopMove)
		{
			// 移動制限を解除
			m_stopMove = false;
		}
	}

	// ---- 攻撃アニメーションにつける ----
	public void ActiveCol()
	{
		m_weaponCol.enabled = true;
	}
	public void InactiveCol()
	{
		m_weaponCol.enabled = false;       
	}

	// 位置が変わるアニメーション終了時に位置合わせをする
	public void SetModelPos()
	{
		m_player.GetComponent<Move_Player>().SetPos(transform.position);
	}

	public void ActiveTrail()
	{
		// TrailRendererを有効にする
		m_trail.GetComponent<TrailRenderer>().emitting = true;
	}

	public void InactiveTrail()
	{
		// TrailRendererを無効にする
		m_trail.GetComponent<TrailRenderer>().emitting = false;
	}

	public void SpellMagic()
	{
		Instantiate(m_magic, m_magicPoint.transform.position, Quaternion.Euler(this.transform.forward));
	}
}
