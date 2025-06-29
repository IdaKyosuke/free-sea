using UnityEngine;

public class Effect_Player : MonoBehaviour
{
	[SerializeField] GameObject m_player;

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
        transform.position = m_player.transform.position;
    }
}
