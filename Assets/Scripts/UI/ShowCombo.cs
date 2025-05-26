using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowCombo : MonoBehaviour
{
	[SerializeField] int m_showComboLine = 5;   // コンボ数を表示し始める最低数
	[SerializeField] TextMeshProUGUI m_comboText;   // コンボ数を表示する用
	[SerializeField] GameObject m_comboManager;	// コンボ数を取得する用

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
		// 表示の最低数を超えているとき
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
