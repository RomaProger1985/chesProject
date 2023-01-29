using System.Collections.Generic;

namespace ChessRules
{
    struct Squer
    {
        public static Squer none = new Squer(-1, -1);
        public int x { get; private set; }
        public int y { get; private set; }
        public Squer(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Squer(string e2)
        {
            if (e2.Length == 2 &&
                e2[0] >= 'a' && e2[0] <= 'h' &&
                e2[1] >= '1' && e2[1] <= '8')
            {
                x = e2[0] - 'a';
                y = e2[1] - '1';
            }
            else
                this = none;
        }

        public bool OnBoard()
        {
            return x >= 0 && x < 8 &&
                y >= 0 && y < 8;
        }
        
        public string Name { get {return ((char)('a' + x)).ToString() + (y + 1).ToString(); } }

        public static bool operator ==(Squer a, Squer b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Squer a, Squer b)
        {
            return !(a == b);
        }

        public static IEnumerable<Squer> YieldSquares()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    yield return new Squer(x, y);
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Squer))
            {
                return false;
            }

            var squer = (Squer)obj;
            return x == squer.x &&
                   y == squer.y &&
                   Name == squer.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = 655456401;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
