using System;
using ProbeTests.ProbeTests;
using Product.Framework;

namespace ProbeTests
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length<1)
            {
                Console.WriteLine("Please specify probe type as the first argument");
                return;
            }
            IProbeTest probeTest=null;
            try
            {
                if (args[0]=="migration")
                {
                    probeTest = new ProbeMigrationTest();
                }
                else if (args[0] == "discovery")
                {
                    probeTest = new ProbeDiscoverAndMatchingTest();
                }
                else if (args[0] == "integration")
                {
                    probeTest = new ProbeIntegrationTest();
                }
                else if (args[0] == "ars")
                {
                    probeTest = new ArsProbeTest();
                }
                else if (args[0] == "cdsp2p")
                {
                    probeTest = new CDSP2PTest();
                }
                else if (args[0] == "discoveryqueue")
                {
                    probeTest = new DiscoveryQueueTest();
                }
                else if (args[0] == "cdsc2c")
                {
                    probeTest = new CDSC2CTest();
                }
                else
                {
                    Console.WriteLine("Unknown probe type");
                    return;
                }
                probeTest.SetUp();
                probeTest.Run();

            }
            catch (Exception e)
            {
                Console.WriteLine("FATAL ERROR DURING SETUP OR CONSTRUCTOR OCCURED");
                Console.WriteLine(e.ToString());
            }
            finally
            {
                probeTest?.TearDown();
            }
        }
    }
}
