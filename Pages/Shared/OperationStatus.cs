using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{
    public class OperationStatus
    {
        public static OperationStatus OK { get; } = new OperationStatus(0, "OK");
        public static OperationStatus BadRequest { get; } = new OperationStatus(1, "BadRequest");
        public static OperationStatus NotFound { get; } = new OperationStatus(2, "NotFound");
        public static OperationStatus ServerError { get; } = new OperationStatus(3, "ServerError");
        private OperationStatus(int value, string name)
        {
            Value = value;
            Name = name;
        }
        public string Name { get; set; }
        public int Value { get; set; }
        public string ReasonForFailure { get; set; }
        public static IEnumerable<OperationStatus> List()
        {
            return new[] { OK, BadRequest, NotFound };
        }
        public static OperationStatus FromString(string status)
        {
            return List().Single(s => String.Equals(s.Name, status, StringComparison.OrdinalIgnoreCase));
        }
        public static OperationStatus FromValue(int value)
        {
            return List().Single(s => s.Value == value);
        }
    }
}
