using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1番最初に確実に起動するように設定する
[DefaultExecutionOrder(-10)]

public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		// マウスカーソルを隠す
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }
}
