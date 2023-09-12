using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace House_Scripts
{
	public class Block : MonoBehaviour
	{
		[SerializeField]
		private Sprite _lightenSprite;
		[SerializeField]
		private AudioClip _hitSound;
		[SerializeField]
		private AudioClip _lightSwitchSound;

		private Vector3 _blockLastPosition;

		private SpriteRenderer _spriteRenderer;
		private bool _isDisconnectedFromRope;
		private bool _isMassive;
		private AudioSource _audioSource;
		public bool LevelFailed { get; set; }

		public int ID { get; set; }
		public Rigidbody2D Rigidbody { get; private set; }
		
		private void Awake()
		{
			Rigidbody = GetComponent<Rigidbody2D>();
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_audioSource = GetComponent<AudioSource>();
		}

		private void OnEnable()
		{
			GameEvents.BlockTemplateFilled += OnBlockTemplateFilled;
		}

		private void OnDisable()
		{
			GameEvents.BlockTemplateFilled -= OnBlockTemplateFilled;
		}

		private void OnBlockTemplateFilled(int _, bool templateFilled)
		{
			GameEvents.BlockTemplateFilled -= OnBlockTemplateFilled;
			if (!templateFilled && !LevelFailed) {
				Destroy(gameObject);
			}
		}

		private void Update()
		{
			_blockLastPosition = transform.position;
		
			Vector2 currentVelocity = Rigidbody.velocity;
			bool isMoving = currentVelocity.magnitude > 0.0001f;

			if (!_isDisconnectedFromRope || _isMassive || isMoving) {
				return;
			}
			_isMassive = true;
			StartCoroutine(MakeMassive());
		}
		
		public void Initialize(int index)
		{
			ID = index;
		}

		public void Move(Vector3 position)
		{
			transform.position = position;
		}

		public void SnapToPosition(Vector3 position)
		{
			StartCoroutine(SnapToPositionRoutine(position));
		}

		public void Drop()
		{
			Rigidbody.isKinematic = false;
		
			Vector3 positionDifference = transform.position - _blockLastPosition;
			float deltaTime = Time.deltaTime;
			Rigidbody.velocity = new Vector2(positionDifference.x / deltaTime, positionDifference.y / deltaTime);
		
			_isDisconnectedFromRope = true;
			GameEvents.InvokeBlockDropped(this);
		}

		public void Lighten()
		{
			if (_lightenSprite == null) {
				return;
			}
			_spriteRenderer.sprite = _lightenSprite;
			_audioSource.clip = _lightSwitchSound;
			_audioSource.pitch = Random.Range(0.8f, 1.8f);
			_audioSource.Play();
			Debug.Log("Block: Подсветить блок");
		}
		
		private void OnCollisionEnter2D(Collision2D _)
		{
			_audioSource.clip = _hitSound;
			_audioSource.pitch = Random.Range(0.8f, 1.8f);
			_audioSource.Play();
		}
		
		private IEnumerator MakeMassive()
		{
			yield return new WaitForSeconds(0.5f);
			Rigidbody.mass = 100000f;
			GameEvents.InvokeBlockPlaced(this);
		}
		
		private IEnumerator SnapToPositionRoutine(Vector3 position)
		{
			float snapDuration = 0.2f;
			float elapsedTime = 0f;
			Vector3 initialPosition = transform.position;
			while (elapsedTime < snapDuration) {
				transform.position = Vector3.Lerp(initialPosition, position, elapsedTime / snapDuration);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			transform.position = position;
		}
	}
}
