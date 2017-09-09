using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripting
{
    public interface Broadcaster
    {
        void Ack(object source);
        void Bye(object source);
    }
}
