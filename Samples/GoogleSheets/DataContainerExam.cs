using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DevBoost.DataTool;

[CreateAssetMenu(fileName = "DataContainer", menuName = "Custom/DataContainer")]
public class DataContainerExam : DataContainerBase
{
    [PageName("GameData",0)]
    public List<GameData> gameData;

    [PageName("Test", 1725374887)]
    public List<ExampleData2> Test1Data;

    [PageName("Test2", 471142041)]
    public List<ExampleData2> Test2Data;
}

[System.Serializable]
public class GameData
{
    public string Id;

    public float SomeFloat;
    public int SomeInt;
    public string[] SomeString;
    public GameState gameState;
    public GameState[] allState;
}

[System.Serializable]
public class ExampleData2
{
    public string Id;

    public float SomeFloat;
    public int SomeInt;
    public string[] SomeString;
}

public enum GameState
{
    None,
    Start,
    Play
}