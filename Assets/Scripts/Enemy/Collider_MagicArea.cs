using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider_MagicArea : MonoBehaviour
{
	private bool m_closePlayer;	// プレイヤーが魔法を使えない範囲内にいるか

    // Start is called before the first frame update
    void Start()
    {
		m_closePlayer = false;
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			// プレイヤーとの距離が近すぎる時
			m_closePlayer = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			// プレイヤーとの距離が一定以上空いている時
			m_closePlayer = false;
		}
	}

	// プレイヤーとの距離が近すぎるかどうか
	public bool ClosePlayer()
	{
		return m_closePlayer;
	}
}
