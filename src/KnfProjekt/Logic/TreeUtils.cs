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
    }
}
