using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollider : MonoBehaviour
{
	private bool m_isGetHit;
    // Start is called before the first frame update
    void Start()
    {
		m_isGetHit = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		
	}

	private void OnTriggerExit(Collider other)
	{
		
	}

	// 敵の攻撃が当たったか取得
	public bool GetIsHitFlg()
	{
		return m_isGetHit;
	}
}
