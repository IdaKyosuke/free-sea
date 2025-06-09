using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "ScriptableObject/Create Demon")]
public class Demon : ScriptableObject
{
	[SerializeField] string m_name = "null";
	// �������D�ރX�e�[�^�X
	[SerializeField] Status.StatusType m_type = Status.StatusType.Hp;

	// �����Ƃ̌_��ł�����X�e�[�^�X�̔{��
	[SerializeField] float[] m_mag = new float[(int)Status.StatusType.Length - 1];

	public string Name()
	{
		return m_name;
	}

	public new Status.StatusType GetType()
	{
		return m_type;
	}

	// �X�e�[�^�X�̎����l�p�ɔ{�����擾
	public float GetMag(Status.StatusType type)
	{
		// �����z��O���w�肵���Ƃ���-1��Ԃ�
		if ((int)type >= m_mag.Length) return -1;
		return m_mag[(int)type];
	}
}
