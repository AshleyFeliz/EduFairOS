//Ashley Esmirna Feliz Rodríguez 2025-0903
using System;
using System.Linq.Expressions;

namespace EduFairOS.Layers.Domain.Core
{
    public abstract class BaseSpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }

        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
    }
}