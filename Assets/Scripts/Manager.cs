using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode
{
    Idle,
    AddStart,
    AddFinish,
    AddObstacle,
    RemoveObstacle
}

public class Manager : MonoBehaviour
{
    public Mode currentMode {get; private set;}
    public BattleField battleField;
    public Astar astar;

    public void ChangeMode(int modeInt)
    {
        Mode mode = (Mode) modeInt;
        currentMode = mode;
    }
}
