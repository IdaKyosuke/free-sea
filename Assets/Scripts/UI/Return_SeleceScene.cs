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
		// �|�[�Y��ʂ��J���{�^���������ꂽ���ɁA���ɊJ�����悤�ɃA�N�e�B�u�󋵂�������Ԃɖ߂��Ă���
		if(Input.GetKeyDown(m_pauseManager.GetComponent<PauseSceneManager>().GetPauseKey()))
		{
			m_selectMode.SetActive(true);
			m_statusMode.gameObject.SetActive(false);
		}
	}

	// �{�^���ɂ���悤�֐�
	public void ReturnSelectScene()
	{
		m_selectMode.SetActive(true);
		m_statusMode.gameObject.SetActive(false);
	}
}
