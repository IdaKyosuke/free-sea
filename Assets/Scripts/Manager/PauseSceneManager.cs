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

		if (m_isPause)
		{
			// fixedUpdata���~�߂�
			Time.timeScale = 0f;
			m_pauseScene.SetActive(true);
		}
		else
		{
			// fixedUpdata���ĊJ
			Time.timeScale = 1.0f;
			m_pauseScene.SetActive(false);
		}
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
}
