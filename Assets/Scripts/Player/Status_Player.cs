using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Status_Player : MonoBehaviour
{
	// プレイヤーのステータスが入ったScriptableObject
	[SerializeField] Status m_status;
	[SerializeField] int m_needExp = 10;
	[SerializeField] int m_getSkillPoint = 2;	// レベルアップでもらえるスキルポイント
	private GameObject m_demon;	// 契約中の悪魔

	private Status.StatusType m_statusType;
	private int m_currentExp;   // 現在溜まっている経験値
	private int m_nextExp;  // 次のレベルに必要な経験値
	private float[] m_statusValue = new float[(int)Status.StatusType.Length - 1];	// ステータスの実数値の配列

	// Start is called before the first frame update
	void Start()
    {
		m_status.Initialize();
		m_currentExp = 0;
		m_nextExp = m_needExp;
		if(!m_demon)
		{
			m_demon = GameObject.FindWithTag("demon_blue");
		}
	}

	private void Update()
	{
		// ステータスの実数値を計算する
		for(int i = 0; i < (int)Status.StatusType.Length - 1; i++)
		{
			m_statusValue[i] = 
				GetStatus((Status.StatusType)i) * m_demon.GetComponent<Move_Demon>().GetMag((Status.StatusType)i);
		}
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
				// スキルポイントの加算
				GetSkillPoint();
			} while (m_currentExp >= m_nextExp);
		}
	}

	// スキルポイントの加算
	private void GetSkillPoint()
	{
		// 悪魔の好む能力にいったん割り振られる
		m_status.AddSkillPoint(m_demon.GetComponent<Move_Demon>().GetDemonType(), m_getSkillPoint);
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

	// ステータス取得（レベル）
	public int GetStatus(Status.StatusType type)
	{
		int value = m_status.GetStatus(type);
		return value;
	}

	// ステータスの実数値を取得
	public float GetStatusValue(Status.StatusType type)
	{
		return m_statusValue[(int)type];
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

	// 悪魔と契約する
	public void SetDemon(GameObject demon)
	{
		m_demon = demon;
	}

	//契約中の悪魔が何かを取得
	public GameObject GetDemon()
	{
		return m_demon;
	}
}
