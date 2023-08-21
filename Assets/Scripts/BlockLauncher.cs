using System.Collections;
using UnityEngine;

public class BlockLauncher : MonoBehaviour
{
	[SerializeField] private float _maxAngle;
	[SerializeField] private float _ropeLength;

	private readonly Vector3 _ropeSplitPoint = new(0, 1, 0);

	private BlockFactory _blockFactory;

	private Rigidbody2D _currentBlock;
	private Vector3 _blockPosition;
	private Vector3 _blockRelativeStartPosition;
	private Vector3 _previousBlockPosition;

	private LineRenderer _mainRope;
	private LineRenderer _sideRope1;
	private LineRenderer _sideRope2;


	private void Awake()
	{
		_blockFactory = GetComponent<BlockFactory>();
		LineRenderer[] _lineRenderers = GetComponentsInChildren<LineRenderer>();

		_mainRope = _lineRenderers[0];
		_sideRope1 = _lineRenderers[1];
		_sideRope2 = _lineRenderers[2];
		_blockRelativeStartPosition = transform.position - new Vector3(0f, _ropeLength, 0f);
	}

	private void Start()
	{
		_currentBlock = _blockFactory.CreateBlock(_blockRelativeStartPosition);
	}

	private void Update()
	{
		if (_currentBlock) {
			_previousBlockPosition = _currentBlock.transform.position;
		}

		MoveBlock();
		UpdateRope();

		if (Input.GetKeyDown(KeyCode.Space)) {
			StartCoroutine(DropBlock());
		}
	}

	private void MoveBlock()
	{
		float angle = _maxAngle * Mathf.Sin(Mathf.Sqrt(9.81f / _ropeLength) * Time.time);
		float xMovement = _blockRelativeStartPosition.x + _ropeLength * Mathf.Sin(angle * Mathf.Deg2Rad);

		_blockPosition =
			new Vector3(xMovement,
			            _blockRelativeStartPosition.y - _ropeLength * Mathf.Cos(angle * Mathf.Deg2Rad) + _ropeLength,
			            _blockRelativeStartPosition.z);

		if (_currentBlock == null) {
			return;
		}

		_currentBlock.transform.position = _blockPosition;
	}

	private void UpdateRope()
	{
		Vector3 mainRopeEnd = _blockPosition + _ropeSplitPoint;
		_mainRope.SetPosition(0, transform.position);
		_mainRope.SetPosition(1, mainRopeEnd);

		_sideRope1.SetPosition(0, mainRopeEnd);
		_sideRope1.SetPosition(1, _blockPosition + new Vector3(-0.5f, 0, 0));

		_sideRope2.SetPosition(0, mainRopeEnd);
		_sideRope2.SetPosition(1, _blockPosition + new Vector3(0.5f, 0, 0));
	}

	private IEnumerator DropBlock()
	{
		_currentBlock.isKinematic = false;

		Vector3 positionDifference = _currentBlock.transform.position - _previousBlockPosition;
		float deltaTime = Time.deltaTime;

		_currentBlock.velocity = new Vector2(positionDifference.x / deltaTime, positionDifference.y / deltaTime);

		_currentBlock = null;
		yield return new WaitForSeconds(1f);
		_currentBlock = _blockFactory.CreateBlock(_blockPosition);
	}
}
