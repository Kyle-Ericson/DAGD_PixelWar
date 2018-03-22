using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ericson;

public class PregameScene : eSingletonMono<PregameScene>
{
    public GameObject ui = null;
    public eMenu maplist = null;
    public int mapToLoad = -1;
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
        maplist.UpdateMenu();
    }
    public void AddButtons()
    {
        foreach(MapData mapdata in Database.mapData)
        {
            maplist.AddBasicButton(mapdata.name).onClick.AddListener(() =>
            {
                mapToLoad = mapdata.id;
                Debug.Log(mapToLoad);
            });
        }
        maplist.UpdateMenu();
    }
    private void HandleStart()
    {
        if(mapToLoad >= 0) SceneManager.ins.StartMatch(mapToLoad, 2);
    }
}
