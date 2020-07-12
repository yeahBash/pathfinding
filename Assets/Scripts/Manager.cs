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
    Walk
}

public enum State
{
    Idle,
    SearchingPath
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
    [SerializeField] private VerticalLayoutGroup _buttonsLayout;
    [SerializeField] private GameObject _buttonPrefab;
    public static Dictionary<EditMode, VisualOfMode> EditModeToVisual {get; private set;}
    //TODO change to private
    public static State CurrentState = State.Idle;
    public static Color CurrentColor {get; private set;}
    private static EditMode _currentEditMode;
    public static EditMode CurrentEditMode 
    {
        get 
        {
            return _currentEditMode;
        } 
        private set 
        {
            _currentEditMode = value;
            ChangeCursorAndColor(value);
        }
    }

    private void Start()
    {
        SetupButtonsAndVisual();

        CurrentEditMode = EditMode.Walk;
    }

    private void SetupButtonsAndVisual()
    {
        EditModeToVisual = new Dictionary<EditMode, VisualOfMode>();

        foreach (ModeProperties mp in _modeProperties)
        {   
            // instantiate buttons in canvas
            SetButtons(mp);

            // setup visual
            EditModeToVisual[mp.mode] = new VisualOfMode(mp.cursorTexture, mp.cellColor);
        }
    }

    private void SetButtons(ModeProperties mp)
    {
        GameObject btnGo = Instantiate(_buttonPrefab);
        btnGo.transform.SetParent(_buttonsLayout.transform);
        btnGo.name = mp.mode.ToString();

        Button btn = btnGo.GetComponent<Button>();
        Image img = btnGo.GetComponent<Image>();

        btn.onClick.AddListener(() => CurrentEditMode = mp.mode);
        img.sprite = mp.buttonSprite;
    }

    private static void ChangeCursorAndColor(EditMode md)
    {
        Cursor.SetCursor(EditModeToVisual[md].texture, Vector3.zero, CursorMode.Auto);
        CurrentColor = EditModeToVisual[md].color;
        Debug.Log(CurrentEditMode);
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
