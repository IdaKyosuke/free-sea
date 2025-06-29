using UnityEngine;

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

	// �U����SE
	[SerializeField] AudioSource m_seImpact;
	[SerializeField] AudioSource m_seSlash;

	private float m_angleZ = 98.0f;

	// �@�������̏Ռ�
	public void InstantiateImpact()
	{
		m_seImpact.Play();
		Instantiate(m_impact, m_weapon.transform.position, Quaternion.identity);
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
	}
	// �����ɔ�Ԏa��
	public void InstantiateSlash()
	{
		Instantiate(m_slash, m_apperPoint.transform.position, m_apperPoint.transform.rotation);
	}
}
