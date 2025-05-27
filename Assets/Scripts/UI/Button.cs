using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
	enum ButtonMode
	{
		SetStatus,
		ReturnGame,
	};

	[SerializeField] GameObject m_pauseManager; // �Q�[���ɖ߂�֐����g���p
	[SerializeField] GameObject m_setStatusMode;    // �X�e�[�^�X������U��p
	[SerializeField] GameObject m_selectMode;	// �Z���N�g���[�h���\���ɂ���悤
	[SerializeField] GameObject m_buttonFrame;  // �{�^���̃t���[��
	[SerializeField] ButtonMode m_buttonMode = ButtonMode.SetStatus;    // �{�^�����������Ƃ��̏�����ς���

	//[SerializeField] GameObject m_seOnCursor;	// �J�[�\�����d�Ȃ�������SE
	//[SerializeField] GameObject m_seClick;		// �N���b�N��������SE

	private void Awake()
	{
		// �ŏ��̓t���[�����\���ɂ��Ă���
		m_buttonFrame.SetActive(false);
		m_setStatusMode.SetActive(false);
	}

	// Start is called before the first frame update
	void Start()
	{
	
	}

	// �J�[�\�����d�Ȃ������̏���
	public void OnCursor()
	{
		m_buttonFrame.SetActive(true);
	}

	// �J�[�\�������ꂽ�Ƃ��̏���
	public void OutCursor()
	{
		m_buttonFrame.SetActive(false);
	}

	// �{�^�����N���b�N�������̏���
	public void Click()
	{
		switch(m_buttonMode)
		{
			case ButtonMode.SetStatus:
				m_setStatusMode.SetActive(true);
				m_buttonFrame.SetActive(false);
				m_selectMode.SetActive(false);
				break;

			case ButtonMode.ReturnGame:
				m_pauseManager.GetComponent<PauseSceneManager>().RestartGame();
				m_buttonFrame.SetActive(false);
				break;
		}
	}
}
