using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyBind", menuName = "ScriptableObject/Create KeyBind")]
public class KeyBind : ScriptableObject
{
	public enum KeyBindName
	{
		// 移動
		MoveForward,
		MoveBack,
		MoveRight,
		MoveLeft,
		// ポーズ画面
		PauseScene,
		// 攻撃など
		Rolling,
		SpecialAttack,
		// 悪魔の変更
		ChangeUpDemon,
		ChangeDownDemon,

		Length,
	}

	// 実際のキーバインド
	public string[] m_keyBinds =
	{
		// 移動
		"w",
		"s",
		"d",
		"a",
		// ポーズ画面
		"q",
		// 攻撃など
		"space",
		"e",
		// 悪魔の変更
		"1",
		"2",
	};
}
