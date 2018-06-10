using System.Text;
using static System.String;
using static System.Byte;

namespace Benchmarks.Sql
{
    internal static class StoredProcedureHelpers
    {
        private const string Space = " ";
        private static readonly string Comma = "   ,";
        private static readonly string NoComma = "    ";

        public static string Parameters(in SqlParameter[] parameters = default)
        {
            if (parameters == null) return Empty;
            var sb = new StringBuilder();
            for (byte i = MinValue; i < parameters.Length; i++)
            {
                ref readonly var param = ref parameters[i];
                sb.AppendLine(Concat(Indent(in i), param.Name, Space, param.Type));
            }
            return sb.ToString();
            ref readonly string Indent(in byte index)
            {
                if (index == MinValue)
                    return ref NoComma;
                else
                    return ref Comma;
            }
        }
    }
}
