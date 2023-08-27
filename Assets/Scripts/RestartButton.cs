using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton: MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnMouseUpAsButton()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void OnMouseEnter()
	{
		_spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f);
	}

	private void OnMouseExit()
	{
		_spriteRenderer.color = Color.white;
	}
}
