using System.Collections;
using UnityEngine;

namespace House_Scripts
{
	public class Block : MonoBehaviour
	{
		[SerializeField]
		private Sprite _lightenSprite;

		private Vector3 _blockLastPosition;

		private SpriteRenderer _spriteRenderer;
		private bool _isDisconnectedFromRope;
		private bool _isMassive;

		public int ID { get; set; }
		public Rigidbody2D Rigidbody { get; private set; }
		
		private void Awake()
		{
			Rigidbody = GetComponent<Rigidbody2D>();
			_spriteRenderer = GetComponent<SpriteRenderer>();
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

		public void  Drop()
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
			Debug.Log("Block: Подсветить блок");
		}
		
		private IEnumerator MakeMassive()
		{
			yield return new WaitForSeconds(0.5f);
			Rigidbody.mass = 100000f;
			GameEvents.InvokeBlockPlaced(this);
		}
	}
}
