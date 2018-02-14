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

public enum Race
{
    ImmuneSys,
    Nanobots,
    Disease
}

public enum Scene
{
    GameScene
}


public enum Team
{
    None = 0,
    Player1 = 1,
    Player2 = 2
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
    original,
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
