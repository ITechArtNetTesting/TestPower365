using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeTests.ProbeTests
{
    public interface IProbeTest
    {
        void SetUp();
        void Run();
        void TearDown();
    }
}
