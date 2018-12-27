
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    [TestFixture]
    public class Player_should
    {
        [Test]
        public void TakeResources()
        {
            var player = new Player();
            Interaction.Make(player, new ResourcePile { Treasure = new Treasure { Amount = 10 } });
            Assert.AreEqual(false, player.Dead);
            Assert.AreEqual(10, player.Gold);
        }

        [Test]
        public void BeatWeakCreepsAndCollectTreasure()
        {
            var player = new Player();
            Interaction.Make(player, new Creeps { Treasure = new Treasure { Amount = 10 }, Army = new Army { Power = 1 } });
            Assert.AreEqual(false, player.Dead);
            Assert.AreEqual(10, player.Gold);
        }

        [Test]
        public void LoseToPowerfulCreeps()
        {
            var player = new Player();
            Interaction.Make(player, new Creeps { Treasure = new Treasure { Amount = 10 }, Army = new Army { Power = 10 } });
            Assert.AreEqual(true, player.Dead);
            Assert.AreEqual(0, player.Gold);
        }

        [Test]
        public void OwnDwelling()
        {
            var player = new Player { Id = 1 };
            var dwelling = new Dwelling();
            Interaction.Make(player, dwelling);
            Assert.AreEqual(false, player.Dead);
            Assert.AreEqual(player.Id, dwelling.Owner);
            Assert.AreEqual(0, player.Gold);
        }

        [Test]
        public void BeatWeakGuandAndOwnDwelling()
        {
            var player = new Player { Id = 1 };
            var mine = new Mine { Army = new Army { Power = 1 } };
            Interaction.Make(player, mine);
            Assert.AreEqual(false, player.Dead);
            Assert.AreEqual(player.Id, mine.Owner);
            Assert.AreEqual(0, player.Gold);
        }

        [Test]
        public void LoseToPowerfuleGuand()
        {
            var player = new Player { Id = 1 };
            var mine = new Mine { Army = new Army { Power = 10 } };
            Interaction.Make(player, mine);
            Assert.AreEqual(true, player.Dead);
            Assert.AreEqual(0, mine.Owner);
            Assert.AreEqual(0, player.Gold);
        }


    }
}
