using UnityEngine;

public class ForMinotaurEffect : MonoBehaviour
{
	// ミノタウロスの武器
	[SerializeField] GameObject m_weapon;

	// エフェクト
	[SerializeField] GameObject m_impact;
	[SerializeField] GameObject m_slash;

	// 斬撃の角度
	[SerializeField] GameObject m_apperPoint;
	[SerializeField] GameObject m_normalPoint;

	// 攻撃のSE
	[SerializeField] AudioSource m_seImpact;
	[SerializeField] AudioSource m_seSlash;

	private float m_angleZ = 98.0f;

	// 叩きつけ時の衝撃
	public void InstantiateImpact()
	{
		m_seImpact.Play();
		Instantiate(m_impact, m_weapon.transform.position, Quaternion.identity);
	}

	// ----- 斬撃を飛ばす -----
	// 武器を振り上げるモーションに付ける
	public void InstantiateSlash_Upper()
	{
		Quaternion rot = Quaternion.identity;
		rot.z = m_angleZ;
		rot.y = transform.eulerAngles.y;
		rot.x = transform.eulerAngles.x;
		Instantiate(m_slash, m_apperPoint.transform.position, rot);
	}
	// 水平に飛ぶ斬撃
	public void InstantiateSlash()
	{
		Instantiate(m_slash, m_apperPoint.transform.position, m_apperPoint.transform.rotation);
	}
}
