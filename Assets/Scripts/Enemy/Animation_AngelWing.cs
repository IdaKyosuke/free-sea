using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_AngelWing : MonoBehaviour
{
	private Animator m_anim;

    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

	public void IsDeath()
	{
		m_anim.SetTrigger("death");
	}

}
