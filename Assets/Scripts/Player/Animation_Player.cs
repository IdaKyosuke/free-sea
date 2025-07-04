using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Animation_Player : MonoBehaviour
{
	[SerializeField] GameObject m_playerCam;
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

	private bool m_isDeath;	// 死亡したか

	// 攻撃アニメーションが動いているか
	const int AttackAnimNum = 3;
	private bool[] m_attackAnimFlg = new bool[AttackAnimNum];

	// 攻撃アニメーション用SE
	[SerializeField] AudioSource m_swingSword;

	// 魔法攻撃用のオブジェクト
	[SerializeField] GameObject m_magic;
	[SerializeField] GameObject m_magicPoint;

	// 魔法攻撃（必殺技）用のオーラ
	[SerializeField] GameObject m_aura;
	[SerializeField] GameObject m_lightning;

	// 必殺技で使うSE
	[SerializeField] AudioSource m_seAura;
	[SerializeField] AudioSource m_seLightning;

	private bool m_canSpecialAttack;    // 必殺技を撃てるかどうか
	private bool m_isAttackSpecial;     // 必殺技を発動中か

	// 必殺技ゲージが溜まっているか確認用
	[SerializeField] GameObject m_zoneManager;

	// 今が無敵時間かどうか
	private bool m_isInvincible;

	// Start is called before the first frame update
	void Start()
    {
        m_anim = GetComponent<Animator>();
		m_pastPos = transform.position;
		m_stopMove = false;
		m_weaponCol.enabled = false;
		m_canSpecialAttack = true;
		m_isAttackSpecial = false;
		m_isDeath = false;
		m_isInvincible = false;
		if (!m_zoneManager)
		{
			m_zoneManager = GameObject.FindWithTag("zoneGaugeManager");
		}
		if(!m_pauseManager)
		{
			m_pauseManager = GameObject.FindWithTag("pauseManager");
		}
		if(!m_playerCam)
		{
			m_playerCam  = GameObject.FindWithTag("playerCam");
		}

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
					m_swingSword.Play();
				}
				if (info.IsName("Base Layer.Attack2"))
				{
					m_attackAnimFlg[1] = true;
					m_swingSword.Play();
				}
				if (info.IsName("Base Layer.Attack3"))
				{
					m_attackAnimFlg[2] = true;
					m_swingSword.Play();
				}

				// ローリング時に無敵判定を用意する
				if (info.IsName("Base Layer.Rolling"))
				{
					m_isInvincible = true;
				}

				// 魔法アニメーション
				if (info.IsName("Base Layer.Spell_Magic"))
				{
					m_player.GetComponent<Move_Player>().SetCamFront();
				}

				// 必殺技開始時に無敵にする
				if(info.IsName("Base Layer.Attack_Special"))
				{
					m_isInvincible = true;
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
					CanMove();
					// 無敵解除
					m_isInvincible = false;	
				}

				if(info.IsName("Base Layer.Attack_Spell"))
				{
					CanMove();
				}

				// 必殺技終了
				if(info.IsName("Base Layer.Attack_Special"))
				{
					m_isAttackSpecial = false;
					m_isInvincible = true;
					CanMove();
				}
			}).AddTo(this);
	}

    // Update is called once per frame
    void Update()
    {
		if (m_isDeath) return;

		if(m_pauseManager.GetComponent<PauseSceneManager>().IsPause())
		{
			return;
		}

		foreach(var attackAnimFlg in m_attackAnimFlg)
		{
			if(attackAnimFlg)
			{
				m_canSpecialAttack = false;
				break;
			}
			m_canSpecialAttack = true;
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
		
		if(Input.GetKeyDown("space"))
		{
			m_anim.SetTrigger("rolling");
		}
		
		m_pastPos = transform.position;
	}

	// 攻撃アニメーション
	private void AttackAnim()
	{
		if(!m_isAttackSpecial && Input.GetMouseButtonDown(0))
		{
			// 左クリックで攻撃
			m_anim.SetTrigger("attack");
		}

		// 必殺ゲージが溜まってたら
		if(m_zoneManager.GetComponent<ZoneGaugeManager>().IsMax())
		{
			// 必殺技
			if(m_canSpecialAttack && Input.GetKeyDown("e"))
			{
				m_anim.SetTrigger("attack_special");
				m_isAttackSpecial = true;
				// ゲージをリセット
				m_zoneManager.GetComponent<ZoneGaugeManager>().ResetGauge();
			}
		}

		// 必殺技（テスト用）
		if (m_canSpecialAttack && Input.GetKeyDown("e"))
		{
			m_anim.SetTrigger("attack_special");
			m_isAttackSpecial = true;
		}
	}

	// 移動できる状態か取得
	public bool GetMoveFlg()
	{
		return m_stopMove;
	}

	// 今が無敵時間かを取得
	public bool IsInvincible()
	{
		return m_isInvincible;
	}

	// ---- 移動できないアニメーションにつける関数 ----
	public void StopMove()
	{
		if(!m_stopMove)
		{
			// 移動を制限
			m_stopMove = true;
			// アニメーションの動きを反映
			m_anim.applyRootMotion = true;
		}
	}
	public void CanMove()
	{
		if(m_stopMove)
		{
			// 移動制限を解除
			m_stopMove = false;
			// アニメーションの動きを反映
			m_anim.applyRootMotion = false;
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


	// ----- 必殺技関連 -----
	public void SetAura()
	{
		// オーラをまとわせる
		Instantiate(m_aura, m_player.transform.position, Quaternion.identity);
		m_seAura.Play();
	}
	public void SetEffect()
	{
		// 攻撃エフェクトを発生させる
		Instantiate(m_lightning, m_player.transform.position, Quaternion.identity);
		m_seLightning.Play();
	}

	// カメラの距離を必殺技用にする
	public void SetCamSpecial()
	{
		m_playerCam.GetComponent<Camera_Player>().SetCamRangeForSpecial();
	}

	// カメラの距離を通常にする
	public void SetCamNormal()
	{
		m_playerCam.GetComponent<Camera_Player>().SetCamRangeForNormal();
	}

	// 死亡アニメーション
	public void DeathAnim()
	{
		// 死亡フラグを共有
		m_isDeath = true;

		// アニメーション再生
		m_anim.SetTrigger("death");
	}
}
