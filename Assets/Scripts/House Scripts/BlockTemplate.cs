using System.Collections;
using UnityEngine;

namespace House_Scripts
{
	public class BlockTemplate : MonoBehaviour
	{
		[SerializeField] private int _blockTemplateId;
		[SerializeField] private Sprite _selectedSprite;
		[SerializeField] private AudioClip _fillSound;
		[SerializeField] private AudioClip _missSound;
		
		private Block _block;
		private SpriteRenderer _spriteRenderer;
		private AudioSource _audioSource;
		private Color _initialColor;

		public bool IsFilled { get; private set; }
		
		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_audioSource = GetComponent<AudioSource>();
			_initialColor = _spriteRenderer.color;
		}

		private void OnEnable()
		{
			GameEvents.BlockSpawned += OnBlockSpawned;
			GameEvents.BlockPlaced += OnBlockPlaced;
		}

		private void OnDisable()
		{
			GameEvents.BlockSpawned -= OnBlockSpawned;
			GameEvents.BlockPlaced -= OnBlockPlaced;
		}

		public void Show()
		{
			gameObject.SetActive(true);
			_spriteRenderer.color = _initialColor;
		}
		
		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void LightenBlock()
		{
			_block.Lighten();
		}
		
		private void OnBlockSpawned(Block block)
		{
			if (_blockTemplateId != block.ID) {
				return;
			}

			SelectAsTarget();
		}

		private void OnBlockPlaced(Block block)
		{
			if (_blockTemplateId != block.ID) {
				return;
			}

			_block = block;
			float distance = Vector2.Distance(transform.position, block.transform.position);
			float maxSize = Mathf.Max(transform.localScale.x, block.transform.localScale.x);
			float distancePercentage = Mathf.Clamp01(1 - distance / maxSize);
			float percentage = distancePercentage * 100f;

			IsFilled = percentage >= GlobalConstants.BLOCK_TEMPLATE_FILL_MIN_PERCENTAGE;
			PlaySound(IsFilled);
			Debug.Log("BlockTemplate: Расстояние в процентах: " + percentage.ToString("F2") + "%");
			StartCoroutine(ShowFillResult(IsFilled));
		}
		
		private void SelectAsTarget()
		{
			Color color = _spriteRenderer.color;
			color.a = 0.8f;
			_spriteRenderer.color = color;
			_spriteRenderer.sprite = _selectedSprite;
			_spriteRenderer.enabled = true;
		}
		
		private IEnumerator ShowFillResult(bool isFilled)
		{
			Color color = _spriteRenderer.color;
			color = isFilled ? Color.green : Color.red;
			color.a = 0.8f;
			_spriteRenderer.color = color;
		
			yield return new WaitForSeconds(0.5f);

			if (isFilled) {
				gameObject.SetActive(false);
			}
			else {
				_spriteRenderer.color = _initialColor;
				SelectAsTarget();
			}
			GameEvents.InvokeBlockTemplateFilled(_blockTemplateId, isFilled);
		}
		
		private void PlaySound(bool isFilled)
		{
			_audioSource.clip = isFilled ? _fillSound : _missSound;
			_audioSource.Play();
		}
	}
}
