using AngouriMath;

namespace KnfProjekt.Logic
{
    public static class KnfPipeline
    {
        public static Entity ToKnf(Entity expr)
        {
            expr = RewriteRules.RemoveImplications(expr);
            expr = RewriteRules.PushNegations(expr);
            expr = RewriteRules.DistributeOrOverAnd(expr);

            return expr;
        }
    }
}
