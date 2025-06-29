using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadNextScene : MonoBehaviour
{
	[SerializeField] string m_nextScene;
	[SerializeField] float m_fadeTime;
	[SerializeField] UnityEngine.UI.Image m_black;
	[SerializeField] AudioSource m_click;

	private bool m_isClick;

    // Start is called before the first frame update
    void Start()
    {
		// 最初は透明
		m_black.color = new Color(m_black.color.r, m_black.color.g, m_black.color.b, 0);
		m_isClick = false;

	}

    // Update is called once per frame
    void Update()
    {
        if(!m_isClick && Input.anyKeyDown)
		{
			m_isClick = true;
			m_click.Play();
			StartCoroutine(LoadNext());
		}
    }

	private void FixedUpdate()
	{
		
	}

	// シーン遷移
	public IEnumerator LoadNext()
	{
		// フェードイン
		yield return StartCoroutine(FadeManager.Fade(m_black, m_fadeTime));
		// シーン遷移
		SceneManager.LoadScene(m_nextScene);
	}
}
