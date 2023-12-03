using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Base.Message
{
    public abstract class BaseMessage
    {
        public long Id { get; set; }
        public DateTime MessageCreated { get; set; }
        public string AccessToekn { get; set; }
    }
}
