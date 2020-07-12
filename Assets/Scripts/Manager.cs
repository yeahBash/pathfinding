using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum EditMode
{
    Start,
    Finish,
    Obstacle,
    NonObstacle
}
public struct VisualOfMode
{
    public VisualOfMode(Texture2D tex, Color col)
    {
        texture = tex;
        color = col;
    }

    public Texture2D texture;
    public Color color;
}

public class Manager : MonoBehaviour
{
    [SerializeField] private ModeProperties[] _modeProperties;
    public static Dictionary<EditMode, VisualOfMode> ModeToVisual {get; private set;}

    [SerializeField] private VerticalLayoutGroup _buttonsLayout;
    [SerializeField] private GameObject _buttonPrefab;
    private static EditMode _currentMode;
    public static Color CurrentColor {get; private set;}
    public static EditMode CurrentMode 
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

    private void Start()
    {
        SetupButtonsAndVisual();

        CurrentMode = EditMode.NonObstacle;
    }

    private void SetupButtonsAndVisual()
    {
        ModeToVisual = new Dictionary<EditMode, VisualOfMode>();

        foreach (ModeProperties mp in _modeProperties)
        {   
            // instantiate buttons in canvas
            SetButtons(mp);

            // setup visual
            ModeToVisual[mp.mode] = new VisualOfMode(mp.cursorTexture, mp.cellColor);
        }
    }

    private void SetButtons(ModeProperties mp)
    {
        GameObject btnGo = Instantiate(_buttonPrefab);
        btnGo.transform.SetParent(_buttonsLayout.transform);
        btnGo.name = mp.mode.ToString();

        Button btn = btnGo.GetComponent<Button>();
        Image img = btnGo.GetComponent<Image>();

        btn.onClick.AddListener(() => CurrentMode = mp.mode);
        img.sprite = mp.buttonSprite;
    }

    private static void ChangeCursorAndColor(EditMode md)
    {
        Cursor.SetCursor(ModeToVisual[md].texture, Vector3.zero, CursorMode.Auto);
        CurrentColor = ModeToVisual[md].color;
        Debug.Log(CurrentColor);
        Debug.Log(CurrentMode);
    }

    void OnValidate()
    {
        int numberOfModes = Enum.GetNames(typeof(EditMode)).Length;
        if (_modeProperties.Length != numberOfModes)
        {
            Array.Resize(ref _modeProperties, numberOfModes);
            throw new Exception("Number of mode properties must be equal to number of modes!");
        }
    }

}
