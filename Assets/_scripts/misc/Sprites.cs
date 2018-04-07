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

    private string tileSpritesPath = "sprites/tiles/Tile_Sprites";
    private string tileSpriteFoodPath = "sprites/tiles/food";
    private string prefabScoutPath = "prefabs/unitgraphics/scout";
    private string prefabSoldierPath = "prefabs/unitgraphics/soldier";
    private string prefabTankPath = "prefabs/unitgraphics/tank";
    private string prefabWorkerPath = "prefabs/unitgraphics/worker";
    private string prefabSniperPath = "prefabs/unitgraphics/sniper";
    
    private string spriteSoldierPath = "sprites/units/infantry_idle";
    private string spriteWorkerPath = "sprites/units/miner_idle";
    private string spriteTankPath = "sprites/units/tank_idle";
    private string spriteSniperPath = "sprites/units/sniper_idle";
    private string spriteScoutPath = "sprites/units/scout_idle";
    


    public void LoadSprites()
    {
        LoadTileSprites();
        LoadUnitSprites();
        loaded = true;
    }
    public void LoadTileSprites()
    {
        Sprite[] tiles = Resources.LoadAll<Sprite>(tileSpritesPath);

        tileSprites.Add(TileType.grassCorner, tiles[0]);
        tileSprites.Add(TileType.grassEdge, tiles[1]);
        tileSprites.Add(TileType.grassMid, tiles[3]);
        tileSprites.Add(TileType.wall, tiles[4]);
        tileSprites.Add(TileType.food, (Resources.Load<Sprite>(tileSpriteFoodPath))); 
        
    }
    public void LoadUnitSprites()
    {
        unitPrefabs.Add(UnitType.scout,(Resources.Load<GameObject>(prefabScoutPath)));
        unitPrefabs.Add(UnitType.worker,(Resources.Load<GameObject>(prefabWorkerPath)));
        unitPrefabs.Add(UnitType.tank,(Resources.Load<GameObject>(prefabTankPath)));
        unitPrefabs.Add(UnitType.sniper,(Resources.Load<GameObject>(prefabSniperPath)));
        unitPrefabs.Add(UnitType.soldier,(Resources.Load<GameObject>(prefabSoldierPath)));

        Sprite[] soldier = Resources.LoadAll<Sprite>(spriteSoldierPath);
        Sprite[] worker = Resources.LoadAll<Sprite>(spriteWorkerPath);
        Sprite[] tank = Resources.LoadAll<Sprite>(spriteTankPath);
        Sprite[] sniper = Resources.LoadAll<Sprite>(spriteSniperPath);
        Sprite[] scout = Resources.LoadAll<Sprite>(spriteScoutPath);

        unitsSprites.Add(UnitType.scout, scout[0]);
        unitsSprites.Add(UnitType.worker, worker[0]);
        unitsSprites.Add(UnitType.tank, tank[0]);
        unitsSprites.Add(UnitType.sniper, sniper[0]);
        unitsSprites.Add(UnitType.soldier, soldier[0]);


    }
}


