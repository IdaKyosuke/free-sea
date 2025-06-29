using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMainManager : MonoBehaviour
{
	[SerializeField] UnityEngine.UI.Image m_clear;
	[SerializeField] UnityEngine.UI.Image m_gameover;
	[SerializeField] UnityEngine.UI.Image m_blackBack;
	[SerializeField] float m_waitTime;
	[SerializeField] string m_nextScene;
	WaitForSeconds m_seconds = new WaitForSeconds(1.0f);
	private void Start()
	{
		m_clear.color = new Color(m_clear.color.r, m_clear.color.g, m_clear.color.b, 0);
		m_gameover.color = new Color(m_gameover.color.r, m_gameover.color.g, m_gameover.color.b, 0);
	}

	public IEnumerator Win()
	{
		yield return FadeManager.Fade(m_clear, m_waitTime);
		yield return FadeManager.Fade(m_blackBack, m_waitTime);
		yield return m_seconds;
		SceneManager.LoadScene(m_nextScene);
	}

	public IEnumerator Lose()
	{
		// �e�L�X�g��\��
		yield return FadeManager.Fade(m_gameover, m_waitTime);
		// ���w�i��\������
		yield return FadeManager.Fade(m_blackBack, m_waitTime);
		// �����҂�
		yield return m_seconds;
		SceneManager.LoadScene(m_nextScene);
	}
}
