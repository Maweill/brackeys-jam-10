using UnityEngine;

namespace House_Scripts
{
	public class BlockLauncher : MonoBehaviour
	{
		[SerializeField] private float _maxAngle;
		[SerializeField] private float _ropeLength;
		[SerializeField] private float _swingSpeed = 1.0f; // Добавлена новая переменная для регулирования скорости

		private readonly Vector3 _ropeSplitPoint = new(0, 1, 0);

		private BlockFactory _blockFactory;
		private Vector3 _blockPosition;
		private Vector3 _blockRelativeStartPosition;

		private Block _currentBlock;

		private LineRenderer _mainRope;
		private LineRenderer _sideRope1;
		private LineRenderer _sideRope2;
		
		private bool _canDropBlocks;
		private bool _levelFailed;

		private void Awake()
		{
			_blockFactory = GetComponent<BlockFactory>();
			LineRenderer[] _lineRenderers = GetComponentsInChildren<LineRenderer>();

			_mainRope = _lineRenderers[0];
			_sideRope1 = _lineRenderers[1];
			_sideRope2 = _lineRenderers[2];
			_blockRelativeStartPosition = transform.position - new Vector3(0f, _ropeLength, 0f);
			gameObject.SetActive(false);
		}
		
		private void Update()
		{
			MoveBlock();
			UpdateRope();

			if (Input.GetKeyDown(KeyCode.Space) && _currentBlock != null && _canDropBlocks) {
				DropBlock();
			}
		}

		private void OnEnable()
		{
			GameEvents.BlockTemplateFilled += OnBlockTemplateFilled;
			GameEvents.CameraMoved += OnCameraMoved;
			GameEvents.LevelFailed += OnLevelFailed;
			GameEvents.LevelInitialized += OnLevelInitialized;
			_currentBlock = _blockFactory.CreateBlock(_blockRelativeStartPosition);
		}
		
		private void OnDisable()
		{
			GameEvents.LevelInitialized -= OnLevelInitialized;
			GameEvents.BlockTemplateFilled -= OnBlockTemplateFilled;
			GameEvents.CameraMoved -= OnCameraMoved;
			_blockFactory.ResetIndex();
			if (_currentBlock != null) {
				Destroy(_currentBlock.gameObject);
			}
		}
		
		private void OnLevelInitialized()
		{
			_levelFailed = false;
		}
		
		private void OnLevelFailed()
		{
			_levelFailed = true;
		}
		
		private void OnBlockTemplateFilled(int _, bool templateFilled)
		{
			if (!templateFilled && !_levelFailed) {
				_blockFactory.ResetToPreviousIndex();
			}
			_currentBlock = _blockFactory.CreateBlock(_blockPosition);
		}

		private void OnCameraMoved()
		{
			_canDropBlocks = true;
		}
		
		private void MoveBlock()
		{
			float angle = _maxAngle * Mathf.Sin(_swingSpeed * Mathf.Sqrt(GlobalConstants.BLOCK_GRAVITY / _ropeLength) * Time.time); // Модифицировали формулу, добавив _swingSpeed
			float xMovement = _blockRelativeStartPosition.x + _ropeLength * Mathf.Sin(angle * Mathf.Deg2Rad);

			_blockPosition =
				new Vector3(xMovement,
				            _blockRelativeStartPosition.y - _ropeLength * Mathf.Cos(angle * Mathf.Deg2Rad) + _ropeLength,
				            _blockRelativeStartPosition.z);

			if (_currentBlock == null) {
				return;
			}

			_currentBlock.Move(_blockPosition);
		}

		private void UpdateRope()
		{
			Vector3 mainRopeEnd = _blockPosition + _ropeSplitPoint;
			_mainRope.SetPosition(0, transform.position);
			_mainRope.SetPosition(1, mainRopeEnd);

			_sideRope1.SetPosition(0, mainRopeEnd);
			_sideRope1.SetPosition(1, _blockPosition + new Vector3(-0.4f, 0, 0));

			_sideRope2.SetPosition(0, mainRopeEnd);
			_sideRope2.SetPosition(1, _blockPosition + new Vector3(0.4f, 0, 0));
		}

		private void DropBlock()
		{
			_currentBlock.Drop();
			_currentBlock.LevelFailed = _levelFailed;
			_currentBlock = null;
		}
	}
}
