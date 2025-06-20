using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;

public class Animation_Enemy : MonoBehaviour
{
	[SerializeField] GameObject m_wingAnim = null; // �H�̃A�j���[�V�����i�H�������Ȃ��G��null�ł����j
	[SerializeField] GameObject m_magicCollider;    // ���@�U�����g���鋗�����𔻒肷��
	[SerializeField] GameObject m_magicCircle;  // ���@�U���̍ۂ̖��@�w
	[SerializeField, Range(1, 100)] int m_probability;  // ���@�U�����g����m���i���j
	[SerializeField] float m_magicRecastTime = 4.0f;    // ���ɖ��@�U���̒��I�ɓ���܂ł̎���
	[SerializeField] GameObject[] m_magicPoints;    // ���@�w�𐶐�����ꏊ

	// �U���p�����蔻��
	[SerializeField] GameObject m_canAttackCollider;	// �U���ł���͈͂��ǂ������肷��
	[SerializeField] BoxCollider m_collider;
	[SerializeField] bool m_haveManyAttacks;    // �U���A�j���[�V�������������邩
	[SerializeField] int m_attackAnimNum;		// �U���A�j���[�V�������������鎞�A�������邩

	[SerializeField] bool m_haveMoveAnim;       // �ړ��A�j���[�V�����������Ă��邩
	private Vector3 m_pastPos;					// �ړ��A�j���[�V�������f�p

	private Animator m_anim;
	private float m_duration;
	private bool m_canMove;
	bool m_isAttacked;

	private bool m_finishDeathAnim;
	private bool m_isGetHit;
	private bool m_isDeath;
	private bool m_attackMagic; // ���@�̍U�����s���� or �Ē��I�܂ł̃��L���X�g���Ԃ��ǂ���
	private float m_magicDuration;  // ���@�U���̃��L���X�g���Ԍv�Z�p


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
		// �U�����������
		m_collider.enabled = false;
		m_pastPos = transform.position;

		// Animator����ObservableStateMachineTrigger�̎Q�Ƃ��擾
		ObservableStateMachineTrigger trigger =
			m_anim.GetBehaviour<ObservableStateMachineTrigger>();

		// State�̊J�n�C�x���g
		IDisposable enterState = trigger
			.OnStateEnterAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;
				if(!m_haveManyAttacks)
				{
					if (info.IsName("Base Layer.attack"))
					{
						// �U�����͈ړ��ł��Ȃ��悤�ɂ���
						m_canMove = false;
					}
				}

				// ���S�A�j���[�V�����J�n��
				if (info.IsName("Base Layer.Death"))
				{
					if (m_wingAnim)
					{
						m_wingAnim.GetComponent<Animation_AngelWing>().IsDeath();
						GetComponent<FlyHeight_Anim>().IsDeath();
					}
				}

			}).AddTo(this);

		// State�̏I���C�x���g
		IDisposable exitState = trigger
			.OnStateExitAsObservable()
			.Subscribe(onStateInfo =>
			{
				AnimatorStateInfo info = onStateInfo.StateInfo;
				if (!m_haveManyAttacks)
				{
					if (info.IsName("Base Layer.attack"))
					{
						// �U�����͈ړ��ł��Ȃ��悤�ɂ���
						m_canMove = true;
					}
				}

				// �q�b�g�A�j���[�V����
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

		// �U���̃��L���X�g���Ԃ��v�Z����
		if (m_isAttacked)
		{
			CountRecastAttack();
		}

		if(m_haveMoveAnim)
		{
			MoveAnim();
		}

		// ���@�U���i�������U���j���s��������
		AttackMagic();

		// �ߐڍU�����s����
		MeleeAttack();
	}

	// �ړ��A�j���[�V�����������Ă���Ƃ����f����
	private void MoveAnim()
	{
		m_anim.SetBool("walk", transform.position != m_pastPos);
		// 1�t���[���O�̈ʒu���X�V
		m_pastPos = transform.position;
	}

	private void AttackMagic()
	{
		// ���@�U���Ɋւ��Ă̏���
		if (!m_isGetHit && !m_isAttacked)
		{
			if (!m_attackMagic && !m_magicCollider.GetComponent<Collider_MagicArea>().ClosePlayer())
			{
				// �U�����󂯂Ă��Ȃ� && �U���̃��L���X�g���ԂłȂ� && �v���C���[�Ƌ߂����Ȃ� && ���@�U���̃��L���X�g���ԂłȂ�
				int prob = UnityEngine.Random.Range(0, 101);
				if (prob <= m_probability)
				{
					// �ݒ肵�Ă����m�����������ꍇ���@�U��������
					foreach (var point in m_magicPoints)
					{
						// ���@�w�𐶐�����
						Instantiate(m_magicCircle, point.transform.position, Quaternion.Euler(90, 0, 0));
					}
					m_isAttacked = true;
					m_canMove = false;
				}
				m_attackMagic = true;
			}
		}

		// ���@�U���̃��L���X�g����
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
		// �R���C�_�[�͈͓̔��Ƀv���C���[������Ȃ�
		if (m_canAttackCollider.GetComponent<CanAttackArea>().GetCanAttack())
		{
			// �߂Â������Ȃ��悤�ɒ���
			m_canMove = false;

			// ���S���Ă��� || �U�����󂯂Ă��鎞�͉������Ȃ�
			if (m_isDeath || m_isGetHit) return;

			// �U�����󂯂Ă��Ȃ� && �U���̃��L���X�g���ԂłȂ�
			if (!m_isAttacked)
			{
				if (!m_haveManyAttacks)
				{
					// �U���A�j���[�V������1�����Ȃ��Ƃ�
					m_anim.SetTrigger("attack");
				}
				else
				{
					// �U���A�j���[�V�������������鎞
					int num = UnityEngine.Random.Range(0, m_attackAnimNum) + 1;
					m_anim.SetTrigger("attack");
					m_anim.SetTrigger("attack" + num);
				}

				m_isAttacked = true;
			}
		}
		else
		{
			// �v���C���[�ɋ߂Â�
			m_canMove = true;
		}
	}

	// �U���̃��L���X�g���Ԃ��v�Z����
	private void CountRecastAttack()
	{
		m_duration += Time.deltaTime;

		if (m_duration >= 3.0f)
		{
			m_duration = 0;
			m_isAttacked = false;

			if (m_attackMagic)
			{
				// ���@�U���ňړ�����������Ă���Ƃ��A������悤�ɂ���
				m_canMove = true;
			}
		}
	}

	// �ړ��ł��邩�擾
	public bool CanMove()
	{
		return m_canMove;
	}

	// �A�j���[�V�����C�x���g�œ����邩�ǂ�����ύX����
	public void SetCanMove()
	{
		m_canMove = true;
	}
	public void SetCantMove()
	{
		m_canMove = false;
	}

	// ���S�A�j���[�V���������s���� && ���S�����t���O�𗧂Ă�
	public void IsDeath()
	{
		m_isDeath = true;
		m_anim.SetTrigger("death");
	}

	// �_���[�W���󂯂�A�j���[�V���������s����
	public void GetHit()
	{
		m_anim.SetTrigger("getHit");
		m_isGetHit = true;
		m_canMove = !m_isGetHit;
	}

	// ���S�A�j���[�V�����̏I���t���O�𗧂Ă�
	public void SetFinishAnimFlg()
	{
		m_finishDeathAnim = true;
	}

	// ���S�A�j���[�V�����̏I�����擾
	public bool FinishDeathAnim()
	{
		return m_finishDeathAnim;
	}

	// ----- �U�������L���ɂ��� -----
	public void ActiveCol()
	{
		m_collider.enabled = true;
	}

	public void EnactiveCol()
	{
		m_collider.enabled = false;
	}

	// ApplyRootMotion�̊Ǘ��p
	public void ActiveApplyRootMotion()
	{
		m_anim.applyRootMotion = true;
	}
	public void InActiveApplyRootMotion()
	{
		m_anim.applyRootMotion = false;
	}
}
