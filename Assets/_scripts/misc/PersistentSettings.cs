using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSettings {

    public static Dictionary<Team, Color> teamColors = new Dictionary<Team, Color>()
    {
        { Team.player1, Color.red * 0.9f },
        { Team.player2, Color.blue * 0.9f }
    };

    public static float volume = 1f;
    public static int dragSpeed = 25;
    public static float zoomSpeed = 50;
    public static bool useGrid = true;
    public static float maxZoom = 3;
    public static float minZoom = 7;
}
