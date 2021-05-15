using System.Data;

namespace MobilOnayService.Models
{
    public class DatabaseParameter
    {
        public DatabaseParameter(string name, object value)
        {
            Name = name;
            Direction = ParameterDirection.Input;
            DataType = DbType.String;
            Size = 2000;
            Value = value;
        }

        public DatabaseParameter(string name, DbType dataType)
        {
            Name = name;
            Direction = ParameterDirection.Output;
            DataType = dataType;
            Size = 2000;
        }

        public DatabaseParameter(string name, DbType dataType, int size)
        {
            Name = name;
            Direction = ParameterDirection.Output;
            DataType = dataType;
            Size = size;
        }

        public DatabaseParameter(string name, object value, DbType dataType)
        {
            Name = name;
            Direction = ParameterDirection.Input;
            DataType = dataType;
            Size = 2000;
            Value = value;
        }

        public DatabaseParameter(string name, object value, DbType dataType, int size)
        {
            Name = name;
            Direction = ParameterDirection.Input;
            DataType = dataType;
            Size = size;
            Value = value;
        }

        public DatabaseParameter(string name, object value, DbType dataType, int size, ParameterDirection direction)
        {
            Name = name;
            Direction = direction;
            DataType = dataType;
            Size = size;
            Value = value;
        }

        public string Name { get; set; }

        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;

        public DbType DataType { get; set; } = DbType.String;

        public int Size { get; set; } = 0;

        public object Value { get; set; }
    }
}
