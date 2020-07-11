using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum Mode
{
    Idle,
    Start,
    Finish,
    Obstacle,
    NonObstacle
}

public class Manager : MonoBehaviour
{
    [SerializeField] private ModeProperties[] _modeProperties;
    private static ModeProperties[] _modePropertiesStatic;
    [SerializeField] private VerticalLayoutGroup _buttonsLayout;
    [SerializeField] private GameObject _buttonPrefab;
    private static Mode _currentMode;
    public static Color CurrentColor {get; private set;}
    public static Mode CurrentMode 
    {
        get 
        {
            return _currentMode;
        } 
        private set 
        {
            _currentMode = value;
            ChangeCursorAndColor(value);
        }
    }

    public BattleField battleField;
    public Astar astar;

    private void Start()
    {
        _modePropertiesStatic = new ModeProperties[_modeProperties.Length];
        _modeProperties.CopyTo(_modePropertiesStatic, 0);
        SetButtons();
        CurrentMode = Mode.Idle;
    }

    private void SetButtons()
    {
        foreach (ModeProperties mp in _modeProperties)
        {   if (mp.mode != Mode.Idle)
            {
                GameObject btnGo = Instantiate(_buttonPrefab);
                btnGo.transform.SetParent(_buttonsLayout.transform);
                btnGo.name = mp.mode.ToString();

                Button btn = btnGo.GetComponent<Button>();
                Image img = btnGo.GetComponent<Image>();

                btn.onClick.AddListener(() => CurrentMode = mp.mode);
                img.sprite = mp.buttonSprite;    
            }
        }
    }

    private static void ChangeCursorAndColor(Mode md)
    {
        ModeProperties modeProperties = Array.Find(_modePropertiesStatic, (x) => x.mode == md);
        Cursor.SetCursor(modeProperties.cursorTexture, Vector3.zero, CursorMode.Auto);
        CurrentColor = modeProperties.cellColor;
    }

    void OnValidate()
    {
        int numberOfModes = Enum.GetNames(typeof(Mode)).Length;
        if (_modeProperties.Length != numberOfModes)
        {
            Array.Resize(ref _modeProperties, numberOfModes);
            throw new Exception("Number of mode properties must be equal to number of modes!");
        }
    }

}
