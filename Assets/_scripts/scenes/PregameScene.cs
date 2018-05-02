using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ericson;

public class PregameScene : eSingletonMono<PregameScene>
{
    public GameObject ui = null;
    public eMenu maplist = null;
    public int mapToLoad = 1;
    public Button startMatchButton = null;

    public override void Init()
    {
        ui = Instantiate(Resources.Load<GameObject>("prefabs/scenes/Pregame"));
        ui.transform.SetParent(this.gameObject.transform);
        maplist = ui.transform.GetChild(1).GetComponent<eMenu>();
        startMatchButton = ui.transform.GetChild(2).GetComponent<Button>();
        startMatchButton.onClick.AddListener(HandleStart);
        maplist.Show();
        AddButtons();
    }
    public void AddButtons()
    {
        foreach(MapData mapdata in Database.mapData)
        {
            if(mapdata.id == 0) continue;
            maplist.AddBasicButton(mapdata.name).onClick.AddListener(() =>
            {
                mapToLoad = mapdata.id;
                MapManager.ins.SpawnMap(mapToLoad, 1000, true);
            });
        }
        maplist.UpdateMenu();
    }
    private void HandleStart()
    {
        if(mapToLoad >= 0) 
        {
            if(PersistentSettings.gameMode == GameMode.online) 
            {
                SocketManager.ins.Send(PacketFactory.BuildNewGame(mapToLoad, PersistentSettings.isPrivate));
                SceneManager.ins.ToWait();
            }
            else SceneManager.ins.StartMatch(mapToLoad);
        }
    }
}
