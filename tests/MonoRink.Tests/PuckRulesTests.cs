using NUnit.Framework;

namespace MonoRink.Tests
{
    public class PuckRulesTests
    {
        [TestCase(20f, PuckRules.MaxSpeed)]
        [TestCase(15f, 15f)]
        [TestCase(10f, 10f)]
        [TestCase(5f, 5f)]
        [TestCase(2f, PuckRules.MinSpeed)]
        public void SpeedIsKeptBetweenTheLimits(float speed, float expected)
        {
            Assert.That(PuckRules.ClampSpeed(speed), Is.EqualTo(expected));
        }

        [Test]
        public void StoppedPuckIsLeftAlone()
        {
            // 得点直後はパックを止めて中央に戻すので、速さ 0 を下限まで引き上げてはいけない
            Assert.That(PuckRules.ClampSpeed(0f), Is.EqualTo(0f));
        }

        [Test]
        public void BendNeedsTrickKeyEnergyAndPaddleMovement()
        {
            Assert.That(PuckRules.CanBend(isTricking: true, isBending: false, energy: 3, moveInput: 1f), Is.True);
            Assert.That(PuckRules.CanBend(isTricking: true, isBending: false, energy: 1, moveInput: -1f), Is.True);

            Assert.That(PuckRules.CanBend(isTricking: false, isBending: false, energy: 3, moveInput: 1f), Is.False, "トリックキーを押していない");
            Assert.That(PuckRules.CanBend(isTricking: true, isBending: true, energy: 3, moveInput: 1f), Is.False, "カーブ中");
            Assert.That(PuckRules.CanBend(isTricking: true, isBending: false, energy: 0, moveInput: 1f), Is.False, "残り回数なし");
            Assert.That(PuckRules.CanBend(isTricking: true, isBending: false, energy: 3, moveInput: 0f), Is.False, "パドルが止まっている");
        }

        [Test]
        public void BendPushesThePuckTheWayThePaddleMoved()
        {
            Assert.That(PuckRules.BendImpulse(1f, 10f), Is.EqualTo(10f));
            Assert.That(PuckRules.BendImpulse(-1f, 10f), Is.EqualTo(-10f));
            Assert.That(PuckRules.BendImpulse(1f, 5f), Is.LessThan(PuckRules.BendImpulse(1f, 15f)), "速いパックほど大きく曲がる");
        }

        [Test]
        public void GoalInYourOwnNetScoresForTheOpponent()
        {
            Assert.That(PuckRules.ScorerForGoal(PuckRules.GoalRedTag), Is.EqualTo(Side.Blue));
            Assert.That(PuckRules.ScorerForGoal(PuckRules.GoalBlueTag), Is.EqualTo(Side.Red));
        }

        [TestCase("Player")]
        [TestCase("Wall")]
        [TestCase("Untagged")]
        public void HittingAnythingElseScoresNothing(string tag)
        {
            Assert.That(PuckRules.ScorerForGoal(tag), Is.Null);
        }

        [Test]
        public void TheFirstToNinePointsWins()
        {
            Assert.That(PuckRules.HasWon(8), Is.False);
            Assert.That(PuckRules.HasWon(9), Is.True);
        }
    }
}
