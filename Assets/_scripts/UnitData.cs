using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable] 
public class UnitData : Data
{
    public UnitType id;
    public string name;
    public int health;
    public int speed;
    public bool canWalkMountains;
}


public enum UnitType
{
    infantry = 0,
    tank = 1
}