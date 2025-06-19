using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyBind", menuName = "ScriptableObject/Create KeyBind")]
public class KeyBind : ScriptableObject
{
	public enum KeyBindName
	{
		// �ړ�
		MoveForward,
		MoveBack,
		MoveRight,
		MoveLeft,
		// �|�[�Y���
		PauseScene,
		// �U���Ȃ�
		Rolling,
		SpecialAttack,
		// �����̕ύX
		ChangeUpDemon,
		ChangeDownDemon,

		Length,
	}

	// ���ۂ̃L�[�o�C���h
	public string[] m_keyBinds =
	{
		// �ړ�
		"w",
		"s",
		"d",
		"a",
		// �|�[�Y���
		"q",
		// �U���Ȃ�
		"space",
		"e",
		// �����̕ύX
		"1",
		"2",
	};
}
