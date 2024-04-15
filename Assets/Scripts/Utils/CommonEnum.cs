public enum UILayer
{
    View,    //界面
    Tip    //弹窗
}

public enum TipType
{
    Info = 1,   //游戏介绍
}

public enum OrderInLayer
{
    Loading = -1,
    Default = 0,
    Map = 1,
    Tile = 2,
    Player = 3,
}

public enum GameState
{
    GameBegin = 1,
    GameIn = 2,
    GameOver = 3,
}

public enum SceneType
{
    Home = 1,
    Mine = 2,
}

public enum DoorType
{
    Home = 1,
    Mine = 2,
}

public enum MapIndex
{
    Up = 1,
    Middle = 2,
    Down = 3,
}

public enum GridType
{
    Air = 1,
    Mineral = 2,
    Monster = 3,
}

public enum SpecialMineralId
{
    Stone = 1,
    Soil = 2,
}