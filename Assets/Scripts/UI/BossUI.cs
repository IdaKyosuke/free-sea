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
			// ���݂�hp���擾
			float currentValue = m_boss.GetComponent<Move_Enemy>().GetCurrentHp();
			// �Q�[�W�̍X�V
			m_gauge.fillAmount = currentValue / m_maxHp;
		}
	}

	public void SetBoss(GameObject boss)
	{
		// �{�X�̃Q�[���I�u�W�F�N�g��ݒ�
		m_boss = boss;
		// �{�X�̃X�e�[�^�X���擾
		m_bossStatus = boss.GetComponent<Move_Enemy>().GetStatus();
		// �{�X�̖��O���X�V
		m_name.SetText(m_bossStatus.name);
		// HP�̍ő�l��ݒ�
		m_maxHp = m_bossStatus.GetHp();
	}
}
