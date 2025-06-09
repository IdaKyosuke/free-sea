using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "ScriptableObject/Create Demon")]
public class Demon : ScriptableObject
{
	[SerializeField] string m_name = "null";
	// 悪魔が好むステータス
	[SerializeField] Status.StatusType m_type = Status.StatusType.Hp;

	// 悪魔との契約でかかるステータスの倍率
	[SerializeField] float[] m_mag = new float[(int)Status.StatusType.Length - 1];

	public string Name()
	{
		return m_name;
	}

	public new Status.StatusType GetType()
	{
		return m_type;
	}

	// ステータスの実数値用に倍率を取得
	public float GetMag(Status.StatusType type)
	{
		// もし配列外を指定したときは-1を返す
		if ((int)type >= m_mag.Length) return -1;
		return m_mag[(int)type];
	}
}
