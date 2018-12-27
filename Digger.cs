using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    class Checker
    {
        public static bool IsInsideMap(int x, int y)
        {
            return x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight;
        }

        public static bool IsEmpty(int x, int y)
        {
            return Game.Map[x, y] == null;
        }
    }

    public class Player : ICreature
    {
        const string Image = "Digger.png";
        const int Priority = 0;
        
        public CreatureCommand Act(int x, int y)
        {
            var result = new CreatureCommand();
            int delta_x = 0;
            int delta_y = 0;
            switch (Game.KeyPressed)
            {
                case Keys.Right:
                    delta_x = 1;
                    break;
                case Keys.Left:
                    delta_x = -1;
                    break;
                case Keys.Up:
                    delta_y = -1;
                    break;
                case Keys.Down:
                    delta_y = 1;
                    break;
            }
            if (CanMoveTo(x + delta_x, y + delta_y))
            {
                result.DeltaX = delta_x;
                result.DeltaY = delta_y;
                return result;
            }
            else
            {
                return result;
            }
        }

        bool CanMoveTo(int x, int y)
        {
            return Checker.IsInsideMap(x, y) && !(Game.Map[x, y] is Sack);
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return Priority;
        }

        public string GetImageFileName()
        {
            return Image;
        }
    }

    public class Terrain : ICreature
    {
        const string Image = "Terrain.png";
        const int Priority = 100;
   
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return Priority;
        }

        public string GetImageFileName()
        {
            return Image;
        }
    }

    public class Sack : ICreature
    {
        const string Image = "Sack.png";
        const int Priority = 70;
        public int CellsFlied = 0;

        public CreatureCommand Act(int x, int y)
        {
            var result = new CreatureCommand();
            if (IsBlockedByCreatureBelow(x, y))
            {
                return result;
            }
            if (CanFallDown(x, y))
            {
                TryKillCreature(Game.Map[x, y + 1]);
                this.CellsFlied++;
                result.DeltaY = 1;
                return result;
            }
            return TryTransformToGold(result);
        }

        bool IsCellPlayerOrMonster(int x, int y)
        {
            return Game.Map[x, y] is Player || Game.Map[x, y] is Monster;
        }

        bool IsBlockedByCreatureBelow(int x, int y)
        {
            return Checker.IsInsideMap(x, y + 1) && IsCellPlayerOrMonster(x, y + 1) && this.CellsFlied == 0;
        }

        bool CanFallDown(int x, int y)
        {
            return Checker.IsInsideMap(x, y + 1) && (Checker.IsEmpty(x, y + 1) || IsCellPlayerOrMonster(x, y + 1));
        }

        void TryKillCreature(ICreature creature)
        {
            if (creature is Player)
            {
                Game.IsOver = true;
                creature = null;
            }
            else if (creature is Monster)
            {
                creature = null;
            }
        }

        CreatureCommand TryTransformToGold(CreatureCommand result)
        {
            if (this.CellsFlied > 1)
            {
                result.TransformTo = new Gold();
            }
            this.CellsFlied = 0;
            return result;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return Priority;
        }

        public string GetImageFileName()
        {
            return Image;
        }
    }

    public class Gold : ICreature
    {
        const string Image = "Gold.png";
        const int Priority = 50;
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
            {
                Game.Scores += 10;
            }
            return true;
        }

        public int GetDrawingPriority()
        {
            return Priority;
        }

        public string GetImageFileName()
        {
            return Image;
        }
    }

    class Vector
    {
        public int X;
        public int Y;
        public Vector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    class Monster : ICreature
    {
        const string Image = "Monster.png";
        const int Priority = 5;

        Vector GetDiggerPosition()
        {
            var diggerPosition = new Vector(-1, -1);
            for (int i = 0; i < Game.MapWidth; i++)
            {
                for (int j = 0; j < Game.MapHeight; j++)
                {
                    if (Game.Map[i, j] is Player)
                    {
                        diggerPosition.X = i;
                        diggerPosition.Y = j;
                        break;
                    }
                }
                if (diggerPosition.X > -1)
                {
                    break;
                }
            }
            return diggerPosition;
        }

        public CreatureCommand Act(int x, int y)
        {
            var diggerPosition = GetDiggerPosition();
            var result = new CreatureCommand();
            if (diggerPosition.X == -1)
            {
                return result;
            }
            var distanceToDigger = new Vector(diggerPosition.X - x, diggerPosition.Y - y);
            if (distanceToDigger.X > 0 && CanMoveTo(x + 1, y))
            {
                result.DeltaX = 1;
            }
            else if (distanceToDigger.X < 0 && CanMoveTo(x - 1, y))
            {
                result.DeltaX = -1;
            }
            if (distanceToDigger.Y > 0 && CanMoveTo(x, y + 1))
            {
                result.DeltaY = 1;
            }
            else if (distanceToDigger.Y < 0 && CanMoveTo(x, y - 1))
            {
                result.DeltaY = -1;
            }
            if (result.DeltaX != 0 && result.DeltaY != 0)
            {
                result.DeltaY = 0;
            }
            return result;
        }

        bool CanMoveTo(int x, int y)
        {
            return Checker.IsInsideMap(x, y) && (Checker.IsEmpty(x,y) || IsGoldOrPlayer(x,y));
        }
       
        bool IsGoldOrPlayer(int x,int y)
        {
            return Game.Map[x, y] is Gold || Game.Map[x, y] is Player;
        }

        void TryKillGoldOrGameOver(int x, int y)
        {
            if (Game.Map[x, y] is Player)
            {
                Game.IsOver = true;
            }
            else if (Game.Map[x, y] is Gold)
            {
                Game.Map[x, y] = null;
            }
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return Priority;
        }

        public string GetImageFileName()
        {
            return Image;
        }
    }
}
