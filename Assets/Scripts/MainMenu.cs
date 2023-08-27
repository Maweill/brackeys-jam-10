using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject _newGameButton;
	[SerializeField] private GameObject _restartButton;
	
	[SerializeField] private SpriteRenderer _lightenBackground;
	[SerializeField] private SpriteRenderer _darkenBackground;
	
	[SerializeField] private GameObject _gameOverText;
	[SerializeField] private GameObject _youWonText;
	[SerializeField] private GameObject _thanksForPlayingText;

	public void HideButtons()
	{
		_newGameButton.gameObject.SetActive(false);
		_restartButton.gameObject.SetActive(false);
	}
	
	public IEnumerator LightenBackground()
	{
		_lightenBackground.enabled = true;
		yield return new WaitForSeconds(0.4f);
		_lightenBackground.enabled = false;
		yield return new WaitForSeconds(0.2f);
		_lightenBackground.enabled = true;
		yield return new WaitForSeconds(0.6f);
		_lightenBackground.enabled = false;
		yield return new WaitForSeconds(0.6f);
		_lightenBackground.enabled = true;
		yield return new WaitForSeconds(1f);
		
		_youWonText.SetActive(true);
		_thanksForPlayingText.SetActive(true);
	}
	
	public IEnumerator DarkenBackground(float duration = 1.0f)
	{
		_darkenBackground.color = Color.black;
		float alpha = 0;

		while(alpha < 1)
		{
			alpha += Time.deltaTime / duration;
			_darkenBackground.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
			yield return null;
		}

		_darkenBackground.color = Color.black;
		yield return new WaitForSeconds(1f);
		_restartButton.gameObject.SetActive(true);
		_gameOverText.SetActive(true);
	}
}
