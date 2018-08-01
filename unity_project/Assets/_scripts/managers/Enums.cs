// public enum GameState
// {
//     awaitingInput,
//     unitSelected,
//     awaitingAction,
//     awaitingAttack,
//     awaitingEat,
//     paused,
//     awaitingSplit,
//     animating
// }

public enum UnitState
{
    idle,
    selected,
    awaitingAction,
    sleeping,
    dead
}
public enum GameMode
{
    online,
    tutorial,
    local
}
public enum Team
{
    None = 0,
    player1 = 1,
    player2 = 2
}

public enum TileType
{
    none,
    grassMid,
    wall,
    food,
    grassCorner,
    grassEdge,
    water
}

public enum Action
{
    move,
    eat,
    attack,
    split
}
public enum UnitType 
{
    worker,
    scout,
    soldier,
    sniper,
    tank
}
public enum Icon 
{
    wait,
    eat,
    attack,
    split
}
public enum TutorialPhase
{
    phase1,
    phase2,
    phase3,
    phase4,
    phase5,
    phase6,
    phase7
}
