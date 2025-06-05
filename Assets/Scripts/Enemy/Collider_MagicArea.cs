using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider_MagicArea : MonoBehaviour
{
	private bool m_closePlayer;	// �v���C���[�����@���g���Ȃ��͈͓��ɂ��邩

    // Start is called before the first frame update
    void Start()
    {
		m_closePlayer = false;
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			// �v���C���[�Ƃ̋������߂����鎞
			m_closePlayer = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			// �v���C���[�Ƃ̋��������ȏ�󂢂Ă��鎞
			m_closePlayer = false;
		}
	}

	// �v���C���[�Ƃ̋������߂����邩�ǂ���
	public bool ClosePlayer()
	{
		return m_closePlayer;
	}
}
