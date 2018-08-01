using System.Collections.Generic;
using UnityEngine;
using ericson;
using System.Net.Sockets;

public class GameScene : eSingletonMono<GameScene>
{

    private float zoffset = -0.2f;
    private UnitType nextSplitType;
    private GameObject unitHolder;
    private Unit _currentSelected = null;
    
    public Team clientTeam;
    public GameMode gameMode;
    public GameObject unitPrefab;
    public Unit gettingAttacked = null;
    public SelectionBox selectionbox;
    public GameState _gameState;
    public GameUI gameUI = null;
    public eMenu actionMenu = null;
    public Vector2 tempGridPos;
    public bool running = true;
    public int currentTurn = 1;
    public int turnCount = 1;
    public List<Vector2> allVisibleTiles = new List<Vector2>();
    public TutorialPhase tutorialPhase = TutorialPhase.phase1;

    public Unit currentSelected
    {
        get { return _currentSelected; }
    }
    public GameState gameState
    {
        get { return _gameState; }
    }


    public override void Init()
    {
        unitHolder = new GameObject("Units");
        selectionbox = Instantiate(Resources.Load<GameObject>("prefabs/Cursor").GetComponent<SelectionBox>());
        gameUI = Instantiate(Resources.Load<GameObject>("prefabs/GameUI").GetComponent<GameUI>());
        actionMenu = Instantiate(Resources.Load<GameObject>("prefabs/RadialMenu")).GetComponent<eMenu>();
        unitPrefab = Resources.Load<GameObject>("prefabs/Unit");
        gameUI.gameObject.transform.SetParent(this.gameObject.transform);
        unitHolder.gameObject.transform.SetParent(this.gameObject.transform);
        selectionbox.gameObject.transform.SetParent(this.gameObject.transform);
        actionMenu.gameObject.transform.SetParent(this.gameObject.transform);
        MapManager.ins.gameObject.transform.SetParent(this.gameObject.transform);
        AStar.ins.gameObject.transform.SetParent(this.gameObject.transform);
        if(!Sprites.ins.loaded) Sprites.ins.LoadSprites();
        MatchStats.ResetAll();
        Database.Load();
    }
    public void SetupMatch(int map)
    {
        running = true;
        _gameState = new AwaitingInput();
        LoadMap(map);
        selectionbox.Show();
        selectionbox.Move(Vector2.zero);
        AddListeners();
        MatchStats.ResetAll();
        gameUI.UpdateArmyText();
        gameUI.UpdateFoodText();
        gameUI.ResetTransitions();
        if(gameUI.menuOpen) gameUI.CloseMenu();
        currentTurn = 1;
    
        if(PersistentSettings.gameMode == GameMode.online) 
        {
            if(PersistentSettings.isHost) clientTeam = Team.player1;
            else clientTeam = Team.player2;
            CheckTurnOnline();
        }
    }
    //
    private void LoadMap(int mapID)
    {
        MapManager.ins.SpawnMap(mapID, 0, false);
        SpawnUnit(UnitType.worker, Team.player1, MapManager.ins.currentMapData.start1.ToVector2());
        SpawnUnit(UnitType.worker, Team.player2, MapManager.ins.currentMapData.start2.ToVector2());
        if(PersistentSettings.gameMode == GameMode.tutorial)
        {
            gameUI.ShowTutorial();
            tutorialPhase = TutorialPhase.phase1;
            MapManager.ins.currentMap[MapManager.ins.currentMapData.start1.ToVector2()].HighlightGrid();
            gameUI.UpdateTutorial();
        }
        else gameUI.HideTutorial();
    }

    
    //
    private void AddListeners()
    {
        RemoveListeners();
        InputEvents.ins.OnMouseLClick += HandleMouseLClick;
        InputEvents.ins.OnMouseRClick += HandleMouseRClick;
        InputEvents.ins.OnMouseMoved += selectionbox.Move;
    }
    private void RemoveListeners()
    {
        InputEvents.ins.OnMouseLClick -= HandleMouseLClick;
        InputEvents.ins.OnMouseRClick -= HandleMouseRClick;
        InputEvents.ins.OnMouseMoved -= selectionbox.Move;
    }

    // handle a left mouse button click
    private void HandleMouseLClick()
    {
        if(PersistentSettings.gameMode == GameMode.online && currentTurn != (int)clientTeam) return;
        if(PersistentSettings.gameMode == GameMode.tutorial && tutorialPhase == TutorialPhase.phase7) gameUI.HideTutorial();
        if(Application.platform == RuntimePlatform.Android) selectionbox.Move();
        _gameState.HandleLeftClick();

    }

    // handle when the right mouse button is clicked
    private void HandleMouseRClick()
    {
        if(PersistentSettings.gameMode == GameMode.online && currentTurn != (int)clientTeam) return;
        if(PersistentSettings.gameMode == GameMode.tutorial && tutorialPhase == TutorialPhase.phase7) gameUI.HideTutorial();
        _gameState.HandleRightClick();
    }

    public void ConfirmSplit(Vector2 splitPos)
    {
        if(MapManager.ins.unitGrid.ContainsKey(splitPos))
        {
            ConfirmMove();
            return;
        }
        SpawnUnit(nextSplitType, currentSelected.team, splitPos);
        MapManager.ins.unitGrid[splitPos].Sleep();
        currentSelected.Split(Database.unitData[(int)nextSplitType].cost);

        
        MatchStats.UpdateSpent((Team)currentTurn, Database.unitData[(int)nextSplitType].cost);

        gameUI.UpdateArmyText();
        gameUI.UpdateFoodText();
        ConfirmMove();        
    }
    public void BeginAttack()
    {
        currentSelected.unitGraphic.Attack();
    }
    public void ConfirmAttack()
    {
        if(gettingAttacked != null)
        {
            gettingAttacked.TakeDamage(currentSelected.data.attack);
            if(gettingAttacked.state == UnitState.dead)
            {
                var saveTeam = gettingAttacked.team;
                KillUnit(gettingAttacked.gridpos);
                CheckForWin(saveTeam);
            }
        }
        GameScene.ins.ConfirmMove();
    }
    public void ConfirmEat()
    {
        currentSelected.Eat();
        ConfirmMove();
        gameUI.UpdateFoodText();
        MatchStats.UpdateGathered((Team)currentTurn, 1);
        if(PersistentSettings.gameMode == GameMode.tutorial && tutorialPhase == TutorialPhase.phase3) NextTutorialPhase();
    }
    public void ConfirmMove()
    {
        
        currentSelected.Sleep();
        _currentSelected = null;
        gettingAttacked = null;
        DeselectAll();
        UpdateFog();
    }
    public void HandleWait()
    {
        ConfirmMove();
    }
    public void HandleAttack()
    {
        foreach(Vector2 v in _currentSelected.enemiesInRange)
        {
            Tile tile = MapManager.ins.currentMap[v];
            tile.Highlight("Attack");
            tile.SetIconColor(Color.red);
        }
        StateAwaitAttack();
    }
    public void HandleUndo()
    {
        Undo();
    }
    public void HandleEat()
    {
        foreach(Vector2 v in _currentSelected.foodInRange)
        {
            Tile tile = MapManager.ins.currentMap[v];
            tile.Highlight("Gather");
        }
        StateAwaitEat();
    }
    public void HandleSplit()
    {
        actionMenu.Clear();
        
        if(_currentSelected.type == UnitType.worker)
        {
            if(GetTeamFoodCount() >= Database.unitData[(int)UnitType.scout].cost)
            {
                var button = actionMenu.AddRadialUnit(UnitType.scout);
                button.onClick.AddListener(() => 
                { 
                    nextSplitType = UnitType.scout;
                    StateAwaitSplit();
                });
            }
            if(GetTeamFoodCount() >= Database.unitData[(int)UnitType.tank].cost)
            {
                actionMenu.AddRadialUnit(UnitType.tank).onClick.AddListener(() => 
                { 
                    nextSplitType = UnitType.tank; 
                    StateAwaitSplit();
                });    
            }
            if(GetTeamFoodCount() >= Database.unitData[(int)UnitType.soldier].cost)
            {
                actionMenu.AddRadialUnit(UnitType.soldier).onClick.AddListener(() => 
                { 
                    nextSplitType = UnitType.soldier; 
                    StateAwaitSplit();
                });
            }
            if(GetTeamFoodCount() >= Database.unitData[(int)UnitType.sniper].cost)
            {
                actionMenu.AddRadialUnit(UnitType.sniper).onClick.AddListener(() => 
                { 
                    nextSplitType = UnitType.sniper;
                    StateAwaitSplit();
                });
            }
            if(GetTeamFoodCount() >= Database.unitData[(int)UnitType.worker].cost)
            {
                actionMenu.AddRadialUnit(UnitType.worker).onClick.AddListener(() => 
                { 
                    nextSplitType = UnitType.worker;
                    StateAwaitSplit();
                });
            }
        }
        else 
        {
            actionMenu.AddRadialUnit(_currentSelected.type).onClick.AddListener(() => 
            {
                nextSplitType = _currentSelected.type;
                StateAwaitSplit();
            });
        }
        
        actionMenu.UpdateRadialMenu();
    }


    public void StateAwaitInput()
    {
        InputEvents.ins.OnMouseMoved -= ShowPath;
        gameUI.ShowEndButton();
        AddListeners();
        actionMenu.Hide();
        _gameState = new AwaitingInput();
    }
    public void StateUnitSelected()
    {
        AddListeners();
        InputEvents.ins.OnMouseMoved += ShowPath;
        gameUI.HideEndButton();
        _gameState = new UnitSelected();
    }
    public void StateAwaitAction()
    {
        AddListeners();
        gameUI.HideEndButton();
        InputEvents.ins.OnMouseMoved -= ShowPath;
        InputEvents.ins.OnMouseMoved -= selectionbox.Move;
        selectionbox.Hide();
        actionMenu.Show();
        if(PersistentSettings.gameMode == GameMode.tutorial 
        && (tutorialPhase == TutorialPhase.phase2 
        || tutorialPhase == TutorialPhase.phase5))
        {
            NextTutorialPhase();
        }
        _gameState = new AwaitingAction();
    }
    public void StateAwaitSplit()
    {
        AddListeners();
        InputEvents.ins.OnMouseMoved -= ShowPath;
        foreach (Vector2 v in currentSelected.inSplitRange)
        {
            Tile tile = MapManager.ins.currentMap[v];
            if(_currentSelected.type == UnitType.worker) tile.Highlight("Build");
            else tile.Highlight("Paste");
        }
        selectionbox.Show();
        actionMenu.Hide();
        _gameState = new AwaitingSplit();
    }
    public void StateAwaitAttack()
    {
        AddListeners();
        InputEvents.ins.OnMouseMoved -= ShowPath;
        gameUI.HideEndButton();
        selectionbox.Show();
        actionMenu.Hide();
        _gameState = new AwaitingAttack();
    }
    public void StateAwaitEat()
    {
        AddListeners();
        gameUI.HideEndButton();
        selectionbox.Show();
        actionMenu.Hide();
        _gameState = new AwaitingEat();
    }
    public void StateAnimating()
    {
        RemoveListeners();
        InputEvents.ins.OnMouseMoved -= ShowPath;
        gameUI.HideEndButton();
        selectionbox.Hide();
        actionMenu.Hide();
        DeselectAllTiles();
        _gameState = new Animating();
    }
    public void WakeUpAll()
    {
        foreach (KeyValuePair<Vector2, Unit> item in MapManager.ins.unitGrid)
        {
            item.Value.Idle();
        }
    }
    private void DeselectAll()
    {
        DeselectAllTiles();
        DeselectAllUnits();
        StateAwaitInput();
    }
    public void DeselectAllTiles()
    {
        foreach (KeyValuePair<Vector2, Tile> k in MapManager.ins.currentMap) 
        { 
            k.Value.SetIconColor(Color.white);
            k.Value.UnHighlight();
        }
    }
    private void DeselectAllUnits()
    {
        foreach (KeyValuePair<Vector2, Unit> k in MapManager.ins.unitGrid)
        {
            if (k.Value.state != UnitState.sleeping) k.Value.Deselect();
        }
        _currentSelected = null;
    }
    public override void CleanUp()
    {
        if(running)
        {
            gameUI.CleanUp();
            MapManager.ins.CleanUp();
            RemoveListeners();
            selectionbox.Hide();
            clientTeam = Team.None;
            currentTurn = 1;
            turnCount = 1;
            PersistentSettings.isHost = false;
        }
        running = false;
    }
    public void TempMoveUnit()
    {
        MapManager.ins.unitGrid.Remove(_currentSelected.prevPos);
        _currentSelected.SetGridPos();
        MapManager.ins.unitGrid.Add(_currentSelected.gridpos, _currentSelected);
        _currentSelected.CheckEverything();
        _currentSelected.AwaitAction();
        DeselectAllTiles();
        SetupActionMenu();
        StateAwaitAction();
    }
    private void SetupActionMenu()
    {
        actionMenu.transform.position = selectionbox.transform.position;
        actionMenu.Clear();
        if(_currentSelected.data.id == UnitType.worker)
        {
            if (_currentSelected.foodInRange.Count > 0) actionMenu.AddRadialButton("Gather").onClick.AddListener(HandleEat);
            if (GetTeamFoodCount() > 0 && _currentSelected.inSplitRange.Count > 0) actionMenu.AddRadialButton("Build").onClick.AddListener(HandleSplit);
        }
        else if (GetTeamFoodCount() >= _currentSelected.data.cost) actionMenu.AddRadialButton("Copy").onClick.AddListener(HandleSplit);
        if(_currentSelected.enemiesInRange.Count > 0) actionMenu.AddRadialButton("Attack").onClick.AddListener(HandleAttack);
        actionMenu.AddRadialButton("Wait").onClick.AddListener(HandleWait);
        actionMenu.AddRadialButton("Undo").onClick.AddListener(HandleUndo);
        actionMenu.UpdateRadialMenu();
    }
    private void KillUnit(Vector2 unitPos)
    {
        MapManager.ins.RemoveUnit(unitPos);
        SoundManager.ins.PlayDeath();
    }
    public void SpawnUnit(UnitType type, Team team, Vector2 gridPos)
    {
        Vector3 worldPos = MapManager.ins.GridToWorld(gridPos);
        worldPos.z = zoffset;
        Unit unit = Instantiate(unitPrefab).GetComponent<Unit>();
        unit.gameObject.transform.position = worldPos;
        unit.Init(type, team);
        MapManager.ins.unitGrid.Add(gridPos, unit);
        unit.SetPrevPos();
        unit.SetGridPos();
        unit.gameObject.transform.SetParent(unitHolder.transform);
        UpdateFog();
    }
    public void SelectUnit()
    {
        _currentSelected = MapManager.ins.unitGrid[selectionbox.gridpos];
        _currentSelected.Select();
        _currentSelected.CheckMove();

        if (_currentSelected.inMoveRange.Count > 0)
        {
            foreach (Vector2 v in currentSelected.inMoveRange)
            {
                if (v != _currentSelected.gridpos) MapManager.ins.currentMap[v].Highlight();
            }
        }
        if(PersistentSettings.gameMode == GameMode.tutorial && (tutorialPhase == TutorialPhase.phase1 || tutorialPhase == TutorialPhase.phase5)) 
        {
            NextTutorialPhase();
        }
        StateUnitSelected();
    }
    private void ShowPath()
    {
        foreach (Vector2 v in currentSelected.inMoveRange) 
        {
            MapManager.ins.currentMap[v].SetIconColor(Color.white);
            MapManager.ins.currentMap[v].Highlight();

        }
        if (_currentSelected.inMoveRange.Contains(selectionbox.gridpos))
        {
            var path = AStar.ins.FindPath(_currentSelected.gridpos, selectionbox.gridpos);
            foreach (Tile t in path) 
            {
                if(selectionbox.gridpos == t.gridpos) 
                {
                    t.Highlight("Move");
                }
                else
                {
                    t.SetIconColor(Color.red); 
                    t.Highlight();
                }
            }
        }
    }
    private void UpdateFog()
    {
        AllFogOn();
        AllUnitsOff();
        foreach(KeyValuePair<Vector2, Unit> k in MapManager.ins.unitGrid)
        {
            if(k.Value.team == (Team)currentTurn)
            {
                k.Value.Show();
                k.Value.CheckVision();
                foreach(Vector2 v in k.Value.visibleTiles)
                {
                    MapManager.ins.currentMap[v].HideFog();
                    allVisibleTiles.Add(v);
                    if(MapManager.ins.unitGrid.ContainsKey(v))
                    {
                        MapManager.ins.unitGrid[v].Show();
                    }
                }
            }
        }
    }
    public void AllFogOn()
    {
        allVisibleTiles.Clear();
        foreach(KeyValuePair<Vector2, Tile> k in MapManager.ins.currentMap)
        {
            k.Value.ShowFog();
        }
    }
    public void AllUnitsOff()
    {
        foreach(Unit u in GetAllUnits())
        {
            u.Hide();
        }
    }
    public void EndTurn()
    {
        DeselectAll();
        WakeUpAll();
        StateAwaitInput();
        UpdateFog();
        MatchStats.UpdateArmyValue((Team)currentTurn, GetArmyValue());
        if(PersistentSettings.gameMode == GameMode.tutorial)
        {
            if((Team)currentTurn != Team.player1) 
            {
                gameUI.BeginTurnTransition();
            }
            if(tutorialPhase == TutorialPhase.phase4 || tutorialPhase == TutorialPhase.phase6) NextTutorialPhase();
        }
    }
    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn > 2)
        {
            currentTurn = 1;
            turnCount++;
        }
    }
    public void Undo()
    {
        MapManager.ins.unitGrid.Remove(_currentSelected.gridpos);
        MapManager.ins.unitGrid.Add(_currentSelected.prevPos, _currentSelected);
        if (_currentSelected) _currentSelected.Undo();
        DeselectAll();
        actionMenu.Clear();
    }
    public int GetArmyValue()
    {
        int value = 0;
        foreach (Unit u in GetAllUnits())
        {
            if ((int)u.team == currentTurn) value += u.data.cost;
        }
        return value;
    }
    public int GetArmyValue(Team team)
    {
        int value = 0;
        foreach (Unit u in GetAllUnits())
        {
            if (u.team == team) value += u.data.cost;
        }
        return value;
    }
    public List<Vector2> GetAllVisibleTiles()
    {
        return allVisibleTiles;
    }
    public int GetTeamFoodCount()
    {
        return MatchStats.currentFood[(Team)currentTurn];        
    }
    private List<Unit> GetAllUnits()
    {
        List<Unit> units = new List<Unit>();
        foreach (KeyValuePair<Vector2, Unit> k in MapManager.ins.unitGrid)
        {
            units.Add(k.Value);
        }
        return units;
    }
    public bool CheckForWin(Team team)
    {
        // check to see if all units on a team are dead
        if (GetArmyValue(team) == 0)
        {
            if(PersistentSettings.gameMode == GameMode.online) 
            {
                if(team == (Team)currentTurn)
                {
                    NextTurn();
                    SocketManager.ins.Send(PacketFactory.BuildGameOver());
                } 
                else SocketManager.ins.Send(PacketFactory.BuildEndTurn());
            }
            MatchStats.UpdateWinner((Team)currentTurn);
            MatchStats.UpdateMapName();
            MatchStats.UpdateTurnCount(turnCount);
            gameUI.BeginEndGameTransition();
            return true;
        }
        return false;

    }
    public void Surrender()
    {
        NextTurn();
        gameUI.BeginEndGameTransition();
        MatchStats.UpdateWinner((Team)currentTurn);
        MatchStats.UpdateMapName();
        if(PersistentSettings.gameMode == GameMode.online) 
        {
            List<Unit> units = GetAllUnits();
            for(int i = units.Count - 1; i >= 0; i--)
            {
                if(units[i].team == clientTeam) MapManager.ins.RemoveUnit(units[i].gridpos);
            }
            SocketManager.ins.Send(PacketFactory.BuildEndTurn());
        }
    }
    private void NextTutorialPhase()
    {
        tutorialPhase++;
        gameUI.UpdateTutorial();
        foreach(KeyValuePair<Vector2, Tile> k in MapManager.ins.currentMap)
        {
            k.Value.UnHighlightGrid();
        }
        var startpos = MapManager.ins.currentMapData.start1.ToVector2();
        switch(tutorialPhase)
        {
            case TutorialPhase.phase2:
                startpos.x -= 1;
                MapManager.ins.currentMap[startpos].HighlightGrid();
                startpos.y -= 1;
                MapManager.ins.currentMap[startpos].HighlightGrid();
                startpos.y += 2;
                MapManager.ins.currentMap[startpos].HighlightGrid();
                break;
            case TutorialPhase.phase3:
                startpos.x -= 2;
                MapManager.ins.currentMap[startpos].HighlightGrid();
                startpos.y -= 1;
                MapManager.ins.currentMap[startpos].HighlightGrid();
                startpos.y += 2;
                MapManager.ins.currentMap[startpos].HighlightGrid();
                break;
            case TutorialPhase.phase4:
                break;
            case TutorialPhase.phase5:
                break;
            case TutorialPhase.phase6:
                break;
            case TutorialPhase.phase7:
                break;
        }
    }
    public void HideLines()
    {
        foreach(KeyValuePair<Vector2, Tile> t in MapManager.ins.currentMap)
        {
            t.Value.HideGrid();
        }
    }
    public void ShowLines()
    {
        foreach(KeyValuePair<Vector2, Tile> t in MapManager.ins.currentMap)
        {
            t.Value.ShowGrid();
        }
    }
    public void CheckTurnOnline()
    {
        if(currentTurn != (int)clientTeam) 
        {
            AllFogOn();
            AllUnitsOff();
            gameUI.HideEndButton();
            gameUI.BeginOnlineTurnTransition();
        }
    }
    public Team OtherTeam()
    {
        if(clientTeam == Team.player1) return Team.player2;
        else return Team.player1;
    }
}