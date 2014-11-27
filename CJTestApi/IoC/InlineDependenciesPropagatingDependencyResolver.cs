using System;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Resolvers;

namespace CJTestApi.IoC
{
    public class InlineDependenciesPropagatingDependencyResolver :
        DefaultDependencyResolver
    {
        protected override CreationContext RebuildContextForParameter(
            CreationContext current,
            Type parameterType)
        {
            if (parameterType.ContainsGenericParameters)
            {
                return current;
            }

            return new CreationContext(parameterType, current, true);
        }
    }
}