using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Factory : MonoBehaviour
{
	[SerializeField] GameObject m_enemy;    // ��������G�̃I�u�W�F�N�g
	// �����͈͂��w�肷��p�̃I�u�W�F�N�g
	[SerializeField] Transform m_range1;
	[SerializeField] Transform m_range2;
	[SerializeField] float m_enemyNum;	// �G�̐�����

    // Start is called before the first frame update
    void Start()
    {
		// �w��͈͓��̃����_�����W�ɓG���w�萔�z�u����
        for(int i = 0; i < m_enemyNum; i++)
		{
			float x = UnityEngine.Random.Range(m_range1.position.x, m_range2.position.x);
			float z = UnityEngine.Random.Range(m_range1.position.z, m_range2.position.z);

			Instantiate(m_enemy, new Vector3(x, 0, z), Quaternion.identity);
		}
    }
}
