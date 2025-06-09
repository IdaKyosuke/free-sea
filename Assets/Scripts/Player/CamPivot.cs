using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPivot : MonoBehaviour
{
	[SerializeField] GameObject m_player;
	[SerializeField] float m_addY = 1.6f;

    // Start is called before the first frame update
    void Start()
    {
		if(!m_player)
		{
			m_player = GameObject.FindWithTag("Player");
		}
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 pos = m_player.transform.position;
		pos.y += m_addY;
        transform.position = pos;
    }
}
