using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return_SeleceScene : MonoBehaviour
{
	[SerializeField] GameObject m_selectMode;
	[SerializeField] GameObject m_statusMode;

    public void ReturnSelectScene()
	{
		m_selectMode.SetActive(true);
		m_statusMode.gameObject.SetActive(false);
	}
}
