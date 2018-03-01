public enum GameState
{
    awaitingInput,
    unitSelected,
    awaitingAction,
    awaitingAttack,
    awaitingEat,
    paused,
    awaitingSplit,
    animating
}

public enum UnitState
{
    idle,
    selected,
    awaitingAction,
    sleeping,
    dead
}

public enum Scene
{
    game,
    options,
    postgame,
    pregame,
    title
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
    open,
    blocking,
    food
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
    queen,
    scout,
    infantry,
    sniper,
    tank,
}
public enum Icon 
{
    wait,
    eat,
    attack,
    split
}
