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

		if (m_isPause)
		{
			// fixedUpdataを止める
			Time.timeScale = 0f;
			m_pauseScene.SetActive(true);
		}
		else
		{
			// fixedUpdataを再開
			Time.timeScale = 1.0f;
			m_pauseScene.SetActive(false);
		}
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
}
