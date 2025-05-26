using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Net.Sockets;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;

public enum Move
{
	Wait,
	Run,
	Attack1,
	Attack2,
	Attack3,
	Attack4,
	AfterAttack,
	Delay,
}

public class MonsterMove : MonoBehaviour
{
	[SerializeField] GameObject Player;
	[SerializeField] GameObject PlayerHealth;
	[SerializeField] GameObject EncountCollider;	// プレイヤーとのエンカウント用
	[SerializeField] GameObject MoveStopCollider;   // プレイヤーと一定距離を保つよう
	[SerializeField] GameObject MonsterHealth;      // モンスターの体力管理
	[SerializeField] GameObject Audio_InBattle;		// 戦闘BGM
	Rigidbody rb;
	Animator anim;
	[SerializeField] float notCombatSpeed = 3.0f;   // 非戦闘時の移動スピード
	[SerializeField] float combatSpeed = 8.0f;      // 戦闘時の移動スピード
	[SerializeField] float EncountTime = 3.0f;      // エンカウントしてから咆哮までの時間
	[SerializeField] float afterHouling = 1.0f;		// 咆哮後の硬直
	[SerializeField] float afterMove = 1.0f;        // 移動後の硬直
	[SerializeField] float attackReadyTime = 1.0f;  // 攻撃前の待機時間
	[SerializeField] int EnemyHealth = 1000;        // モンスターの体力
	[SerializeField] int punchDamage = 10;			// パンチのダメージ
	[SerializeField] int headbuttDamage = 8;		// 頭突きのダメージ
	[SerializeField] int pressDamage = 20;      // のしかかりのダメージ

	[SerializeField] AudioSource Buff;	// 咆哮
	[SerializeField] AudioSource Attack;	// 攻撃

	// 非戦闘時に進行方向を決定
	private float moveX;
	private float moveZ;

	private Move selectMove; // 行動を決定
	private int moveTime;	// 行動時間

	private bool isCombat = false;	// 現在の状態
	private bool isSelected = false;	// 行動が決定したか
	private float countTime = 0;    // 行動時間を計測
	private bool isHoul = false;	// 咆哮開始
	private bool isHouling = false;	// 咆哮中
	private bool isHouled = false;	// 咆哮後の硬直カウント開始
	private bool huntStarted = false;   // 戦闘開始
	private bool isSelect = false;	// 行動が選択されているか
	private bool isDistance = true; // プレイヤーに近づけるか
	private bool isDeath = false;	// モンスターが死んだ
	private bool deathAnimFlg = false;	// 死亡アニメーションフラグ
	private int isAttack = 0;  // 攻撃中の部位
	private bool isPlayerDeath = false;
	Vector3 moveDirection = Vector3.zero;
	Vector3 position;
	Vector3 postPosition;
	Vector3 delta;
	Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
		/*
		if(!MonsterHealth)
		{
			MonsterHealth = GameObject.FindWithTag("MonsterHealth");
		}
		 */

		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		moveX = 0;
		moveZ = 0;
		selectMove = 0;
		moveTime = 0;
		position = new Vector3(transform.position.x, 0, transform.position.z);
		postPosition = new Vector3(transform.position.x, 0, transform.position.z);
		EncountCollider.SetActive(true);
		MoveStopCollider.SetActive(true);
		//Audio_InBattle.SetActive(false);

		// AnimatorからObservableStateMachineTriggerの参照を取得
		ObservableStateMachineTrigger trigger =
			anim.GetBehaviour<ObservableStateMachineTrigger>();

		// Stateの開始イベント
		IDisposable enterState = trigger
			.OnStateEnterAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;
				// Base Layer
				if (info.IsName("Base Layer.Bear_Buff"))
				{
					// 声を再生
					Buff.Play();
				}
				if (info.IsName("Base Layer.HeadButt"))
				{
					isAttack = 1;
				}
				if (info.IsName("Base Layer.RightPunch"))
				{
					isAttack = 2;
				}
				if(info.IsName("Base Layer.LeftPunch"))
				{
					isAttack = 3;
				}
				if(info.IsName("Base Layer.Press"))
				{
					isAttack = 4;
				}
				
			}).AddTo(this);

		// Stateの終了イベント
		IDisposable exitState = trigger
			.OnStateExitAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;
				// Base Layer
				if(info.IsName("Base Layer.Bear_Buff"))
				{
					// 咆哮後の硬直カウント開始
					isHouled = true;
					// 改めて行動を選択する
					isSelect = false;
					// 戦闘中のアニメーションにする
					anim.SetBool("isCombat", true);
				}
			}).AddTo(this);
	}

    // Update is called once per frame
    void Update()
    {
		//isDeath = MonsterHealth.GetComponent<MonsterHealth>().IsDeath();
		//isPlayerDeath = PlayerHealth.GetComponent<PlayerHealth>().IsDeath();

		if(isDeath && !deathAnimFlg)
		{
			anim.SetTrigger("deathTrigger");
			deathAnimFlg = true;
		}

		// 非戦闘時の行動
        if(!isCombat || isPlayerDeath)
		{
			if(!isPlayerDeath)
			{
				isCombat = EncountCollider.GetComponent<MonsterCollider>().IsCombat();
			}
			NotCombatSelectMove();
		}
		// 戦闘時の行動
		else
		{
			IsCombatSelectMove();
		}

    }

	private void FixedUpdate()
	{
		if(isDeath)
		{
			return;
		}

		if (!isCombat || isPlayerDeath)
		{
			NotCombat();
		}
		else
		{
			IsCombat();
		}
	}

	// 非戦闘時の行動選択
	private void NotCombatSelectMove()
	{
		if (!isSelected)
		{
			int randomMove;
			// ランダムな行動の選択
			randomMove = UnityEngine.Random.Range(0, 2);
			if (randomMove != 0)
			{
				selectMove = Move.Run;
				moveX = UnityEngine.Random.Range(-1, 2);
				moveZ = UnityEngine.Random.Range(-1, 2);
				moveDirection = new Vector3(moveX, 0, moveZ);
			}
			else
			{
				selectMove = Move.Wait;
			}
			moveTime = UnityEngine.Random.Range(2, 6);
			isSelected = true;
			countTime = 0;
		}
	}
	// 非戦闘時の行動
	private void NotCombat()
	{
		if (isSelected)
		{
			// 選択された行動を実行
			countTime += Time.deltaTime;
			if (countTime <= moveTime)
			{
				position = new Vector3(transform.position.x, 0, transform.position.z);
				delta = position - postPosition;
				switch (selectMove)
				{
					case Move.Wait:
						rb.velocity = Vector3.zero;
						anim.SetBool("notCombatWalk", false);
						break;

					case Move.Run:
						rb.velocity = moveDirection * notCombatSpeed;
						anim.SetBool("notCombatWalk", true);


						if (delta != Vector3.zero)
						{
							var rotation = Quaternion.LookRotation(delta, Vector3.up);
							transform.rotation = rotation;
						}

						break;
				}
				postPosition = position;
			}
			else
			{
				isSelected = false;
			}
		}
	}

	// 戦闘時の行動選択
	private void IsCombatSelectMove()
	{
		if (!Player)
		{
			Player = GameObject.FindWithTag("Player");
			//Audio_InBattle.SetActive(true);
			countTime = 0;
		}
		else
		{
			// 戦闘開始時のにらみ合い
			if (!huntStarted)
			{
				countTime += Time.deltaTime;
			}
			// 戦闘開始後
			else
			{
				// 衝突判定フラグを受け取る
				isDistance = MoveStopCollider.GetComponent<MonsterMoveStopCollider>().IsStop();
				// 咆哮中は行動しない
				if (isHouling)
				{
					return;
				}

				if (!isSelect)
				{
					int randomMove = UnityEngine.Random.Range(0, 4);

					if(randomMove == 0)
					{
						selectMove = Move.Wait;
						moveTime = UnityEngine.Random.Range(1, 3);
					}
					else
					{
						selectMove = Move.Run;
						playerPos = Player.transform.position;
					}
					isSelect = true;
					countTime = 0;
				}
			}
		}
	}

	// 戦闘時の行動
	private void IsCombat()
	{
		if (Player)
		{
			// 戦闘開始時のにらみ合い
			if (!huntStarted)
			{
				if (countTime <= EncountTime)
				{
					playerPos = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);   // プレイヤーの位置を取得
					transform.position = Vector3.MoveTowards(transform.position, playerPos, notCombatSpeed * Time.deltaTime);
					anim.SetBool("notCombatWalk", true);

					transform.LookAt(playerPos);

					// 接敵用のコライダーを消す
					EncountCollider.SetActive(false);
				}
				else
				{
					countTime = 0;
					isHoul = true;
					huntStarted = true;
				}
			}
			// 戦闘開始後
			else
			{
				// 咆哮中は行動しない
				if (isHouling)
				{
					if(isHouled)
					{
						countTime += Time.deltaTime;
                        if (countTime >= afterHouling)
                        {
							isHouling = false;
							isHouled = false;
                        }
                    }

					return;
				}

				// 咆哮
				if (isHoul)
				{
					anim.SetTrigger("buffed");
					rb.velocity = Vector3.zero;
					isHoul = false;
					isHouling = true;
				}
				
				if (isSelect)
				{
					switch (selectMove)
					{
						//　その場で待機
						case Move.Wait:
							Stay();
							break;
						// プレイヤーまで走って行く
						case Move.Run:
							RuntoPlayer();
							break;
						// 正面の処理
						case Move.Attack1:
							PunchFront();
							break;
						// 右前の処理
						case Move.Attack2:
							PunchRight();
							break;
						// 左前の処理
						case Move.Attack3:
							PunchLeft();
							break;
						// のしかかり
						case Move.Attack4:
							PressFront();
							break;
						// 何もしないための分岐
						case Move.AfterAttack:
							break;
						// 移動後の硬直
						case Move.Delay:
							countTime += Time.deltaTime;
							if(countTime >= afterMove)
							{
								isSelect = false;
								isDistance = true;
							}
							break;
					}
				}
			}
		}
	}

	// その場で待機する
	private void Stay()
	{
		countTime += Time.deltaTime;

		anim.SetBool("combatWalk", false);

		if (countTime >= moveTime)
		{
			isSelect = false;
			countTime = 0;
		}
	}

	// プレイヤーに向かって走っていく
	private void RuntoPlayer()
	{
		// プレイヤーの方を向く
		transform.rotation = Quaternion.Slerp(
			transform.rotation,
			Quaternion.LookRotation(playerPos - transform.position),
			0.1f
		);

		if (!isDistance)
		{
			transform.position = Vector3.MoveTowards(transform.position, playerPos, combatSpeed * Time.deltaTime);
			anim.SetBool("combatWalk", true);
		}

		if (isDistance ? true : playerPos == transform.position)
		{
			anim.SetBool("combatWalk", false);
			rb.velocity = Vector3.zero;
			countTime = 0;
			PlayerDirection();
		}
	}

	// プレイヤーの角度を判断する
	private void PlayerDirection()
	{
		var diff  = Player.transform.position - transform.position;
		var axis = Vector3.Cross(transform.forward, diff);
		var angle = Vector3.Angle(transform.forward, diff) * (axis.y < 0 ? -1 : 1);

		// 正面の処理
		if (-15 <= angle && angle <= 15)
        {
			if(0 <= angle)
			{
				int move = UnityEngine.Random.Range(0, 2);
				switch(move)
				{
					case 0:
						selectMove = Move.Attack1;
						break;

					case 1:
						selectMove = Move.Attack4;
						break;
				}
			}
			else
			{
				selectMove = Move.Attack4;
			}
		}
		// 右前の処理
		else if(15 < angle && angle <= 45)
		{
			selectMove = Move.Attack2;
		}      
		// 左前の処理
		else if (-45 <= angle && angle < -15)
		{
			selectMove = Move.Attack3;
		}
		else
		{
			// 攻撃の範囲外の時、向き直す
			playerPos = Player.transform.position;
			selectMove = Move.Run;
		}

	}

	// 正面にいたとき、のしかかり
	private void PressFront()
	{
		countTime += Time.deltaTime;

		if (countTime >= attackReadyTime)
		{
			anim.SetTrigger("attack4");
		}
	}

	// 正面にいたとき、頭突き
	private void PunchFront()
	{
		countTime += Time.deltaTime;

		if(countTime >= attackReadyTime)
		{
			anim.SetTrigger("attack3");
		}
	}
	// 左前にいたとき、パンチ
	private void PunchLeft()
	{
		countTime += Time.deltaTime;

		if (countTime >= attackReadyTime)
		{
			anim.SetTrigger("attack2");
		}
	}   
	// 右前にいたとき、パンチ
	private void PunchRight()
	{
		countTime += Time.deltaTime;

		if (countTime >= attackReadyTime)
		{
			anim.SetTrigger("attack1");
		}
	}   

	public int Damage()
	{
		switch (isAttack)
		{
			// 正面
			case 1:
				return headbuttDamage;

			// 右前
			case 2:
				return punchDamage;
				
			// 左前
			case 3:
				return punchDamage;

			// プレス
			case 4:
				return pressDamage;

			default:
				return 0;
		}
	}

	public int Health()
	{
		return EnemyHealth;
	}

	public int IsAttack()
	{
		return isAttack;
	}

	// ---- アニメーション用 ----
	public void AfterAttack()
	{
		// 攻撃終了後の待機時間に入るための関数
		isAttack = 0;
		selectMove = Move.Delay;
	}

	public void WaitFinishAttack()
	{
		// 攻撃を開始したら、攻撃終了待機状態に移行
		selectMove = Move.AfterAttack;
		// 攻撃SEを再生
		Attack.Play();
	}
}
