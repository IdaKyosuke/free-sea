using System.Collections;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
	// フェード用関数
    public static IEnumerator Fade(UnityEngine.UI.Image texture, float fadeTime)
	{
		float alpha = texture.color.a;
		float time = 0;
		float targetAlpha = (alpha == 0 ? 1 : 0);

		WaitForSeconds waitTime = new WaitForSeconds(0.000001f);

		while (true)
		{
			time += Time.deltaTime;
			float t = time / fadeTime;
			if (t >= 1.0f)
			{
				texture.color = new Color(texture.color.r, texture.color.g, texture.color.b, targetAlpha);
				break;
			}
			float change = Mathf.Lerp(alpha, targetAlpha, t);
			texture.color = new Color(texture.color.r, texture.color.g, texture.color.b, change);

			Debug.Log(texture.color);

			yield return waitTime;
		}
	}
}
