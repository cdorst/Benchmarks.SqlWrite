using System.IO;
using System.Reflection;

namespace Benchmarks.Sql
{
    internal static class ResourceConstants
    {
        private static readonly string Is_NC_AsInt = "Benchmarks.Sql.Is_NC_AsInt";
        private static readonly string Is_NC_Bytes = "Benchmarks.Sql.Is_NC_Bytes";
        private static readonly string NotNC_AsInt = "Benchmarks.Sql.NotNC_AsInt";
        private static readonly string NotNC_Bytes = "Benchmarks.Sql.NotNC_Bytes";

        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

        public static string GetNativeCompiled_AsInt_SqlText() => GetSqlText(in Is_NC_AsInt);
        public static string GetNativeCompiled_Bytes_SqlText() => GetSqlText(in Is_NC_Bytes);
        public static string GetNotNativeCompiled_AsInt_SqlText() => GetSqlText(in NotNC_AsInt);
        public static string GetNotNativeCompiled_Bytes_SqlText() => GetSqlText(in NotNC_Bytes);

        private static string GetSqlText(in string resource)
        {
            using (var stream = Assembly.GetManifestResourceStream(resource))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
