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

    public Dictionary<TileType, Sprite> tileSprites = new Dictionary<TileType, Sprite>();
    public Dictionary<int, Sprite> unitSprites = new Dictionary<int, Sprite>();

    public void LoadSprites()
    {
        LoadTileSprites();
    }
    public void LoadTileSprites()
    {
        tileSprites.Add(TileType.plain, (Resources.Load<Sprite>("sprites/prototype/plain")));
        tileSprites.Add(TileType.forest, (Resources.Load<Sprite>("sprites/prototype/forest")));
        tileSprites.Add(TileType.mountain, (Resources.Load<Sprite>("sprites/prototype/mountain")));
        tileSprites.Add(TileType.food, (Resources.Load<Sprite>("sprites/prototype/forest")));
    }
    public void LoadUnitSprites()
    {
        
    }
}


