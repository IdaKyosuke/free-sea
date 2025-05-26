using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "ScriptableObject/Create Status")]
public class Status : ScriptableObject
{
	// ステータスの要素
	public enum StatusType
	{
		Hp,
		Atk,
		Def,
		Spd,
		SkillPoint,

		Length,
	}

	// 初期ステータス
	[SerializeField] int m_lv;	// 敵のステータスの時は使わない?
	[SerializeField] int m_hp;
	[SerializeField] int m_atk;
	[SerializeField] int m_def;
	[SerializeField] int m_spd;
	[SerializeField] int m_skillPoint;  // プレイヤー専用

	// ステータスの配列
	private int[] m_status = new int[(int)StatusType.Length];

	// ステータスが初期化されたかどうか
	private bool m_isInitialized = false;

	// プレイヤー用の関数
	// ステータスの初期化
	public void Initialize()
	{
		// 多重初期化されないようフラグで管理
		if(!m_isInitialized)
		{
			m_status[(int)StatusType.Hp] = m_hp;
			m_status[(int)StatusType.Atk] = m_atk;
			m_status[(int)StatusType.Def] = m_def;
			m_status[(int)StatusType.Spd] = m_spd;
			m_status[(int)StatusType.SkillPoint] = m_skillPoint;
			m_isInitialized = true;
		}
	}

	public void LvUp() { m_lv++; }

	// ---- スキルポイント割り振り用 ----
	public void UseSkillPoint(StatusType type, int point) 
	{
		// 任意のステータスを強化
		m_status[(int)type] += point;
		// スキルポイントを消費
		m_status[(int)StatusType.SkillPoint]--; 
	}

	// スキル振り直し用
	public void ResetSkillPoint(StatusType type, int point)
	{
		// 任意のステータスを弱化
		m_status[(int)type] -= point;
		// スキルポイントを追加
		m_status[(int)StatusType.SkillPoint]++;
	}

	// ステータス取得用
	public int GetStatus(StatusType type)
	{
		return m_status[(int)type];
	}
}
