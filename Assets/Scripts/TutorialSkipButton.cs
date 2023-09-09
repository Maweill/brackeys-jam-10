using UnityEngine;

public class TutorialSkipButton : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseUpAsButton()
    {
        GameEvents.InvokeTutorialSkipped();
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
