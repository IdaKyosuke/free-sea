using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Return_SeleceScene : MonoBehaviour
{
	[SerializeField] GameObject m_selectMode;
	[SerializeField] GameObject m_statusMode;
	[SerializeField] GameObject m_pauseManager;

	private void Start()
	{
		if(!m_pauseManager)
		{
			m_pauseManager = GameObject.FindWithTag("pauseManager");
		}
	}

	private void Update()
	{
		// ポーズ画面を開くボタンを押された時に、次に開く時ようにアクティブ状況を初期状態に戻しておく
		if(Input.GetKeyDown(m_pauseManager.GetComponent<PauseSceneManager>().GetPauseKey()))
		{
			m_selectMode.SetActive(true);
			m_statusMode.gameObject.SetActive(false);
		}
	}

	// ボタンにつけるよう関数
	public void ReturnSelectScene()
	{
		m_selectMode.SetActive(true);
		m_statusMode.gameObject.SetActive(false);
	}
}
