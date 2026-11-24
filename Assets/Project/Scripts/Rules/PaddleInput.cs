// キー入力をパドルの移動・回転の向きに変換する。UnityEngine に依存しないので単体テストできる
public static class PaddleInput
{
    // 片方のキーだけが押されていれば +1 か -1、両方押されているか、どちらも押されていなければ 0
    public static float ResolveAxis(bool positiveHeld, bool negativeHeld)
    {
        if (positiveHeld && !negativeHeld)
        {
            return 1f;
        }
        if (negativeHeld && !positiveHeld)
        {
            return -1f;
        }
        return 0f;
    }
}
