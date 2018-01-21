public enum GameState
{
    awaitingInput,
    unitSelected,
    awaitingAction,
    paused
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
    plain = 1,
    forest = 2,
    mountain = 3,
    food = 4
}

public enum Action
{
    move,
    eat,
    attack,
    split
}
