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

	private bool m_isDeath;	// ���S������

	// �U���A�j���[�V�����������Ă��邩
	const int AttackAnimNum = 3;
	private bool[] m_attackAnimFlg = new bool[AttackAnimNum];

	// ���@�U���p�̃I�u�W�F�N�g
	[SerializeField] GameObject m_magic;
	[SerializeField] GameObject m_magicPoint;

	// ���@�U���i�K�E�Z�j�p�̃I�[��
	[SerializeField] GameObject m_aura;
	[SerializeField] GameObject m_lightning;

	private bool m_canSpecialAttack;    // �K�E�Z�����Ă邩�ǂ���
	private bool m_isAttackSpecial;     // �K�E�Z�𔭓�����

	// �K�E�Z�Q�[�W�����܂��Ă��邩�m�F�p
	[SerializeField] GameObject m_zoneManager;

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
		if (!m_zoneManager)
		{
			m_zoneManager = GameObject.FindWithTag("zoneGaugeManager");
		}

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
					CanMove();
				}

				if(info.IsName("Base Layer.Attack_Spell"))
				{
					CanMove();
				}

				// �K�E�Z�I��
				if(info.IsName("Base Layer.Attack_Special"))
				{
					m_isAttackSpecial = false;
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
		
		if(Input.GetKeyDown("space"))
		{
			m_anim.SetTrigger("rolling");
		}
		
		m_pastPos = transform.position;
	}

	// �U���A�j���[�V����
	private void AttackAnim()
	{
		if(!m_isAttackSpecial && Input.GetMouseButtonDown(0))
		{
			// ���N���b�N�ōU��
			m_anim.SetTrigger("attack");
		}

		// �K�E�Q�[�W�����܂��Ă���
		if(m_zoneManager.GetComponent<ZoneGaugeManager>().IsMax())
		{
			// �K�E�Z
			if(m_canSpecialAttack && Input.GetKeyDown("e"))
			{
				m_anim.SetTrigger("attack_special");
				m_isAttackSpecial = true;
				// �Q�[�W�����Z�b�g
				m_zoneManager.GetComponent<ZoneGaugeManager>().ResetGauge();
			}
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
			// �A�j���[�V�����̓����𔽉f
			m_anim.applyRootMotion = true;
		}
	}
	public void CanMove()
	{
		if(m_stopMove)
		{
			// �ړ�����������
			m_stopMove = false;
			// �A�j���[�V�����̓����𔽉f
			m_anim.applyRootMotion = false;
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


	// ----- �K�E�Z�֘A -----
	public void SetAura()
	{
		// �I�[�����܂Ƃ킹��
		Instantiate(m_aura, m_player.transform.position, Quaternion.identity);
	}
	public void SetEffect()
	{
		// �U���G�t�F�N�g�𔭐�������
		Instantiate(m_lightning, m_player.transform.position, Quaternion.identity);
	}

	// ���S�A�j���[�V����
	public void DeathAnim()
	{
		// ���S�t���O�����L
		m_isDeath = true;

		// �A�j���[�V�����Đ�
		m_anim.SetTrigger("death");
	}
}
