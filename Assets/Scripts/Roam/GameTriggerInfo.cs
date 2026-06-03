using UnityEngine;

public class GameTriggerInfo : MonoBehaviour
{
    // GameTriggerInfo.cs
    public enum GameType
    {
        None,        // 默认/漫游/无UI
        RollaBall,   // 篮球弹球游戏
        Tank,        // 坦克游戏
        TableBall,   // 新增桌球小游戏
        CarRace,      // 新增赛车小游戏
        HitBoxes// 跳转场景类型（HitBoxes / ThePlaneWar）可后续加
    }

    public GameType gameType = GameType.None;
}