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

	// ----- 悪魔関連 -----
	private GameObject m_demon; // 契約中の悪魔
	[SerializeField] GameObject[] m_demonList;  // 契約可能な悪魔のリスト
	private int m_demonIndex;   // 契約中の悪魔のindex番号
	[SerializeField] GameObject m_changeSmoke;  // 悪魔を変更する時の煙
	[SerializeField] float m_waitChangeTime;    // 悪魔の見た目を変更するまでの時間
	[SerializeField] string m_upKey;	// 悪魔のIndex番号を上げる用
	[SerializeField] string m_downKey;  // 悪魔のIndex番号を下げる用
	private bool m_isDemonChange;		// 現在悪魔を変更中か

	private int m_currentExp;   // 現在溜まっている経験値
	private int m_nextExp;  // 次のレベルに必要な経験値
	private float[] m_statusValue = new float[(int)Status.StatusType.Length - 1];   // ステータスの実数値の配列

	private float m_hp; // 現在の体力
	private bool m_isDeath; // 死んだか
	private bool m_isInvincible;	// 現在無敵時間か

	// Start is called before the first frame update
	void Start()
    {
		m_status.Initialize();
		m_currentExp = 0;
		m_nextExp = m_needExp;
		m_isDeath = false;
		m_demonIndex = 0;
		m_isDemonChange = false;
		if (!m_demon)
		{
			m_demon = GameObject.FindWithTag("demon_blue");
		}
		// ステータスの実数値を計算する
		for (int i = 0; i < (int)Status.StatusType.Length - 1; i++)
		{
			m_statusValue[i] =
				GetStatus((Status.StatusType)i) * m_demon.GetComponent<Demon_Status>().GetMag((Status.StatusType)i);
		}
		// hpを初期化
		m_hp = m_statusValue[(int)Status.StatusType.Hp];
	}

	private void Update()
	{
		// 死んだか判断
		if(m_hp <= 0)
		{
			m_isDeath = true;
			return;
		}
		// ステータスの実数値を計算する
		for(int i = 0; i < (int)Status.StatusType.Length - 1; i++)
		{
			m_statusValue[i] = 
				GetStatus((Status.StatusType)i) * m_demon.GetComponent<Demon_Status>().GetMag((Status.StatusType)i);


			// hpに関するステータスを下げたとき、現在のhpを下回ったら現在のhpを補正する
			if ((Status.StatusType)i == Status.StatusType.Hp)
			{
				if(m_statusValue[i] < m_hp)
				{
					m_hp = m_statusValue[i];
				}
			}
		}

		// 現在悪魔を変更中でない時
		if(!m_isDemonChange)
		{
			// 悪魔を変更する用のキーが押されたら
			if(Input.GetKeyDown(m_upKey))
			{
				StartCoroutine(ChangeDemon(1));
			}
			else if(Input.GetKeyDown(m_downKey))
			{
				StartCoroutine(ChangeDemon(-1));
			}
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
				// レベルアップエフェクト
				ActiveLevelUpEffect();
			} while (m_currentExp >= m_nextExp);
		}
	}

	// スキルポイントの加算
	private void GetSkillPoint()
	{
		// 悪魔の好む能力にいったん割り振られる
		m_status.AddSkillPoint(m_demon.GetComponent<Demon_Status>().GetDemonType(), m_getSkillPoint);
	}

	// ダメージ処理
	public void GetHit(float damage)
	{
		// 無敵時間中は処理を飛ばす
		if (!m_isInvincible)
		{
			m_hp -= damage;
			if (m_hp <= 0)
			{
				// 最低値を0にする
				m_hp = 0;
			}
		}
	}

	// 攻撃処理
	public int TakeHit()
	{
		int damage = m_status.GetStatus(Status.StatusType.Atk);
		return damage;
	}


	// ----- ステータスを取得 -----
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

	// 現在のHpを取得
	public float CurrentHp()
	{
		return m_hp;
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

	// 死亡フラグを返す
	public bool IsDeath()
	{
		return m_isDeath;
	}

	// 無敵時間かどうか判定する
	public void SetInvincible(bool flg)
	{
		m_isInvincible = flg;
	}

	// 契約中の悪魔を変更する(upkey=>1, downkey=>-1)
	private IEnumerator ChangeDemon(int key)
	{
		// 悪魔を変更中のフラグを立てる
		m_isDemonChange = true;

		// インデックス番号を変更
		m_demonIndex += key;

		// 番号の補正
		if (m_demonList.Length <= m_demonIndex)
		{
			m_demonIndex = 0;
		}
		else if(m_demonIndex < 0)
		{
			m_demonIndex = m_demonList.Length - 1;
		}

		// 変更前に煙を出す
		Instantiate(m_changeSmoke, m_demon.transform.position, Quaternion.EulerAngles(new Vector3(-90, 0, 0)));

		// 指定した時間待つ
		yield return new WaitForSeconds(m_waitChangeTime);

		// 変更前の悪魔を非アクティブに
		m_demon.SetActive(false);

		// 契約する悪魔を変更
		SetDemon(m_demonList[m_demonIndex]);

		// 契約後の悪魔をアクティブに
		m_demon.SetActive(true);

		// 悪魔の変更が終了したのでフラグを折る
		m_isDemonChange = false;

		yield return null;
	}

	// ゲーム終了時に走る関数
	private void OnApplicationQuit()
	{
		// テストで値を反映させる
		m_status.Save();
	}

	// レベルアップ時にエフェクトを出す
	private void ActiveLevelUpEffect()
	{
		GameObject.FindWithTag("Player").GetComponent<Move_Player>().ActiveLevelUpEffect();
	}
}
