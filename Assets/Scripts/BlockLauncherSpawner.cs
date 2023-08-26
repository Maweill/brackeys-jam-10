using House_Scripts;
using UnityEngine;

public class BlockLauncherSpawner : MonoBehaviour
{
    [SerializeField]
    private BlockLauncher _blockLauncher;
    [SerializeField]
    private float _currentSpawnPositionY;

    private void OnEnable()
    {
        GameEvents.GameStarted += SpawnBaseBlockLauncher;
        GameEvents.LevelCompleted += SpawnBlockLauncher;
    }

    private void OnDisable()
    {
        GameEvents.GameStarted -= SpawnBaseBlockLauncher;
        GameEvents.LevelCompleted -= SpawnBlockLauncher;
    }

    private void SpawnBaseBlockLauncher()
    {
        _currentSpawnPositionY = -5;
        Vector3 spawnPosition = new(0, _currentSpawnPositionY, 0);
        Instantiate(_blockLauncher, spawnPosition, Quaternion.identity);
    }

    private void SpawnBlockLauncher()
    {
        //TODO если уровень Х, то спавнить BlockLauncher с N количеством частей домов

        float spawnPositionY = _currentSpawnPositionY - GlobalConstants.LEVEL_SHIFT;
        Vector3 spawnPosition = new(0, spawnPositionY, 0);
        Instantiate(_blockLauncher, spawnPosition, Quaternion.identity);

        _currentSpawnPositionY = spawnPositionY;
    }
}
