using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ForMinotaurEffect : MonoBehaviour
{
	// �~�m�^�E���X�̕���
	[SerializeField] GameObject m_weapon;

	// �G�t�F�N�g
	[SerializeField] GameObject m_impact;
	[SerializeField] GameObject m_slash;

	// �a���̊p�x
	[SerializeField] GameObject m_apperPoint;
	[SerializeField] GameObject m_normalPoint;

	private float m_angleZ = 98.0f;

	// �@�������̏Ռ�
	public void InstantiateImpact()
	{
		Instantiate(m_impact, m_weapon.transform.position, m_weapon.transform.rotation);
	}

	// ----- �a�����΂� -----
	// �����U��グ�郂�[�V�����ɕt����
	public void InstantiateSlash_Upper()
	{
		Quaternion rot = Quaternion.identity;
		rot.z = m_angleZ;
		rot.y = transform.eulerAngles.y;
		rot.x = transform.eulerAngles.x;
		Instantiate(m_slash, m_apperPoint.transform.position, rot);
		Debug.Log(rot);
	}
	// �����ɔ�Ԏa��
	public void InstantiateSlash()
	{
		Instantiate(m_slash, m_apperPoint.transform.position, m_apperPoint.transform.rotation);
	}
}
