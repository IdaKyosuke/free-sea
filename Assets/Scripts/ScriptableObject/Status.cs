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

	// 初期ステータスの配列
	private int[] m_firstStatus = new int[(int)StatusType.Length];

	// ステータスの配列
	private int[] m_status = new int[(int)StatusType.Length];

	// プレイヤー用の関数
	// ステータスの初期化
	public void Initialize()
	{
		// 変更用ステータスを作成
		m_status[(int)StatusType.Hp] = m_hp;
		m_status[(int)StatusType.Atk] = m_atk;
		m_status[(int)StatusType.Def] = m_def;
		m_status[(int)StatusType.Spd] = m_spd;
		m_status[(int)StatusType.SkillPoint] = m_skillPoint;

		// 初期ステータスを配列化
		m_firstStatus[(int)StatusType.Hp] = 1;
		m_firstStatus[(int)StatusType.Atk] = 1;
		m_firstStatus[(int)StatusType.Def] = 1;
		m_firstStatus[(int)StatusType.Spd] = 1;
		m_firstStatus[(int)StatusType.SkillPoint] = 0;
	}

	public void LvUp() { m_lv++; }
	public int GetLv() {  return m_lv; }

	// ---- スキルポイント割り振り用 ----
	public void UseSkillPoint(StatusType type, int point) 
	{
		// 残りスキルポイントが0を下回るときに0までのポイントを使えるよう調整
		if (m_status[(int)StatusType.SkillPoint] - point < 0)
		{
			// 任意のステータスを強化
			m_status[(int)type] += m_status[(int)StatusType.SkillPoint];
			// スキルポイントを0に合わせる
			m_status[(int)StatusType.SkillPoint] = 0;

			return;
		}

		// 任意のステータスを強化
		m_status[(int)type] += point;
		// スキルポイントを消費
		m_status[(int)StatusType.SkillPoint]--; 
	}

	// スキル振り直し用
	public void ResetSkillPoint(StatusType type, int point)
	{
		// 初期値を下回る場合は初期値までしか下げない
		if (m_status[(int)type] - point < m_firstStatus[(int)type])
		{
			// 最低値まで下げた時の余剰ポイントを追加
			int p = m_status[(int)type] - m_firstStatus[(int)type];
			m_status[(int)StatusType.SkillPoint] += p;
			// 現在のステータスを初期値に合わせる
			m_status[(int)type] = m_firstStatus[(int)type];
			return;
		}
		// 任意のステータスを弱化
		m_status[(int)type] -= point;
		// スキルポイントを追加
		m_status[(int)StatusType.SkillPoint] += point;
	}

	// レベルアップでスキルポイントを加算する（契約中の悪魔が好むステータスに自動で割り振られる）
	public void AddSkillPoint(StatusType type, int point)
	{
		m_status[(int)type] += point;
	}

	// ステータス取得用
	public int GetStatus(StatusType type)
	{
		return m_status[(int)type];
	}
}
