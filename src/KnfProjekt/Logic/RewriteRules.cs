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
            return TreeUtils.UntilStable(expr, e =>
                e.Replace(node =>
                {
            
                    if (node is not Entity.Notf notNode)
                        return node;

                    var child = notNode.DirectChildren[0];

                    // 1) Doppelnegation: ¬(¬A)  =>  A
                    if (child is Entity.Notf innerNot)
                        return innerNot.DirectChildren[0];

                    // 2) De Morgan für AND: ¬(A1 ∧ A2 ∧ ... ∧ An)  =>  ¬A1 ∨ ¬A2 ∨ ... ∨ ¬An
                    if (child is Entity.Andf andNode)
                    {
                        var negatedChildren = andNode.DirectChildren.Select(c => (Entity)(!c)).ToList();
                        return OrAll(negatedChildren);
                    }

                    // 3) De Morgan für OR: ¬(A1 ∨ A2 ∨ ... ∨ An)  =>  ¬A1 ∧ ¬A2 ∧ ... ∧ ¬An
                    if (child is Entity.Orf orNode)
                    {
                        var negatedChildren = orNode.DirectChildren.Select(c => (Entity)(!c)).ToList();
                        return AndAll(negatedChildren);
                    }

                    return node;
                })
            );
        }

        private static Entity AndAll(IReadOnlyList<Entity> parts)
        {
            if (parts.Count == 0) return "true";
            if (parts.Count == 1) return parts[0];

            Entity acc = parts[0];
            for (int i = 1; i < parts.Count; i++)
                acc = acc & parts[i];
            return acc;
        }

        private static Entity OrAll(IReadOnlyList<Entity> parts)
        {
            if (parts.Count == 0) return "false";
            if (parts.Count == 1) return parts[0];

            Entity acc = parts[0];
            for (int i = 1; i < parts.Count; i++)
                acc = acc | parts[i];
            return acc;
        }

        public static Entity DistributeOrOverAnd(Entity expr)
        {
            return TreeUtils.UntilStable(expr, e =>
                e.Replace(node =>
                {
                    if (node is not Entity.Orf orNode)
                        return node;
 
                    var andChild = orNode.DirectChildren.FirstOrDefault(ch => ch is Entity.Andf);
                    if (andChild is null)
                        return node; 

                    var andNode = (Entity.Andf)andChild;

                    // "Rest" = OR aller anderen Kinder (alles außer dem AND-Kind)
                    // Beispiel: (X or (A and B)) -> Rest = X
                    // Beispiel: (p or q or (A and B)) -> Rest = (p or q)
                    var restParts = orNode.DirectChildren.Where(ch => !ReferenceEquals(ch, andChild)).ToList();
                    var restOr = OrAll(restParts);

                    // Distributiv:
                    // restOr OR (A1 AND A2 ... AND An)
                    // => (restOr OR A1) AND (restOr OR A2) ... AND (restOr OR An)
                    var distributedClauses =
                        andNode.DirectChildren
                               .Select(a => (Entity)(restOr | a))
                               .ToList();

                    return AndAll(distributedClauses);
                })
            );
        }
    }

        
}
