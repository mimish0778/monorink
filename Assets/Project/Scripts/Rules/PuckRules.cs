// パックの速さ、カーブショット、得点と勝敗のルール。UnityEngine に依存しないので単体テストできる
public static class PuckRules
{
    public const float MaxSpeed = 15f;
    public const float MinSpeed = 5f;
    public const float BendCoef = 1f;
    public const int ScoreToWin = 9;

    public const string GoalRedTag = "GoalRed";
    public const string GoalBlueTag = "GoalBlue";

    // パックの速さを上限と下限の間に収める（速さ 0 のときは向きがないので 0 のまま）
    public static float ClampSpeed(float speed)
    {
        if (MaxSpeed < speed)
        {
            return MaxSpeed;
        }
        if (0f < speed && speed < MinSpeed)
        {
            return MinSpeed;
        }
        return speed;
    }

    // カーブショットを打てるか（トリックキーを押していて、カーブ中でなく、残り回数があり、パドルを動かしている）
    public static bool CanBend(bool isTricking, bool isBending, int energy, float moveInput)
        => isTricking && !isBending && 0 < energy && moveInput != 0f;

    // カーブショットでパックに与える縦方向の力。パドルを動かした向きに、パックの速さに比例して曲げる
    public static float BendImpulse(float bendDirection, float puckSpeed)
        => bendDirection * BendCoef * puckSpeed;

    // パックが入ったゴールから、得点するプレイヤーを返す（自分のゴールに入ると相手の得点）。ゴール以外なら null
    public static Side? ScorerForGoal(string goalTag) => goalTag switch
    {
        GoalRedTag => Side.Blue,
        GoalBlueTag => Side.Red,
        _ => null,
    };

    public static bool HasWon(int score) => ScoreToWin <= score;
}

public enum Side
{
    Red,
    Blue,
}
