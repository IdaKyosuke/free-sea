using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Demon : MonoBehaviour
{
	// �����pScriptableObject
	[SerializeField] Demon m_demon;

	// �����̍D���ȃX�e�[�^�X���擾
	public Status.StatusType GetDemonType()
	{
		return m_demon.GetType();
	}

	public string GetName()
	{
		return m_demon.Name();
	}
}
