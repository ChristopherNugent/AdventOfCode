using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using System.Linq;
using Advent.Solutions.Interfaces;

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
