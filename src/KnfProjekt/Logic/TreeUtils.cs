using AngouriMath;

namespace KnfProjekt.Logic
{
    public static class TreeUtils
    {
        public static Entity UntilStable(
            Entity expr,
            Func<Entity, Entity> rewrite)
        {
            Entity prev;
            do
            {
                prev = expr;
                expr = rewrite(expr);
            }
            while (!expr.Equals(prev));

            return expr;
        }

         public static void Dump(Entity e, int indent = 0)
        {
            Console.WriteLine(new string(' ', indent * 2) + $"{e.GetType().Name} : {e}");
            foreach (var ch in e.DirectChildren)
                Dump(ch, indent + 1);
        }


    }
}
