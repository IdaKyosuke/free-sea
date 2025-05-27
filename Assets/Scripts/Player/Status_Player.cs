using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Status_Player : MonoBehaviour
{
	// ScriptableObject
	[SerializeField] Status m_status;
	//private int m_devil;
	[SerializeField] int m_needExp = 10;

	private Status.StatusType m_statusType;
	private int m_currentExp;   // 現在溜まっている経験値
	private int m_nextExp;  // 次のレベルに必要な経験値

	// Start is called before the first frame update
	void Start()
    {
		m_status.Initialize();
		m_currentExp = 0;
		m_nextExp = m_needExp;
	}

	// 経験値の加算 -> レベルアップ
	public void AddExp(int exp)
	{
		m_currentExp += exp;

		// レベルアップ処理
		if (m_currentExp >= m_nextExp)
		{
			// 上がる分はまとめてあげる
			do
			{
				// あふれた分の経験値は持ち越し
				m_currentExp -= m_nextExp;
				// 次のレベルに必要な経験値を増やす
				m_nextExp = (int)(m_nextExp * 1.3f);
				// レベルアップ
				m_status.LvUp();
			} while (m_currentExp >= m_nextExp);
		}
	}

	// ダメージ処理
	public int GetHit()
	{
		int damage = m_status.GetStatus(Status.StatusType.Atk);
		return damage;
	}

	// レベルを取得
	public int GetLv() 
	{
		int l = m_status.GetLv();
		return l;
	}

	// ステータス取得
	public int GetStatus(Status.StatusType type)
	{
		int value = m_status.GetStatus(type);
		return value;
	}

	// ---- ステータス変更 ----
	// 割り振り
	public void SetStatus(Status.StatusType type, int point)
	{
		m_status.UseSkillPoint(type, point);
	}

	// 振り直し
	public void ResetStatus(Status.StatusType type, int point)
	{
		m_status.ResetSkillPoint(type, point);
	}
}
