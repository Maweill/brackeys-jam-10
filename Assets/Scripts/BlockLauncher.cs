using System.Collections;
using UnityEngine;

public class BlockLauncher : MonoBehaviour
{
	[SerializeField] private float _amplitudeX = 3f;
	[SerializeField] private float _amplitudeY = 0.5f;
	[SerializeField] private float _frequency = 2.0f;
	[SerializeField] private Vector3 _startPosition = new(0, 0, 0);

	private BlockFactory _blockFactory;

	private Rigidbody2D _currentBlock;

	private void Awake()
	{
		_blockFactory = GetComponent<BlockFactory>();
	}

	private void Start()
	{
		_currentBlock = _blockFactory.CreateBlock(_startPosition);
	}

	private void Update()
	{
		MoveBlock();

		if (Input.GetKeyDown(KeyCode.Space)) {
			StartCoroutine(DropBlock());
		}
	}

	private void MoveBlock()
	{
		if (_currentBlock == null) {
			return;
		}

		float yMovement = _amplitudeY * Mathf.Sin(_frequency * Time.time);
		float xMovement = _amplitudeX * Mathf.Cos(_frequency * Time.time);

		_currentBlock.transform.position =
			new Vector3(_startPosition.x + xMovement, _startPosition.y + yMovement, _startPosition.z);
	}

	private IEnumerator DropBlock()
	{
		_currentBlock.isKinematic = false;

		// Вычисляем производные
		float yVelocity = _amplitudeY * _frequency * Mathf.Cos(_frequency * Time.time);
		float xVelocity = -_amplitudeX * _frequency * Mathf.Sin(_frequency * Time.time);

		// Устанавливаем начальную скорость для блока
		_currentBlock.velocity = new Vector2(xVelocity, yVelocity);

		_currentBlock = null;
		yield return new WaitForSeconds(1f);
		_currentBlock = _blockFactory.CreateBlock(_startPosition);
	}
}
