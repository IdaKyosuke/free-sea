using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Factory : MonoBehaviour
{
	[SerializeField] GameObject m_enemy;    // 生成する敵のオブジェクト
	// 生成範囲を指定する用のオブジェクト
	[SerializeField] Transform m_range1;
	[SerializeField] Transform m_range2;
	[SerializeField] float m_enemyNum;  // 敵の生成数

	private List<GameObject> m_enemyList = new List<GameObject>();
	private bool m_isCombat;

    // Start is called before the first frame update
    void Start()
    {
		// 指定範囲内のランダム座標に敵を指定数配置する
        for(int i = 0; i < m_enemyNum; i++)
		{
			float x = UnityEngine.Random.Range(m_range1.position.x, m_range2.position.x);
			float z = UnityEngine.Random.Range(m_range1.position.z, m_range2.position.z);

			m_enemyList.Add(Instantiate(m_enemy, new Vector3(x, this.transform.position.y, z), Quaternion.identity));
		}

		this.GetComponent<SphereCollider>().radius =(m_range1.position.x - m_range2.position.x) / 2;
		m_isCombat = false;
	}

	private void Update()
	{
		for(int i = 0; i < m_enemyList.Count; i++)
		{
			if (!m_enemyList[i])
			{
				m_enemyList[i] = null;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (m_isCombat) return;

		if(other.gameObject.CompareTag("Player"))
		{
			m_isCombat = true;
			foreach(var enemy in m_enemyList)
			{
				enemy.GetComponent<Move_Enemy>().Combat();
			}
		}
	}

	
}
