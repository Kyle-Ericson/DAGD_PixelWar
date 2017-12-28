public enum GameState
{
    // idle state where nothing is selected, waiting for player input
    awaitingInput = 0,
    // a unit is selected, waiting to decide what to do with that unit
    unitSelected = 1,
}
