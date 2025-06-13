using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Status : MonoBehaviour
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

	// �X�e�[�^�X�̎����l�p�ɔ{�����擾
	public float GetMag(Status.StatusType type)
	{
		return m_demon.GetMag(type);
	}
}
