using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveStopCollider : MonoBehaviour
{
	private bool stop = false;  // ÉvÉåÉCÉÑÅ[Ç…Ç‘Ç¬Ç©Ç¡ÇΩÇ©

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			stop = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			stop = false;
		}
	}

	public bool IsStop()
	{
		return stop;
	}
}
