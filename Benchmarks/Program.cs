using System.Threading.Tasks;
using static BenchmarkDotNet.Running.BenchmarkRunner;
using static Benchmarks.DatabasePreparer;

namespace Benchmarks
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await PrepareDatabase();
            Run<Tests>();
        }
    }
}
