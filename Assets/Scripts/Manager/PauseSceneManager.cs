using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステータス変更などを行う
public class PauseSceneManager : MonoBehaviour
{
	private bool m_isPause; // ポーズ画面かどうか
	[SerializeField] string m_pauseKey = "q";   // ポーズ画面に入る用のキー
	[SerializeField] GameObject m_pauseScene;	// ポーズ画面

    // Start is called before the first frame update
    void Start()
    {
		m_isPause = false;
		m_pauseScene.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		// ポーズ画面に入る or 出る
		if (Input.GetKeyDown(m_pauseKey))
		{
			m_isPause = !m_isPause;
		}
		/*
		if (m_isPause)
		{
			// Timeが関わる処理 && fixedUpdate を止める
			Time.timeScale = 0f;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			m_pauseScene.SetActive(true);
		}
		else
		{
			// Timeが関わる処理 && fixedUpdateを再開
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

	// ポーズ画面のボタンからゲームに戻る用
	public void RestartGame()
	{
		m_isPause = false;
	}

	// ポーズ画面のキーバインドを受け渡す
	public string GetPauseKey()
	{
		return m_pauseKey;
	}
}
