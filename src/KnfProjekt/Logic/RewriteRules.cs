using AngouriMath;

namespace KnfProjekt.Logic
{
    public static class RewriteRules
    {
        public static Entity RemoveImplications(Entity expr)
        {
            return TreeUtils.UntilStable(expr, e =>
                e.Replace(node =>
                {
                    if (node is Entity.Impliesf imp)
                    {
                        var left = imp.DirectChildren[0];
                        var right = imp.DirectChildren[1];
                        
                         // A → B  ===  ¬A ∨ B
                        return (!left) | right;
                    }

                    return node;
                })
            );
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
