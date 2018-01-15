using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ericson;

public class Unit : ESprite 
{

    private Vector2 _prevPos;
    private ActionMenu _actionMenu = null;
    private UnitData _data = null;
    public UnitState state = UnitState.idle;
    public List<Vector2> tilesInRange = new List<Vector2>();
    public TextMesh tierText;
    private float zoffset = -0.2f;
    public Vector2 prevPos
    {
        get { return _prevPos; }
    }
    public ActionMenu actionMenu
    {
        get
        {
            if (_actionMenu == null) _actionMenu = gameObject.transform.Find("ActionMenu").gameObject.GetComponent<ActionMenu>();
            return _actionMenu;
        }
    }
    public UnitData data { get { return _data; } }






    public void Awake()
    {
        _actionMenu = gameObject.transform.Find("ActionMenu").gameObject.GetComponent<ActionMenu>();
        tierText = gameObject.transform.Find("Tier").gameObject.GetComponent<TextMesh>();
    }
    public void Init(int tier, Team team)
    {
        UpdateTier(tier);
        SetTeam(team);
        HideMenu();
    }
    public void Select()
    {        
        CheckMoveDistance();
        state = UnitState.selected;
    }
    public void Sleep()
    {
        SetColor(GetComponent<SpriteRenderer>().color / 2);
        HideMenu();
        state = UnitState.sleeping;
    }
    public void Idle()
    {
        tilesInRange.Clear();
        HideMenu();
        state = UnitState.idle;
    }
    public void AwaitAction()
    {
        ShowMenu();
        state = UnitState.awaitingAction;
    }

    public void Undo()
    {
        var temp = MapManager.ins.GridToWorld(prevPos);
        temp.z = zoffset;
        gameObject.transform.position = temp;
        HideMenu();
        state = UnitState.idle;
    }
    public void Eat()
    {
        ChangeTier(1);
    }
    public void TakeDamage(int damage)
    {
        ChangeTier(-damage);
    }
    private void ChangeTier(int tier)
    {
        int newTier = _data.tier + tier;
        if (newTier > 3) newTier = 3;
        else if (newTier < 1) state = UnitState.dead;
        if(state != UnitState.dead)
        {
            UpdateTier(newTier);
        }
    }
    private void UpdateTier(int tier)
    {
        _data = Database.unitData[(int)tier - 1];
        if (tierText) tierText.text = _data.tier.ToString();
    }
    public void Deselect()
    {
        state = UnitState.idle;
    }
    private void ShowMenu()
    {
        if(actionMenu) actionMenu.gameObject.SetActive(true);
    }
    private void HideMenu()
    {
        if(actionMenu)
        {
            actionMenu.gameObject.SetActive(false);
            actionMenu.Clear();
        }
    }
    public void Move(Vector2 target)
    {
        var temp = MapManager.ins.GridToWorld(target);
        temp.z = zoffset;
        gameObject.transform.position = temp;
    }
    public void SetPrevPos(Vector2 newPos)
    {
        _prevPos = newPos;
    }
    public Button AddButton(string label)
    {
        return actionMenu.AddButton(label);
    }
    private void CheckMoveDistance()
    {
        for (int i = -data.speed; i <= data.speed; i++)
        {
            for (int j = -data.speed; j <= data.speed; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) > data.speed || i == 0 && j == 0) continue;
                Vector2 gPos = MapManager.ins.WorldToGrid(transform.position);
                gPos.x += i;
                gPos.y += j;
                if (!MapManager.ins.currentMap.ContainsKey(gPos)) continue;
                var tile = MapManager.ins.currentMap[gPos];
                if (data.size <= tile.data.maxSize)
                {
                    tile.Highlight();
                    tilesInRange.Add(gPos);
                }
            }
        }
    }

    private void SetTeam(Team team)
    {
        switch (team)
        {
            case Team.Player1:
                SetColor(Color.red);
                break;
            case Team.Player2:
                SetColor(Color.blue);
                break;
        }
    }
}
