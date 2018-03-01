using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ericson;

public class Unit : e_Sprite 
{
    public Vector2 _prevPos;
    public Vector2 _gridpos;
    private UnitData _data = null;
    public UnitState state = UnitState.idle;
    public List<Vector2> inMoveRange = new List<Vector2>();
    public List<Vector2> visibleTiles = new List<Vector2>();
    public List<Vector2> foodInRange = new List<Vector2>();
    public List<Vector2> inAttackRange = new List<Vector2>();
    public List<Vector2> enemiesInRange = new List<Vector2>();
    public List<Vector2> inSplitRange = new List<Vector2>();
    public TextMesh healthText;
    public TextMesh foodText;
    private float zoffset = -0.5f;
    public Team team = 0;
    private int health = 0;
    public int food = 0;
    private int actionPoints = 3;
    public bool isMoving = false;
    private int moveIterator = 0;
    private int moveSpeed = 8;
    private List<Tile> pathToFollow = new List<Tile>();
    public SpriteRenderer unitIcon = null;
    private Color player1Color = Color.red * 0.75f;
    private Color player2Color = Color.blue * 0.75f;
    


    public Vector2 gridpos 
    {
        get { return _gridpos; }
    }
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
        healthText = gameObject.transform.Find("Health").gameObject.GetComponent<TextMesh>();
        foodText = gameObject.transform.Find("Food").gameObject.GetComponent<TextMesh>();
    }
    public void Init(UnitType type, Team team)
    {
        SetType(type);
        SetTeam(team);
    }
    void Update()
    {
        if(isMoving) FollowPath();
    }
    public void StartMoving(List <Tile> path)
    {
        SetPrevPos();
        pathToFollow = path;
        isMoving = true;
    }
    public void FollowPath()
    {
        if (pathToFollow != null && pathToFollow.Count > 0)
        {
            var target = pathToFollow[moveIterator].transform.position;
            target.z = zoffset;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            if (transform.position == target) moveIterator++;
        }
        if (moveIterator >= pathToFollow.Count)
        {
            moveIterator = 0;
            isMoving = false;
            GameScene.ins.TempMoveUnit();
        }
    }
    public void Select()
    {   
        ClearAllLists();     
        ChangeState(UnitState.selected);
        SetGridPos();
    }
    public void Sleep()
    {
        ClearAllLists();
        SetColor(GetComponent<SpriteRenderer>().color * 0.5f);
        SetPrevPos();
        SetGridPos();
        ChangeState(UnitState.sleeping);
    }
    public void Idle()
    {
        ClearAllLists();
        if(state == UnitState.sleeping) SetColor(GetComponent<SpriteRenderer>().color * 2);
        ChangeState(UnitState.idle);
    }
    public void AwaitAction()
    {
        ChangeState(UnitState.awaitingAction);
    }

    public void Undo()
    {
        var temp = MapManager.ins.GridToWorld(_prevPos);
        temp.z = zoffset;
        gameObject.transform.position = temp;
        Idle();
    }
    public void Split(int damage)
    {
        food -= damage;
        UpdateText();
    }
    public void Eat()
    {
        food += 1;
        foodInRange.Clear();
        UpdateText();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0) ChangeState(UnitState.dead);
        if(state != UnitState.dead) UpdateText();
    }
    private void SetType(UnitType type)
    {
        _data = Database.unitData[(int)type];
        health = _data.maxHealth;
        food = 0;
        if(type != UnitType.queen) foodText.gameObject.SetActive(false);
        unitIcon.sprite = Sprites.ins.unitSprites[type];
        UpdateText();
    }
    private void UpdateText()
    {
        if (healthText) healthText.text = health.ToString();
        if(foodText) foodText.text = food.ToString();
    }
    public void Deselect()
    {
        if(state != UnitState.idle) Idle();
        ClearAllLists();
    }
    public void Move(Vector2 target)
    {
        var temp = MapManager.ins.GridToWorld(target);
        temp.z = zoffset;
        gameObject.transform.position = temp;
    }
    public void SetPrevPos()
    {
        _prevPos = MapManager.ins.WorldToGrid(transform.position);
    }
    public void SetGridPos()
    {
        _gridpos = MapManager.ins.WorldToGrid(transform.position);
    }
    public void CheckMove()
    {
        inMoveRange = GetTilesInRange(data.speed);
        for(int i = inMoveRange.Count - 1; i >= 0; i--)
        {
            var path = AStar.ins.FindPath(gridpos, inMoveRange[i]);
            if ((path != null && path.Count > data.speed) || path == null)
            {
                inMoveRange.Remove(inMoveRange[i]);
            }
            else if(path != null)
            {
                foreach (Tile t in path)
                {
                    if (MapManager.ins.unitGrid.ContainsKey(t.gridpos) && MapManager.ins.unitGrid[t.gridpos].team != team)
                    {
                        try
                        {
                            inMoveRange.Remove(inMoveRange[i]);
                        }
                        catch
                        {

                        }
                        
                    }
                }
            }
        }
    }
    //
    public void CheckVision()
    {
        visibleTiles.Clear();


        CheckMove();

        for (int d = 0; d < 8; d++)
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
                if ((data.size > temptile.data.maxSize) ||
                    (MapManager.ins.unitGrid.ContainsKey(tempgPos) && MapManager.ins.unitGrid[tempgPos].team != team))
                {
                    if (!visibleTiles.Contains(temptile.gridpos))
                    {
                        visibleTiles.Add(temptile.gridpos);
                    }
                    break;
                }


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
                    if (data.size > tile.data.maxSize ||
                    (MapManager.ins.unitGrid.ContainsKey(gPos) && MapManager.ins.unitGrid[gPos].team != team))
                    {
                        if (!visibleTiles.Contains(MapManager.ins.WorldToGrid(tile.transform.position)))
                        {
                            visibleTiles.Add(MapManager.ins.WorldToGrid(tile.transform.position));
                        }
                        break;
                    }
                    else
                    {
                        if (!visibleTiles.Contains(MapManager.ins.WorldToGrid(tile.transform.position)))
                        {
                            visibleTiles.Add(MapManager.ins.WorldToGrid(tile.transform.position));
                        }
                    }
                }
            }
        }

        foreach (Vector2 v1 in inMoveRange)
        {
            if (!visibleTiles.Contains(v1)) visibleTiles.Add(v1);
        }
    }
    //
    public void CheckForEnemies()
    {
       
        foreach(Vector2 v in inAttackRange)
        {
            if(MapManager.ins.unitGrid.ContainsKey(v) && MapManager.ins.unitGrid[v].team != team)
            {
                foreach(Vector2 k in GameScene.ins.GetAllVisibleTiles())
                {
                    if(k == v) 
                    {
                        enemiesInRange.Add(v);
                        break;
                    }
                }
                
            }
        }
    }
    public void CheckAttackRange()
    {
        List<Vector2> tempRange = GetTilesInRange(data.range);
        foreach(Vector2 v in tempRange) 
        {
            var thisgPos = MapManager.ins.WorldToGrid(transform.position);
            var dx = thisgPos.x - v.x;
            var dy = thisgPos.y - v.y;
            if(Mathf.Abs(dx) + Mathf.Abs(dy) == 1 && data.range > 1) continue;
            inAttackRange.Add(v);
        }
    }
    public void CheckForFood()
    {
        if(food == _data.maxFood) return;
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

                if(tile.data.type == TileType.food)
                {
                    foodInRange.Add(gPos);
                }
            }
        }
    }
    public void CheckSplitRange() 
    {
        List <Vector2> tempSplitRange = GetTilesInRange(1);

        foreach(Vector2 v in tempSplitRange)
        {
            if((MapManager.ins.unitGrid.ContainsKey(v) && visibleTiles.Contains(v)) || 
               MapManager.ins.currentMap[v].data.maxSize < 0 ||
               MapManager.ins.WorldToGrid(transform.position) == v) continue;

            inSplitRange.Add(v);
        }
    }
    private List<Vector2> GetTilesInRange(int range)
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
                if (!MapManager.ins.currentMap.ContainsKey(gPos) 
                    || MapManager.ins.currentMap[gPos].data.maxSize < data.size) continue;
                toReturn.Add(gPos);
            }
        }
        return toReturn;
    }
    private void SetTeam(Team _team)
    {
        team = _team;
        SetColor(PersistentSettings.team_colors[_team]);
    }
    private void ChangeState(UnitState newstate)
    {
        state = newstate;
    }
    public void ClearAllLists()
    {
        inAttackRange.Clear();
        foodInRange.Clear();
        inMoveRange.Clear();
        inSplitRange.Clear();
        enemiesInRange.Clear();
    }
    public void CheckEverything()
    {
        CheckAttackRange();
        CheckForEnemies();
        CheckForFood();
        CheckSplitRange();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private int GetDistance(Vector2 g1, Vector2 g2)
    {
        return ((int)Mathf.Abs(g1.x - g2.x)) + ((int)Mathf.Abs(g1.y - g2.y));
    }
}
