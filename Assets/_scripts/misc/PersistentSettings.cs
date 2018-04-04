using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentSettings {

    public static Dictionary<Team, Color> teamColors = new Dictionary<Team, Color>()
    {
        { Team.player1, Color.red },
        { Team.player2, Color.blue }
    };

    private static float _volume = 0.5f;
    private static float _dragSpeed = 25;
    private static float _dragSensitivity = 0.5f;
    private static float _zoomSpeed = 50;
    private static bool _useGrid = true;
    private static float _maxZoom = 3;
    private static float _minZoom = 7;

    public static float volume
    {
        get { return _volume; }
    }
    public static float dragSpeed
    {
        get { return _dragSpeed * _dragSensitivity; }
    }
    public static float zoomSpeed
    {
        get { return _zoomSpeed; }
    }
    public static bool useGrid
    {
        get { return _useGrid; }
    }
    public static float maxZoom
    {
        get { return _maxZoom; }
    }
    public static float minZoom
    {
        get { return _minZoom; }
    }


    public static void SetVolume(float value)
    {
        _volume = value;
        Debug.Log("Volume:" + _volume);
    }
    public static void SetGridLines(bool value)
    {
        _useGrid = value;
        Debug.Log("UseGrid?:" + _useGrid);
    }
    public static void SetDragSensitivity(float value)
    {
        _dragSensitivity = value;
        Debug.Log("Drag:" + _dragSensitivity);
    }


}
