using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public Dictionary<UnitType, GameObject> unitPrefabs = new Dictionary<UnitType, GameObject>();
    public Dictionary<UnitType, Sprite> unitsSprites = new Dictionary<UnitType, Sprite>();
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
        unitPrefabs.Add(UnitType.scout,(Resources.Load<GameObject>("prefabs/unitgraphics/scout")));
        unitPrefabs.Add(UnitType.worker,(Resources.Load<GameObject>("prefabs/unitgraphics/worker")));
        unitPrefabs.Add(UnitType.tank,(Resources.Load<GameObject>("prefabs/unitgraphics/tank")));
        unitPrefabs.Add(UnitType.sniper,(Resources.Load<GameObject>("prefabs/unitgraphics/sniper")));
        unitPrefabs.Add(UnitType.soldier,(Resources.Load<GameObject>("prefabs/unitgraphics/soldier")));

        Sprite[] soldier = Resources.LoadAll<Sprite>("sprites/units/soldier");
        Sprite[] worker = Resources.LoadAll<Sprite>("sprites/units/worker");
        Sprite[] tank = Resources.LoadAll<Sprite>("sprites/units/tank");
        Sprite[] sniper = Resources.LoadAll<Sprite>("sprites/units/sniper");
        Sprite[] scout = Resources.LoadAll<Sprite>("sprites/units/scout");


        unitsSprites.Add(UnitType.scout, scout[0]);
        unitsSprites.Add(UnitType.worker, worker[0]);
        unitsSprites.Add(UnitType.tank, tank[0]);
        unitsSprites.Add(UnitType.sniper, sniper[0]);
        unitsSprites.Add(UnitType.soldier, soldier[0]);


    }
    public void LoadActionSprites()
    {
        iconSprites.Add(Icon.split, (Resources.Load<Sprite>("sprites/actions/split")));
        iconSprites.Add(Icon.wait, (Resources.Load<Sprite>("sprites/actions/wait")));
        iconSprites.Add(Icon.attack, (Resources.Load<Sprite>("sprites/actions/attack")));
        iconSprites.Add(Icon.eat, (Resources.Load<Sprite>("sprites/actions/eat")));
        
    }
}


