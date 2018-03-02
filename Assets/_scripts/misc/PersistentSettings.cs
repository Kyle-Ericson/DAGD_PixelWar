using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSettings {

    public static Dictionary<Team, Color> team_colors = new Dictionary<Team, Color>()
    {
        { Team.player1, Color.red * 0.9f },
        { Team.player2, Color.blue * 0.9f }
    };

    public static float volume = 1f;
    public static int drag_speed = 1;
    public static bool use_grid = true;
}
