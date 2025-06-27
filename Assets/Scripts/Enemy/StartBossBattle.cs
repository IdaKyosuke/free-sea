using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class StartBossBattle : MonoBehaviour
{
	[SerializeField] GameObject m_bossEnemy;    // ボスとして登場させたい敵
	[SerializeField] GameObject m_areaWall;   // ボスと戦うためのエリアを区切る壁
	[SerializeField] GameObject m_bossUI;       // ボスの名前や体力

	[SerializeField] float m_waitTime = 1.0f;  // UIや壁が消えるまでの時間
	private float m_duration;	// 経過時間の計測用

	// 敵が生きているかのフラグ
	private bool m_isArrive;

    // Start is called before the first frame update
    void Start()
    {
		// ボスとして登場する敵を設定
		m_bossUI.GetComponent<BossUI>().SetBoss(m_bossEnemy);
		// 戦闘が始まるまで隠しておく
		m_bossUI.SetActive(false);
		// 壁を非アクティブにする
		m_areaWall.SetActive(false);
		m_isArrive = true;
		m_duration = 0;
	}

    // Update is called once per frame
    void Update()
    {
		if (!m_isArrive) return;

		if(m_bossEnemy.GetComponent<Move_Enemy>().GetCurrentHp() <= 0)
		{
			m_duration += Time.deltaTime;
			if(m_duration >= m_waitTime)
			{
				DeathEnemy();
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!m_isArrive) return;

		// プレイヤーが範囲内に入ったら戦闘を始める
		if (other.gameObject.CompareTag("Player"))
		{
			// ボスが動き始める
			m_bossEnemy.GetComponent<Move_Enemy>().Combat();
			// 壁をセットする
			m_areaWall.SetActive(true);
			// 名前や体力を表示する
			m_bossUI.SetActive(true);
		}
	}

	public void DeathEnemy()
	{
		m_isArrive = false;
		// 壁をセットする
		m_areaWall.SetActive(false);
		// 名前や体力を表示する
		m_bossUI.SetActive(false);
	}
}
