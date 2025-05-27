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

	[SerializeField] GameObject m_pauseManager; // ゲームに戻る関数を使う用
	[SerializeField] GameObject m_setStatusMode;    // ステータスを割り振る用
	[SerializeField] GameObject m_selectMode;	// セレクトモードを非表示にするよう
	[SerializeField] GameObject m_buttonFrame;  // ボタンのフレーム
	[SerializeField] ButtonMode m_buttonMode = ButtonMode.SetStatus;    // ボタンを押したときの処理を変える

	//[SerializeField] GameObject m_seOnCursor;	// カーソルが重なった時のSE
	//[SerializeField] GameObject m_seClick;		// クリックした時のSE

	private void Awake()
	{
		// 最初はフレームを非表示にしておく
		m_buttonFrame.SetActive(false);
		m_setStatusMode.SetActive(false);
	}

	// Start is called before the first frame update
	void Start()
	{
	
	}

	// カーソルが重なった時の処理
	public void OnCursor()
	{
		m_buttonFrame.SetActive(true);
	}

	// カーソルが離れたときの処理
	public void OutCursor()
	{
		m_buttonFrame.SetActive(false);
	}

	// ボタンをクリックした時の処理
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
