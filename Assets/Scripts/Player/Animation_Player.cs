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
	[SerializeField] GameObject m_trail;	// TrailRenderer���������I�u�W�F�N�g
	// �|�[�Y��ʊǗ��p�I�u�W�F�N�g
	[SerializeField] GameObject m_pauseManager;

	private Animator m_anim;

	// �ړ��A�j���[�V�����p
	private Vector3 m_pastPos;

	// �ړ������p
	private bool m_stopMove;

	// �U���A�j���[�V�����������Ă��邩
	const int AttackAnimNum = 3;
	private bool[] m_attackAnimFlg = new bool[AttackAnimNum];

	// ���@�U���p�̃I�u�W�F�N�g
	[SerializeField] GameObject m_magic;
	[SerializeField] GameObject m_magicPoint;

	// Start is called before the first frame update
	void Start()
    {
        m_anim = GetComponent<Animator>();
		m_pastPos = transform.position;
		m_stopMove = false;
		m_weaponCol.enabled = false;

		// TrailRenderer�𖳌��ɂ���
		m_trail.GetComponent<TrailRenderer>().emitting = false;

		for (int i = 0; i < AttackAnimNum; i++)
		{
			m_attackAnimFlg[i] = false;
		}

		// Animator����ObservableStateMachineTrigger�̎Q�Ƃ��擾
		ObservableStateMachineTrigger trigger =
			m_anim.GetBehaviour<ObservableStateMachineTrigger>();

		// State�̊J�n�C�x���g
		IDisposable enterState = trigger
			.OnStateEnterAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;

				// �A���p�t���O�Ǘ�
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

				// ���@�A�j���[�V����
				if(info.IsName("Base Layer.Spell_Magic"))
				{
					m_player.GetComponent<Move_Player>().SetCamFront();
				}
			}).AddTo(this);

		// State�̏I���C�x���g
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
					// �A�j���[�V�����̓����ɍ��킹�Ĉʒu���킹
					SetModelPos();

					if (!m_attackAnimFlg[0])
					{
						CanMove();
						InactiveCol();
					}
					m_attackAnimFlg[2] = false;
				}

				// �ʒu���킹
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

	// �ړ��A�j���[�V����
	private void Move()
	{
		// �����Ă��邩�ǂ���
		if(Input.GetKey("left shift"))
		{
			m_anim.SetBool("run", true);
		}
		else
		{
			m_anim.SetBool("run", false);
		}

		// �ړ��A�j���[�V����
		if (m_player.GetComponent<Move_Player>().GetMoveDir() != Vector3.zero)
		{
			m_anim.SetBool("walk", true);
		}
		else
		{
			m_anim.SetBool("walk", false);
		}

		// ���@�A�j���[�V����
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

	// �U���A�j���[�V����
	private void AttackAnim()
	{
		if(Input.GetMouseButtonDown(0))
		{
			// ���N���b�N�ōU��
			m_anim.SetTrigger("attack");
		}
	}

	// �ړ��ł����Ԃ��擾
	public bool GetMoveFlg()
	{
		return m_stopMove;
	}

	// ---- �ړ��ł��Ȃ��A�j���[�V�����ɂ���֐� ----
	public void StopMove()
	{
		if(!m_stopMove)
		{
			// �ړ��𐧌�
			m_stopMove = true;
		}
	}
	public void CanMove()
	{
		if(m_stopMove)
		{
			// �ړ�����������
			m_stopMove = false;
		}
	}

	// ---- �U���A�j���[�V�����ɂ��� ----
	public void ActiveCol()
	{
		m_weaponCol.enabled = true;
	}
	public void InactiveCol()
	{
		m_weaponCol.enabled = false;       
	}

	// �ʒu���ς��A�j���[�V�����I�����Ɉʒu���킹������
	public void SetModelPos()
	{
		m_player.GetComponent<Move_Player>().SetPos(transform.position);
	}

	public void ActiveTrail()
	{
		// TrailRenderer��L���ɂ���
		m_trail.GetComponent<TrailRenderer>().emitting = true;
	}

	public void InactiveTrail()
	{
		// TrailRenderer�𖳌��ɂ���
		m_trail.GetComponent<TrailRenderer>().emitting = false;
	}

	public void SpellMagic()
	{
		Instantiate(m_magic, m_magicPoint.transform.position, Quaternion.Euler(this.transform.forward));
	}
}
