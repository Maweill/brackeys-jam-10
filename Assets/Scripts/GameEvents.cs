using System;

public static class GameEvents
{
	public static event Action GameStarted;
	public static event Action LevelInitialized;
	public static event Action LevelCompleted;
	public static event Action LevelFailed;

	public static event Action BlockDropped; // блок отпущен
	public static event Action BlockPlaced; // блок приземлился
	public static event Action BlocksEnded; // блоки закончились
	
	public static void InvokeGameStarted()
	{
		GameStarted?.Invoke();
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
	
	public static void InvokeBlockDropped()
	{
		BlockDropped?.Invoke();
	}
	
	public static void InvokeBlockPlaced()
	{
		BlockPlaced?.Invoke();
	}
	
	public static void InvokeBlocksEnded()
	{
		BlocksEnded?.Invoke();
	}
}