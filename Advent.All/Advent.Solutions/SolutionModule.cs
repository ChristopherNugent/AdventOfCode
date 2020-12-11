using Advent.Solutions.Interfaces;
using Autofac;
using System.Linq;

namespace Advent.Solutions
{
    public class SolutionModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterAssemblyTypes(typeof(SolutionModule).Assembly)
                .Where(t => typeof(IAdventSolution).IsAssignableFrom(t))
                .AsImplementedInterfaces();
        }
    }
}
