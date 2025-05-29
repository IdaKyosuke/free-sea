using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Demon : MonoBehaviour
{
	// 悪魔用ScriptableObject
	[SerializeField] Demon m_demon;

	// 悪魔の好きなステータスを取得
	public Status.StatusType GetDemonType()
	{
		return m_demon.GetType();
	}

	public string GetName()
	{
		return m_demon.Name();
	}
}
