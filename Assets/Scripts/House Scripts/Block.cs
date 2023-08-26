using System.Collections;
using UnityEngine;

namespace House_Scripts
{
	public class Block : MonoBehaviour
	{
		public Rigidbody2D Rigidbody { get; private set; }

		private Vector3 _blockLastPosition;

		private bool _isDisconnectedFromRope;
		private bool _isMassive;

		public int ID { get; set; }

		private void Awake()
		{
			Rigidbody = GetComponent<Rigidbody2D>();
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

		private IEnumerator MakeMassive()
		{
			yield return new WaitForSeconds(0.5f);
			Rigidbody.mass = 100000f;
			GameEvents.InvokeBlockPlaced(this);
		}
	}
}
