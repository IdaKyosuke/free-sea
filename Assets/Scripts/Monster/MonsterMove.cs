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
	[SerializeField] GameObject EncountCollider;	// �v���C���[�Ƃ̃G���J�E���g�p
	[SerializeField] GameObject MoveStopCollider;   // �v���C���[�ƈ�苗����ۂ悤
	[SerializeField] GameObject MonsterHealth;      // �����X�^�[�̗̑͊Ǘ�
	[SerializeField] GameObject Audio_InBattle;		// �퓬BGM
	Rigidbody rb;
	Animator anim;
	[SerializeField] float notCombatSpeed = 3.0f;   // ��퓬���̈ړ��X�s�[�h
	[SerializeField] float combatSpeed = 8.0f;      // �퓬���̈ړ��X�s�[�h
	[SerializeField] float EncountTime = 3.0f;      // �G���J�E���g���Ă�����K�܂ł̎���
	[SerializeField] float afterHouling = 1.0f;		// ���K��̍d��
	[SerializeField] float afterMove = 1.0f;        // �ړ���̍d��
	[SerializeField] float attackReadyTime = 1.0f;  // �U���O�̑ҋ@����
	[SerializeField] int EnemyHealth = 1000;        // �����X�^�[�̗̑�
	[SerializeField] int punchDamage = 10;			// �p���`�̃_���[�W
	[SerializeField] int headbuttDamage = 8;		// ���˂��̃_���[�W
	[SerializeField] int pressDamage = 20;      // �̂�������̃_���[�W

	[SerializeField] AudioSource Buff;	// ���K
	[SerializeField] AudioSource Attack;	// �U��

	// ��퓬���ɐi�s����������
	private float moveX;
	private float moveZ;

	private Move selectMove; // �s��������
	private int moveTime;	// �s������

	private bool isCombat = false;	// ���݂̏��
	private bool isSelected = false;	// �s�������肵����
	private float countTime = 0;    // �s�����Ԃ��v��
	private bool isHoul = false;	// ���K�J�n
	private bool isHouling = false;	// ���K��
	private bool isHouled = false;	// ���K��̍d���J�E���g�J�n
	private bool huntStarted = false;   // �퓬�J�n
	private bool isSelect = false;	// �s�����I������Ă��邩
	private bool isDistance = true; // �v���C���[�ɋ߂Â��邩
	private bool isDeath = false;	// �����X�^�[������
	private bool deathAnimFlg = false;	// ���S�A�j���[�V�����t���O
	private int isAttack = 0;  // �U�����̕���
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

		// Animator����ObservableStateMachineTrigger�̎Q�Ƃ��擾
		ObservableStateMachineTrigger trigger =
			anim.GetBehaviour<ObservableStateMachineTrigger>();

		// State�̊J�n�C�x���g
		IDisposable enterState = trigger
			.OnStateEnterAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;
				// Base Layer
				if (info.IsName("Base Layer.Bear_Buff"))
				{
					// �����Đ�
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

		// State�̏I���C�x���g
		IDisposable exitState = trigger
			.OnStateExitAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;
				// Base Layer
				if(info.IsName("Base Layer.Bear_Buff"))
				{
					// ���K��̍d���J�E���g�J�n
					isHouled = true;
					// ���߂čs����I������
					isSelect = false;
					// �퓬���̃A�j���[�V�����ɂ���
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

		// ��퓬���̍s��
        if(!isCombat || isPlayerDeath)
		{
			if(!isPlayerDeath)
			{
				isCombat = EncountCollider.GetComponent<MonsterCollider>().IsCombat();
			}
			NotCombatSelectMove();
		}
		// �퓬���̍s��
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

	// ��퓬���̍s���I��
	private void NotCombatSelectMove()
	{
		if (!isSelected)
		{
			int randomMove;
			// �����_���ȍs���̑I��
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
	// ��퓬���̍s��
	private void NotCombat()
	{
		if (isSelected)
		{
			// �I�����ꂽ�s�������s
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

	// �퓬���̍s���I��
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
			// �퓬�J�n���̂ɂ�ݍ���
			if (!huntStarted)
			{
				countTime += Time.deltaTime;
			}
			// �퓬�J�n��
			else
			{
				// �Փ˔���t���O���󂯎��
				isDistance = MoveStopCollider.GetComponent<MonsterMoveStopCollider>().IsStop();
				// ���K���͍s�����Ȃ�
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

	// �퓬���̍s��
	private void IsCombat()
	{
		if (Player)
		{
			// �퓬�J�n���̂ɂ�ݍ���
			if (!huntStarted)
			{
				if (countTime <= EncountTime)
				{
					playerPos = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);   // �v���C���[�̈ʒu���擾
					transform.position = Vector3.MoveTowards(transform.position, playerPos, notCombatSpeed * Time.deltaTime);
					anim.SetBool("notCombatWalk", true);

					transform.LookAt(playerPos);

					// �ړG�p�̃R���C�_�[������
					EncountCollider.SetActive(false);
				}
				else
				{
					countTime = 0;
					isHoul = true;
					huntStarted = true;
				}
			}
			// �퓬�J�n��
			else
			{
				// ���K���͍s�����Ȃ�
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

				// ���K
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
						//�@���̏�őҋ@
						case Move.Wait:
							Stay();
							break;
						// �v���C���[�܂ő����čs��
						case Move.Run:
							RuntoPlayer();
							break;
						// ���ʂ̏���
						case Move.Attack1:
							PunchFront();
							break;
						// �E�O�̏���
						case Move.Attack2:
							PunchRight();
							break;
						// ���O�̏���
						case Move.Attack3:
							PunchLeft();
							break;
						// �̂�������
						case Move.Attack4:
							PressFront();
							break;
						// �������Ȃ����߂̕���
						case Move.AfterAttack:
							break;
						// �ړ���̍d��
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

	// ���̏�őҋ@����
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

	// �v���C���[�Ɍ������đ����Ă���
	private void RuntoPlayer()
	{
		// �v���C���[�̕�������
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

	// �v���C���[�̊p�x�𔻒f����
	private void PlayerDirection()
	{
		var diff  = Player.transform.position - transform.position;
		var axis = Vector3.Cross(transform.forward, diff);
		var angle = Vector3.Angle(transform.forward, diff) * (axis.y < 0 ? -1 : 1);

		// ���ʂ̏���
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
		// �E�O�̏���
		else if(15 < angle && angle <= 45)
		{
			selectMove = Move.Attack2;
		}      
		// ���O�̏���
		else if (-45 <= angle && angle < -15)
		{
			selectMove = Move.Attack3;
		}
		else
		{
			// �U���͈̔͊O�̎��A��������
			playerPos = Player.transform.position;
			selectMove = Move.Run;
		}

	}

	// ���ʂɂ����Ƃ��A�̂�������
	private void PressFront()
	{
		countTime += Time.deltaTime;

		if (countTime >= attackReadyTime)
		{
			anim.SetTrigger("attack4");
		}
	}

	// ���ʂɂ����Ƃ��A���˂�
	private void PunchFront()
	{
		countTime += Time.deltaTime;

		if(countTime >= attackReadyTime)
		{
			anim.SetTrigger("attack3");
		}
	}
	// ���O�ɂ����Ƃ��A�p���`
	private void PunchLeft()
	{
		countTime += Time.deltaTime;

		if (countTime >= attackReadyTime)
		{
			anim.SetTrigger("attack2");
		}
	}   
	// �E�O�ɂ����Ƃ��A�p���`
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
			// ����
			case 1:
				return headbuttDamage;

			// �E�O
			case 2:
				return punchDamage;
				
			// ���O
			case 3:
				return punchDamage;

			// �v���X
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

	// ---- �A�j���[�V�����p ----
	public void AfterAttack()
	{
		// �U���I����̑ҋ@���Ԃɓ��邽�߂̊֐�
		isAttack = 0;
		selectMove = Move.Delay;
	}

	public void WaitFinishAttack()
	{
		// �U�����J�n������A�U���I���ҋ@��ԂɈڍs
		selectMove = Move.AfterAttack;
		// �U��SE���Đ�
		Attack.Play();
	}
}
