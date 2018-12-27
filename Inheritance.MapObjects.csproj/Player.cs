using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    public class Player
    {
        public int Gold { get; private set; }
        public bool Dead { get; private set; }


        public bool CanBeat(Army army)
        {
            return army.Power < 5;
        }
            
        public int Id { get; set; }

        public void Consume(Treasure treasure)
        {
            Gold += treasure.Amount;
        }

        public void Die()
        {
            Dead = true;
        }
    }
}
