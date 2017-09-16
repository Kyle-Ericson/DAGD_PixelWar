using System.Collections;
using System.Collections.Generic;
using UnityEngine;





// library for loading map data from json
[System.Serializable] public class UnitCollection
{
    // list of maps from maps.json
    public List<UnitItem> unitItems = new List<UnitItem>();
}
