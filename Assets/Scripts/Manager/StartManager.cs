using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1�ԍŏ��Ɋm���ɋN������悤�ɐݒ肷��
[DefaultExecutionOrder(-10)]

public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		// �}�E�X�J�[�\�����B��
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }
}
