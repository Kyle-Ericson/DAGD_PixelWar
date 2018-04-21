using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentSettings {

    public static Dictionary<Team, Color> teamColors = new Dictionary<Team, Color>()
    {
        { Team.player1, Color.red },
        { Team.player2, Color.blue }
    };

    public static  GameMode gameMode;
    public static string gameKey = "";
    public static bool isHost = false;
    public static bool isPrivate = false;
    private static float _musicVolume = 0.5f;
    private static float _sfxVolume = 0.5f;
    private static float _dragSpeed = 25;
    private static float _dragSensitivity = 0.5f;
    private static float _zoomSpeed = 50;
    private static bool _useGrid = true;
    private static float _maxZoom = 3;
    private static float _minZoom = 7;

    public static float musicVolume
    {
        get { return _musicVolume; }
    }
    public static float sfxVolume
    {
        get { return _sfxVolume; }
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


    public static void SetMusicVolume(float value)
    {
        _musicVolume = value;
        SoundManager.ins.SetMusicVolume();
        Debug.Log("Music Volume:" + _musicVolume);
    }
    public static void SetSFXVolume(float value)
    {
        _sfxVolume = value;
        SoundManager.ins.SetSFXVolume();
        Debug.Log("SFX Volume:" + _sfxVolume);
    }
    public static void SetGridLines(bool value)
    {
        _useGrid = value;
        if(_useGrid) GameScene.ins.ShowLines();
        else GameScene.ins.HideLines();
    }
    public static void SetDragSensitivity(float value)
    {
        _dragSensitivity = value;
        Debug.Log("Drag:" + _dragSensitivity);
    }
}
