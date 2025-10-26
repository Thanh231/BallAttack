using System;
using UnityEngine;

public class EventManager
{
    public static Action StartGame;
    public static Action ResetGame;
    public static Action CleanObject;
    public static Action<GameObject> ReturnObjectToPool;
    public static Action ShowMenu;
    public static Action<string> ShowTutorial;
    public static Action<int> IncreasePoint;
}
