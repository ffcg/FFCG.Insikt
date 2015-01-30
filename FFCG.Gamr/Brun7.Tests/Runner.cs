using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFCG.Brun7;
using NUnit.Framework;

namespace Brun7.Tests
{
    [TestFixture]
    public class Runner
    {
        [Test]
        public void SomeTest()
        {
            var bingoGame = new BingoGame(Guid.NewGuid().ToString());

            bingoGame.AddPlayer(new BingoPlayer("conn-id", "Name"));

        }
    }
}
