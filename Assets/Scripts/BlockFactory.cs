using UnityEngine;

public class BlockFactory : MonoBehaviour
{
	[SerializeField] private Rigidbody2D _blockPrefab;

	public Rigidbody2D CreateBlock(Vector3 startPosition)
	{
		Rigidbody2D block = Instantiate(_blockPrefab, startPosition, Quaternion.identity);
		block.isKinematic = true;
		return block;
	}
}
