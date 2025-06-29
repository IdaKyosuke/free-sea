using UnityEngine;

public class Item_Health : MonoBehaviour
{
	[SerializeField] GameObject m_playerStatus;
	[SerializeField] float m_rotAngle;		// 1フレームごとの回転量

	[SerializeField] float m_healAmount;	// 回復量

    // Start is called before the first frame update
    void Start()
    {
        if(!m_playerStatus)
		{
			m_playerStatus = GameObject.FindWithTag("playerStatus");
		}
    }

    // Update is called once per frame
    void Update()
    {
		// 毎フレーム回転
		transform.Rotate(0, m_rotAngle, 0);
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			m_playerStatus.GetComponent<Status_Player>().Heal(m_healAmount);
			Destroy(this.gameObject);
		}
	}
}
