using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
	[SerializeField] GameObject m_zoneManager;	// ゲージ加算用
	[SerializeField] float m_comboResetTime = 3.0f; // コンボをリセットするまでの間隔
	private float m_durationTime;	// 時間計測用
	private int m_currentCombo;	// 現在のフレームでのコンボ数
	private int m_pastCombo;	// 1フレーム前のコンボ数

    // Start is called before the first frame update
    void Start()
    {
		if(!m_zoneManager)
		{
			m_zoneManager = GameObject.FindWithTag("zoneGaugeManager");
		}
		m_currentCombo = 0;
		m_pastCombo = 0;
		m_durationTime = 0;
	}

    // Update is called once per frame
    void Update()
    {
		// 前フレームからコンボ数が増えていないとき
        if(m_currentCombo == m_pastCombo)
		{
			m_durationTime += Time.deltaTime;
			// コンボリセット時間を超えたとき
			if(m_durationTime >= m_comboResetTime)
			{
				// コンボをリセット
				m_currentCombo = 0;
				m_pastCombo = 0;
			}
		}
		else
		{
			// 経過時間をリセット
			m_durationTime = 0;
		}

		m_pastCombo = m_currentCombo;
	}

	public void AddCombe()
	{
		m_currentCombo++;
		// ゲージを加算
		m_zoneManager.GetComponent<ZoneGaugeManager>().AddGauge();
	}

	public int GetCombe()
	{
		return m_currentCombo;
	}
}
