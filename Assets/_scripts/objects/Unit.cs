using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ericson;

public class Unit : ESprite 
{

    private Vector2 _prevPos;
    private UnitData _data = null;
    public UnitState state = UnitState.idle;
    public List<Vector2> tilesWithinMoveRange = new List<Vector2>();
    public List<Vector2> foodWithinRange = new List<Vector2>();
    public List<Vector2> enemiesWithinRange = new List<Vector2>();
    public TextMesh tierText;
    private float zoffset = -0.2f;
    public Team team = 0;


    public Vector2 prevPos
    {
        get { return _prevPos; }
    }
    public UnitData data
    {
        get { return _data; }
    }






    public void Awake()
    {
        tierText = gameObject.transform.Find("Tier").gameObject.GetComponent<TextMesh>();
    }
    public void Init(int tier, Team team)
    {
        UpdateTier(tier);
        SetTeam(team);
    }
    public void Select()
    {        
        ChangeState(UnitState.selected);
    }
    public void Sleep()
    {
        SetColor(GetComponent<SpriteRenderer>().color / 2);
        ChangeState(UnitState.sleeping);
    }
    public void Idle()
    {
        tilesWithinMoveRange.Clear();
        if(state == UnitState.sleeping) SetColor(GetComponent<SpriteRenderer>().color * 2);
        ChangeState(UnitState.idle);
    }
    public void AwaitAction()
    {
        ChangeState(UnitState.awaitingAction);
    }

    public void Undo()
    {
        var temp = MapManager.ins.GridToWorld(prevPos);
        temp.z = zoffset;
        gameObject.transform.position = temp;
        Idle();
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
        if(state != UnitState.idle) Idle();
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
    public void CheckMove()
    {
        CheckTiles(data.speed, Action.move);
    }
    public void CheckAttackRange()
    {
        CheckTiles(data.range, Action.attack);
    }
    public void CheckForFood()
    {
        CheckTiles(1, Action.eat);
    }
    private void CheckTiles(int value, Action action)
    {
        for (int i = -value; i <= value; i++)
        {
            for (int j = -value; j <= value; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) > value || i == 0 && j == 0) continue;
                Vector2 gPos = MapManager.ins.WorldToGrid(transform.position);
                gPos.x += i;
                gPos.y += j;
                if (!MapManager.ins.currentMap.ContainsKey(gPos)) continue;
                var tile = MapManager.ins.currentMap[gPos];


                switch (action)
                {
                    case Action.move:
                        if (data.size <= tile.data.maxSize)
                        {
                            tile.Highlight();
                            tilesWithinMoveRange.Add(gPos);
                        }
                        break;
                    case Action.attack:
                        tile.SetColor(Color.red / 2);
                        break;
                }
            }
        }
    }

    private void SetTeam(Team _team)
    {
        team = _team;
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
    private void ChangeState(UnitState newstate)
    {
        state = newstate;
        //Debug.Log("Current Unit State: " + state);
    }
}
