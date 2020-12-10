using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Advent.Solutions;

namespace Advent.ConsoleApp
{
    public static class ContainerSetup
    {
        public static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<SolutionModule>();
            return builder.Build();
        }
    }
}
