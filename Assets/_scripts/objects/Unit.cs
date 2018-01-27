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
    public List<Vector2> inMoveRange = new List<Vector2>();
    public List<Vector2> inEatRange = new List<Vector2>();
    public List<Vector2> inAttackRange = new List<Vector2>();
    public List<Vector2> enemiesInRange = new List<Vector2>();
    public List<Vector2> inSplitRange = new List<Vector2>();
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
        inMoveRange.Clear();
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
        for(int d = 0; d < 8; d++)
        {
            for (int i = 0; i <= data.speed; i++)
            {
                Vector2 tempgPos = MapManager.ins.WorldToGrid(transform.position);
                switch (d)
                {
                    case 0: // right, down
                        tempgPos.x += i;
                        break;
                    case 1: // right, up
                        tempgPos.x += i;
                        break;
                    case 2: // left, down
                        tempgPos.x -= i;
                        break;
                    case 3: // left, up
                        tempgPos.x -= i;
                        break;
                    case 4: // down, left
                        tempgPos.y += i;
                        break;
                    case 5: // down, right
                        tempgPos.y += i;
                        break;
                    case 6: // up, left
                        tempgPos.y -= i;
                        break;
                    case 7: // up, down
                        tempgPos.y -= i;
                        break;
                }
                if (!MapManager.ins.currentMap.ContainsKey(tempgPos)) continue;
                var temptile = MapManager.ins.currentMap[tempgPos];
                
                if (data.size > temptile.data.maxSize) break;
                if(MapManager.ins.unitGrid.ContainsKey(tempgPos) && MapManager.ins.unitGrid[tempgPos].team != team) break;
                

                for (int j = 0; j <= data.speed; j++)
                {
                    if (Mathf.Abs(i) + Mathf.Abs(j) > data.speed) continue;
                    Vector2 gPos = MapManager.ins.WorldToGrid(transform.position);
                    switch (d)
                    {
                        case 0: // right, down
                            gPos.x += i;
                            gPos.y += j;
                            break;
                        case 1: // right, up
                            gPos.x += i;
                            gPos.y -= j;
                            break;
                        case 2: // left, down
                            gPos.x -= i;
                            gPos.y += j;
                            break;
                        case 3: // left, up
                            gPos.x -= i;
                            gPos.y -= j;
                            break;
                        case 4: // down, left
                            gPos.x -= j;
                            gPos.y += i;
                            break;
                        case 5: // down, right
                            gPos.x += j;
                            gPos.y += i;
                            break;
                        case 6: // up, left
                            gPos.x -= j;
                            gPos.y -= i;
                            break;
                        case 7: // up, right
                            gPos.x += j;
                            gPos.y -= i;
                            break;
                    }
                    if (!MapManager.ins.currentMap.ContainsKey(gPos)) continue;
                    var tile = MapManager.ins.currentMap[gPos];
                    if (data.size > tile.data.maxSize) break;
                    if(MapManager.ins.unitGrid.ContainsKey(gPos) && MapManager.ins.unitGrid[gPos].team != team) break;
                        
                    if(!MapManager.ins.unitGrid.ContainsKey(gPos) ||
                        gPos == MapManager.ins.WorldToGrid(gameObject.transform.position)) 
                    {
                        inMoveRange.Add(MapManager.ins.WorldToGrid(tile.transform.position));
                    }

                    
                }
            }
        }
    }
    public void CheckForEnemies()
    {
        foreach(Vector2 v in inAttackRange)
        {
            if(MapManager.ins.unitGrid.ContainsKey(v) && MapManager.ins.unitGrid[v].team != team)
            {
                enemiesInRange.Add(v);
            }
        }
    }
    public void CheckAttackRange()
    {
        List<Vector2> tempRange = CheckTiles(data.range);
        Debug.Log(data.range);
        foreach(Vector2 v in tempRange) 
        {
            Debug.Log(v);
            var thisgPos = MapManager.ins.WorldToGrid(transform.position);
            var dx = thisgPos.x - v.x;
            var dy = thisgPos.y - v.y;
            if(Mathf.Abs(dx) + Mathf.Abs(dy) == 1 && data.range > 1) continue;
            inAttackRange.Add(v);
        }
    }
    public void CheckForFood()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) > 1) continue;
                Vector2 gPos = MapManager.ins.WorldToGrid(transform.position);
                gPos.x += i;
                gPos.y += j;
                if (!MapManager.ins.currentMap.ContainsKey(gPos)) continue;
                var tile = MapManager.ins.currentMap[gPos];

                // TODO: add tile to food list
                // TODO: turn it a color
            }
        }
    }
    public void CheckSplitRange() 
    {
        List <Vector2> tempSplitRange = CheckTiles(1);

        foreach(Vector2 v in tempSplitRange)
        {
            if(MapManager.ins.unitGrid.ContainsKey(v) || 
               MapManager.ins.currentMap[v].data.maxSize < 0 ||
               MapManager.ins.WorldToGrid(transform.position) == v) continue;

            inSplitRange.Add(v);
        }
    }
    private List<Vector2> CheckTiles(int range)
    {
        List<Vector2> toReturn = new List<Vector2>();
        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) > range) continue;
                Vector2 gPos = MapManager.ins.WorldToGrid(transform.position);
                gPos.x += i;
                gPos.y += j;
                if (!MapManager.ins.currentMap.ContainsKey(gPos)) continue;
                var tile = MapManager.ins.currentMap[gPos];
                toReturn.Add(gPos);
            }
        }
        return toReturn;
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
