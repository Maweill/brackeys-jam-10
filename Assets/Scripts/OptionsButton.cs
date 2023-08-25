using UnityEngine;

public class OptionsButton: MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnMouseUpAsButton()
	{
		//TODO show options menu
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
