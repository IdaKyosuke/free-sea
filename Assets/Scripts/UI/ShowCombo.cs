using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowCombo : MonoBehaviour
{
	[SerializeField] int m_showComboLine = 5;   // �R���{����\�����n�߂�Œᐔ
	[SerializeField] TextMeshProUGUI m_comboText;   // �R���{����\������p
	[SerializeField] GameObject m_comboManager;	// �R���{�����擾����p

    // Start is called before the first frame update
    void Start()
    {
		if(!m_comboManager)
		{
			m_comboManager = GameObject.FindWithTag("comboManager");
		}
	}

    // Update is called once per frame
    void Update()
    {
		// �\���̍Œᐔ�𒴂��Ă���Ƃ�
        if(m_comboManager.GetComponent<ComboManager>().GetCombe() >= m_showComboLine)
		{
			m_comboText.SetText("{0} COMBO!!", m_comboManager.GetComponent<ComboManager>().GetCombe());
		}
		else
		{
			m_comboText.SetText("");
		}
    }
}
