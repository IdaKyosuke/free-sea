using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "ScriptableObject/Create Demon")]
public class Demon : ScriptableObject
{
	[SerializeField] string m_name = "null";
	// �������D�ރX�e�[�^�X
	[SerializeField] Status.StatusType m_type = Status.StatusType.Hp;

	public string Name()
	{
		return m_name;
	}

	public new Status.StatusType GetType()
	{
		return m_type;
	}
}
