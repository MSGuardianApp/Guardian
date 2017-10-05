using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardian.Common.Instrumentation
{
    public interface ILogger
    {
        void TrackEvent(string eventName, IDictionary<string, string> properties = null);

        void TrackException(Exception exception, IDictionary<string, string> properties = null);
    }
}
