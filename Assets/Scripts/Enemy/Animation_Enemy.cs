using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;

public class Animation_Enemy : MonoBehaviour
{
	[SerializeField] GameObject m_wingAnim = null; // 羽のアニメーション（羽を持たない敵はnullでいい）
	[SerializeField] GameObject m_magicCollider;    // 魔法攻撃を使える距離かを判定する
	[SerializeField] GameObject m_magicCircle;  // 魔法攻撃の際の魔法陣
	[SerializeField, Range(1, 100)] int m_probability;  // 魔法攻撃が使われる確率（％）
	[SerializeField] float m_magicRecastTime = 4.0f;    // 次に魔法攻撃の抽選に入るまでの時間
	[SerializeField] GameObject[] m_magicPoints;    // 魔法陣を生成する場所

	// 攻撃用当たり判定
	[SerializeField] GameObject m_canAttackCollider;	// 攻撃できる範囲かどうか判定する
	[SerializeField] BoxCollider m_collider;
	[SerializeField] bool m_haveManyAttacks;    // 攻撃アニメーションが複数あるか
	[SerializeField] int m_attackAnimNum;		// 攻撃アニメーションが複数ある時、いくつあるか

	[SerializeField] bool m_haveMoveAnim;       // 移動アニメーションを持っているか
	private Vector3 m_pastPos;					// 移動アニメーション反映用

	private Animator m_anim;
	private float m_duration;
	private bool m_canMove;
	bool m_isAttacked;

	private bool m_finishDeathAnim;
	private bool m_isGetHit;
	private bool m_isDeath;
	private bool m_attackMagic; // 魔法の攻撃を行った or 再抽選までのリキャスト時間かどうか
	private float m_magicDuration;  // 魔法攻撃のリキャスト時間計算用

	private bool m_isCombat;	// 接敵しているか

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
		m_attackMagic = false;
		// 攻撃判定を消す
		m_collider.enabled = false;
		m_pastPos = transform.position;
		m_isCombat = false;


		// AnimatorからObservableStateMachineTriggerの参照を取得
		ObservableStateMachineTrigger trigger =
			m_anim.GetBehaviour<ObservableStateMachineTrigger>();

		// Stateの開始イベント
		IDisposable enterState = trigger
			.OnStateEnterAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;
				if(!m_haveManyAttacks)
				{
					if (info.IsName("Base Layer.attack"))
					{
						// 攻撃中は移動できないようにする
						m_canMove = false;
					}
				}

				// 死亡アニメーション開始時
				if (info.IsName("Base Layer.Death"))
				{
					if (m_wingAnim)
					{
						m_wingAnim.GetComponent<Animation_AngelWing>().IsDeath();
						GetComponent<FlyHeight_Anim>().IsDeath();
					}
				}

			}).AddTo(this);

		// Stateの終了イベント
		IDisposable exitState = trigger
			.OnStateExitAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;
				if (!m_haveManyAttacks)
				{
					if (info.IsName("Base Layer.attack"))
					{
						// 攻撃中は移動できないようにする
						m_canMove = true;
					}
				}

				// ヒットアニメーション
				if (info.IsName("Base Layer.GetHit"))
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

		// 攻撃のリキャスト時間を計算する
		if (m_isAttacked)
		{
			CountRecastAttack();
		}

		if(m_haveMoveAnim)
		{
			MoveAnim();
		}

		if (!m_isCombat) return;

		// 魔法攻撃（遠距離攻撃）を行うか判定
		AttackMagic();

		// 近接攻撃を行うか
		MeleeAttack();
	}

	// 移動アニメーションを持っているとき反映する
	private void MoveAnim()
	{
		m_anim.SetBool("walk", transform.position != m_pastPos);
		// 1フレーム前の位置を更新
		m_pastPos = transform.position;
	}

	private void AttackMagic()
	{
		// 魔法攻撃に関しての処理
		if (!m_isGetHit && !m_isAttacked)
		{
			if (!m_attackMagic && !m_magicCollider.GetComponent<Collider_MagicArea>().ClosePlayer())
			{
				// 攻撃を受けていない && 攻撃のリキャスト時間でない && プレイヤーと近すぎない && 魔法攻撃のリキャスト時間でない
				int prob = UnityEngine.Random.Range(0, 101);
				if (prob <= m_probability)
				{
					// 設定していた確率内だった場合魔法攻撃をする
					foreach (var point in m_magicPoints)
					{
						// 魔法陣を生成する
						Instantiate(m_magicCircle, point.transform.position, Quaternion.Euler(90, 0, 0));
					}
					m_isAttacked = true;
					m_canMove = false;
				}
				m_attackMagic = true;
			}
		}

		// 魔法攻撃のリキャスト時間
		if (m_attackMagic)
		{
			m_magicDuration += Time.deltaTime;
			if (m_magicDuration >= m_magicRecastTime)
			{
				m_attackMagic = false;
				m_magicDuration = 0;
			}
		}
	}

	private void MeleeAttack()
	{
		// コライダーの範囲内にプレイヤーがいるなら
		if (m_canAttackCollider.GetComponent<CanAttackArea>().GetCanAttack())
		{
			// 近づきすぎないように調整
			m_canMove = false;

			// 死亡している || 攻撃を受けている時は何もしない
			if (m_isDeath || m_isGetHit) return;

			// 攻撃を受けていない && 攻撃のリキャスト時間でない
			if (!m_isAttacked)
			{
				if (!m_haveManyAttacks)
				{
					// 攻撃アニメーションが1つしかないとき
					m_anim.SetTrigger("attack");
				}
				else
				{
					// 攻撃アニメーションが複数ある時
					int num = UnityEngine.Random.Range(0, m_attackAnimNum) + 1;
					m_anim.SetTrigger("attack");
					m_anim.SetTrigger("attack" + num);
				}

				m_isAttacked = true;
			}

		}
		else
		{
			// プレイヤーに近づく
			m_canMove = true;
		}
	}

	// 攻撃のリキャスト時間を計算する
	private void CountRecastAttack()
	{
		m_duration += Time.deltaTime;

		if (m_duration >= 3.0f)
		{
			m_duration = 0;
			m_isAttacked = false;

			if (m_attackMagic)
			{
				// 魔法攻撃で移動が制限されているとき、動けるようにする
				m_canMove = true;
			}
		}
	}

	// 移動できるか取得
	public bool CanMove()
	{
		return m_canMove;
	}

	// アニメーションイベントで動けるかどうかを変更する
	public void SetCanMove()
	{
		m_canMove = true;
	}
	public void SetCantMove()
	{
		m_canMove = false;
	}

	// 死亡アニメーションを実行する && 死亡したフラグを立てる
	public void IsDeath()
	{
		m_isDeath = true;
		m_anim.SetTrigger("death");
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

	// ----- 攻撃判定を有効にする -----
	public void ActiveCol()
	{
		m_collider.enabled = true;
	}

	public void EnactiveCol()
	{
		m_collider.enabled = false;
	}

	// ApplyRootMotionの管理用
	public void ActiveApplyRootMotion()
	{
		m_anim.applyRootMotion = true;
	}
	public void InActiveApplyRootMotion()
	{
		m_anim.applyRootMotion = false;
	}

	// 攻撃モーション中か判定する
	public bool IsAttacked()
	{
		return m_isAttacked;
	}

	// プレイヤーと接敵状態にする
	public void SetCombat()
	{
		m_isCombat = true;
	}
}
