using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
	private GameObject m_boss;
	private Enemy_Status m_bossStatus;
	[SerializeField] TextMeshProUGUI m_name;
	[SerializeField] Image m_gauge;
	private int m_maxHp;

	// Start is called before the first frame update
	void Awake()
    {
		m_maxHp = 0;
	}

    // Update is called once per frame
    void Update()
    {
		if(m_boss)
		{
			// 現在のhpを取得
			float currentValue = m_boss.GetComponent<Move_Enemy>().GetCurrentHp();
			// ゲージの更新
			m_gauge.fillAmount = currentValue / m_maxHp;
		}
	}

	public void SetBoss(GameObject boss)
	{
		// ボスのゲームオブジェクトを設定
		m_boss = boss;
		// ボスのステータスを取得
		m_bossStatus = boss.GetComponent<Move_Enemy>().GetStatus();
		// ボスの名前を更新
		m_name.SetText(m_bossStatus.name);
		// HPの最大値を設定
		m_maxHp = m_bossStatus.GetHp();
	}
}
