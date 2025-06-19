using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �X�e�[�^�X�ύX�Ȃǂ��s��
public class PauseSceneManager : MonoBehaviour
{
	private bool m_isPause; // �|�[�Y��ʂ��ǂ���
	[SerializeField] string m_pauseKey = "q";   // �|�[�Y��ʂɓ���p�̃L�[
	[SerializeField] GameObject m_pauseScene;	// �|�[�Y���

    // Start is called before the first frame update
    void Start()
    {
		m_isPause = false;
		m_pauseScene.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		// �|�[�Y��ʂɓ��� or �o��
		if (Input.GetKeyDown(m_pauseKey))
		{
			m_isPause = !m_isPause;
		}
		/*
		if (m_isPause)
		{
			// Time���ւ�鏈�� && fixedUpdate ���~�߂�
			Time.timeScale = 0f;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			m_pauseScene.SetActive(true);
		}
		else
		{
			// Time���ւ�鏈�� && fixedUpdate���ĊJ
			Time.timeScale = 1.0f;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			m_pauseScene.SetActive(false);
		}
		*/
		Time.timeScale = m_isPause ? 0f : 1.0f;
		Cursor.lockState = m_isPause ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = m_isPause;
		m_pauseScene.SetActive(m_isPause);
	}

	public bool IsPause()
	{
		return m_isPause;
	}

	// �|�[�Y��ʂ̃{�^������Q�[���ɖ߂�p
	public void RestartGame()
	{
		m_isPause = false;
	}

	// �|�[�Y��ʂ̃L�[�o�C���h���󂯓n��
	public string GetPauseKey()
	{
		return m_pauseKey;
	}
}
