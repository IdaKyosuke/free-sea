using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Status : MonoBehaviour
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

	// ステータスの実数値用に倍率を取得
	public float GetMag(Status.StatusType type)
	{
		return m_demon.GetMag(type);
	}
}
