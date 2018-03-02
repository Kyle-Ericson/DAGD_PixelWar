using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites {

    // singleton
    private static Sprites _ins = null;
    public static Sprites ins
    {
        get
        {
            if (_ins == null)
            {
                _ins = new Sprites();
            }
            return _ins;
        }
    }
    public bool loaded = false;

    public Dictionary<TileType, Sprite> tileSprites = new Dictionary<TileType, Sprite>();
    public Dictionary<UnitType, Sprite> unitSprites = new Dictionary<UnitType, Sprite>();
    public Dictionary<Icon, Sprite> iconSprites = new Dictionary<Icon, Sprite>();

    public void LoadSprites()
    {
        LoadTileSprites();
        LoadActionSprites();
        LoadUnitSprites();
        loaded = true;
    }
    public void LoadTileSprites()
    {
        tileSprites.Add(TileType.open, (Resources.Load<Sprite>("sprites/tiles/open")));
        tileSprites.Add(TileType.food, (Resources.Load<Sprite>("sprites/tiles/food"))); 
        tileSprites.Add(TileType.blocking, (Resources.Load<Sprite>("sprites/tiles/blocking")));
    }
    public void LoadUnitSprites()
    {
        unitSprites.Add(UnitType.scout,(Resources.Load<Sprite>("sprites/units/scout")));
        unitSprites.Add(UnitType.queen,(Resources.Load<Sprite>("sprites/units/queen")));
        unitSprites.Add(UnitType.tank,(Resources.Load<Sprite>("sprites/units/tank")));
        unitSprites.Add(UnitType.sniper,(Resources.Load<Sprite>("sprites/units/sniper")));
        unitSprites.Add(UnitType.infantry,(Resources.Load<Sprite>("sprites/units/infantry")));
    }
    public void LoadActionSprites()
    {
        iconSprites.Add(Icon.split, (Resources.Load<Sprite>("sprites/actions/split")));
        iconSprites.Add(Icon.wait, (Resources.Load<Sprite>("sprites/actions/wait")));
        iconSprites.Add(Icon.attack, (Resources.Load<Sprite>("sprites/actions/attack")));
        iconSprites.Add(Icon.eat, (Resources.Load<Sprite>("sprites/actions/eat")));
        
    }
}


