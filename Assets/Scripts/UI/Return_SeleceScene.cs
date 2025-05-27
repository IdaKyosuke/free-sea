using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return_SeleceScene : MonoBehaviour
{
	[SerializeField] GameObject m_selectMode;

    public void ReturnSelectScene()
	{
		m_selectMode.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
