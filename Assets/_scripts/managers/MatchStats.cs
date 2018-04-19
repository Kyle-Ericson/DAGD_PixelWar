using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatchStats {

    public static Team winner = 0;
    public static string mapName;
    public static int mapID;
    public static int spentP1 = 0;
    public static int spentP2 = 0;
    public static int gatheredP1 = 0;
    public static int gatheredP2 = 0;
    public static int maxArmyValueP1 = 0;
    public static int maxArmyValueP2 = 0;
    public static int turnCount = 0;
    public static Dictionary<Team, int> currentFood = new Dictionary<Team, int>();


    public static void ResetAll()
    {
        mapName = "";
        spentP1 = 0;
        spentP2 = 0;
        gatheredP1 = 0;
        gatheredP2 = 0;
        maxArmyValueP1 = 0;
        maxArmyValueP2 = 0;
        turnCount = 0;
        winner = 0;
        currentFood[Team.player1] = 0;
        currentFood[Team.player2] = 0;
    }
    public static void UpdateArmyValue(Team team, int value)
    {
        switch(team)
        {
            case Team.player1:
                if (value > maxArmyValueP1) maxArmyValueP1 = value;
                break;
            case Team.player2:
                if (value > maxArmyValueP2) maxArmyValueP2 = value;
                break;
        }
    }

    public static void UpdateGathered(Team team, int value)
    {
        switch(team)
        {
            case Team.player1:
                gatheredP1 += value;
                break;
            case Team.player2:
                gatheredP2 += value;
                break;
        }
    }

    public static void UpdateSpent(Team team, int value)
    {
        switch (team)
        {
            case Team.player1:
                spentP1 += value;
                break;
            case Team.player2:
                spentP2 += value;
                break;
        }
    }
    public static void UpdateMapName()
    {
        mapName = MapManager.ins.currentMapData.name;
        mapID = MapManager.ins.currentMapData.id;
    }
    public static void UpdateTurnCount(int value)
    {
        turnCount = value;
    }
    public static void UpdateWinner(Team team)
    {
        winner = team;
    }
    public static void AddFood(Team team, int amount)
    {
        currentFood[team] += amount;
    }
    public static void SubtractFood(Team team, int amount)
    {
        currentFood[team] -= amount;
    }



}
