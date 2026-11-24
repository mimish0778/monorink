using NUnit.Framework;

namespace MonoRink.Tests
{
    public class PaddleInputTests
    {
        [TestCase(true, false, 1f)]
        [TestCase(false, true, -1f)]
        [TestCase(false, false, 0f)]
        [TestCase(true, true, 0f)]
        public void OpposingKeysCancelOut(bool positive, bool negative, float expected)
        {
            Assert.That(PaddleInput.ResolveAxis(positive, negative), Is.EqualTo(expected));
        }
    }
}
