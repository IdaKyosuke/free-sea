using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class StartBossBattle : MonoBehaviour
{
	[SerializeField] GameObject m_bossEnemy;    // �{�X�Ƃ��ēo�ꂳ�������G
	[SerializeField] GameObject m_areaWall;   // �{�X�Ɛ키���߂̃G���A����؂��
	[SerializeField] GameObject m_bossUI;       // �{�X�̖��O��̗�

	[SerializeField] float m_waitTime = 1.0f;  // UI��ǂ�������܂ł̎���
	private float m_duration;	// �o�ߎ��Ԃ̌v���p

	// �G�������Ă��邩�̃t���O
	private bool m_isArrive;

	// BGM
	[SerializeField] AudioSource m_normal;
	[SerializeField] AudioSource m_boss;

	// �{�X�pBGM�𗬂��܂ł̗]�C�p
	private bool m_isStart;
	[SerializeField] float m_waitTimeBGM;
	private bool m_isPlay;

    // Start is called before the first frame update
    void Start()
    {
		// �{�X�Ƃ��ēo�ꂷ��G��ݒ�
		m_bossUI.GetComponent<BossUI>().SetBoss(m_bossEnemy);
		// �퓬���n�܂�܂ŉB���Ă���
		m_bossUI.SetActive(false);
		// �ǂ��A�N�e�B�u�ɂ���
		m_areaWall.SetActive(false);
		m_isArrive = true;
		m_duration = 0;
		m_isStart = false;
		m_isPlay = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (!m_isArrive) return;

		if(m_bossEnemy.GetComponent<Move_Enemy>().GetCurrentHp() <= 0)
		{
			m_duration += Time.deltaTime;
			if(m_duration >= m_waitTime)
			{
				DeathEnemy();
			}
		}

		if(!m_isPlay && m_isStart)
		{
			m_duration += Time.deltaTime;
			if(m_duration >= m_waitTimeBGM)
			{
				m_boss.Play();
				m_isPlay = true;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!m_isArrive) return;

		// �v���C���[���͈͓��ɓ�������퓬���n�߂�
		if (other.gameObject.CompareTag("Player"))
		{
			// �ʏ�BGM���~�߂�
			m_normal.Stop();
			// �{�X�������n�߂�
			m_bossEnemy.GetComponent<Move_Enemy>().Combat();
			// �ǂ��Z�b�g����
			m_areaWall.SetActive(true);
			// ���O��̗͂�\������
			m_bossUI.SetActive(true);
			m_isStart = true;
		}
	}

	public void DeathEnemy()
	{
		m_isArrive = false;
		// �ǂ��A�N�e�B�u�ɂ���
		m_areaWall.SetActive(false);
		// ���O��̗͂��\������
		m_bossUI.SetActive(false);
	}
}
