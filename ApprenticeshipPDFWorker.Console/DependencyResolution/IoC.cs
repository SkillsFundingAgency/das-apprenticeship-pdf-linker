using StructureMap;

namespace ApprenticeshipPDFWorker.Console.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialise()
        {
            return new Container(c =>
            {
                c.AddRegistry<CoreRegistry>();
                c.AddRegistry<WebRegistry>();
                c.AddRegistry<ComparerRegistry>();
            });
        }
    }
}
