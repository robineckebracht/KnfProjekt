using AngouriMath;

namespace KnfProjekt.Logic
{
    public static class RewriteRules
    {
        public static Entity RemoveImplications(Entity expr)
        {
            // kommt als erstes
            return expr;
        }

        public static Entity PushNegations(Entity expr)
        {
            // De Morgan, Doppelnegation
            return expr;
        }

        public static Entity DistributeOrOverAnd(Entity expr)
        {
            // A or (B and C)
            return expr;
        }
    }
}
