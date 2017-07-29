using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

        [HideInInspector] public static GameController instance = null; // singleton of game controller
        private Selector selector; // reference to the selector
        private Dictionary<Vector2, Tile> tiles; // list of tiles of the current map




        void Awake () {
                GameController.instance = this; // singleton gamecontroller instance
                selector = Instantiate(Resources.Load<GameObject>("prefabs/Selector").GetComponent<Selector>()); // get reference to the selector
        }
        void Start () {
                Collections.Load(); // load all the libraries
                LoadMap(2);
        }
        void Update () {
                MoveSelector();
        }
        private void LoadMap(int mapID) {
                tiles = MapLoader.instance.Load(mapID); // spawn the map and load the tiles
        }
        private void RemoveMap() {
                foreach(KeyValuePair<Vector2, Tile> item in tiles) // for each tile...
                {
                        Destroy(item.Value.gameObject); // destroy the game object
                }
                tiles.Clear(); // clear tiles dictionary
        }
        public void MoveSelector () {
                foreach(KeyValuePair<Vector2, Tile> k in tiles) // for each tile...
                {
                        if(k.Value && k.Value.isSelected) // if it is selected...
                        {
                                selector.Move(k.Value.transform.position); // move the selector
                        }
                }
        }
}
