public enum GameState
{
    awaitingInput,
    unitSelected,
    awaitingAction,
    awaitingAttack,
    awaitingEat,
    paused,
    awaitingSplit
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
    plain,
    forest,
    mountain,
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
    worker,
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
