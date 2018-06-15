using Benchmarks.Model;

namespace Benchmarks
{
    public static class Constants
    {
        public const string BytesCommand = "[dbo].[NotNC_Bytes]";
        public const string Command = "[dbo].[NotNC_AsInt]";
        public const string ExecBytesCommand = "EXEC [dbo].[NotNC_Bytes] ";
        public const string ExecCommand = "EXEC [dbo].[NotNC_AsInt] ";
        public static readonly EntityA Entity = new EntityA { EntityBId = 5, EntityCId = 5 };

        public const string DatabaseUserName = "SA";
        public const string DatabasePassword = "DevelopmentP@ssw0rd";
        public const string DatabaseDbName = "Benchmarks-53AB33DA-5FC4-4442-911B-DDEA265DAAB7";
        public const string DatabaseHostName = "localhost";

        public static readonly string ConnectionString = string.Concat("Persist Security Info=False;Packet Size=512;Server=", DatabaseHostName,
            ";User ID=", DatabaseUserName,
            ";Password=", DatabasePassword,
            ";Initial Catalog=", DatabaseDbName, ";");
    }
}
