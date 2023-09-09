using System;
using House_Scripts;

public static class GameEvents
{
	public static event Action GameStarted;
	public static event Action LevelInitialized;
	public static event Action LevelCompleted;
	public static event Action LevelFailed;
	public static event Action GameCompleted;
	public static event Action TutorialSkipped; 
	public static event Action<Block> BlockSpawned; // блок появился
	public static event Action<Block> BlockDropped; // блок отпущен
	public static event Action<Block> BlockPlaced; // блок приземлился
	public static event Action BlocksEnded; // блоки закончились
	public static event Action<int, bool> BlockTemplateFilled;
	
	public static event Action CameraMoved;
	
	public static void InvokeGameStarted()
	{
		GameStarted?.Invoke();
	}
	
	public static void InvokeGameCompleted()
	{
		GameCompleted?.Invoke();
	}
	
	public static void InvokeLevelInitialized()
	{
		LevelInitialized?.Invoke();
	}
	
	public static void InvokeLevelCompleted()
	{
		LevelCompleted?.Invoke();
	}
	
	public static void InvokeLevelFailed()
	{
		LevelFailed?.Invoke();
	}

	public static void InvokeTutorialSkipped()
	{
		TutorialSkipped?.Invoke();
	}
	
	public static void InvokeBlockSpawned(Block block)
	{
		BlockSpawned?.Invoke(block);
	}
	
	public static void InvokeBlockDropped(Block block)
	{
		BlockDropped?.Invoke(block);
	}
	
	public static void InvokeBlockPlaced(Block block)
	{
		BlockPlaced?.Invoke(block);
	}
	
	public static void InvokeBlocksEnded()
	{
		BlocksEnded?.Invoke();
	}

	public static void InvokeBlockTemplateFilled(int id, bool filled)
	{
		BlockTemplateFilled?.Invoke(id, filled);
	}
	
	public static void InvokeCameraMoved()
	{
		CameraMoved?.Invoke();
	}
}
