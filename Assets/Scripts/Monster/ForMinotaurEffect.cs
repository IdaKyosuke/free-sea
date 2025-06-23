using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForMinotaurEffect : MonoBehaviour
{
	// ミノタウロスの武器
	[SerializeField] GameObject m_weapon;

	// エフェクト
	[SerializeField] GameObject m_impact;
	[SerializeField] GameObject m_slash;

	// 叩きつけ時の衝撃
	public void InstantiateImpact()
	{
		Instantiate(m_impact, m_weapon.transform.position, m_weapon.transform.rotation);
	}

	// 斬撃を飛ばす
	public void InstantiateSlash()
	{
		Instantiate(m_slash, m_weapon.transform.position, m_weapon.transform.rotation);
		Debug.Log(0);
	}
}
