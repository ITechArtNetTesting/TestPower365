using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Utilities
{
    public class FiddlerProxy
    {
        static IList<Session> StoredSessions=new List<Session>();

        static private int proxyPort;

        public static void StartFiddlerProxy(int desiredPort)
        {
            FiddlerCoreStartupFlags flags = FiddlerCoreStartupFlags.Default &
                                            ~FiddlerCoreStartupFlags.RegisterAsSystemProxy;

            FiddlerApplication.Startup(desiredPort, flags);
            proxyPort = FiddlerApplication.oProxy.ListenPort;
        }

        public static void StartWritingRequests()
        {            
            FiddlerApplication.AfterSessionComplete += delegate (Session targetSession)
            {
                StoredSessions.Add(targetSession);
            };
        }

        public static void StopFiddlerProxy()
        {            
            FiddlerApplication.Shutdown();
        }
        
        public static OpenQA.Selenium.Proxy GetProxy()
        {
            if (proxyPort == 0)
            {
                throw new NullReferenceException("You need first StartFiddlerProxy");
            }
            else
            {
                OpenQA.Selenium.Proxy proxy = new OpenQA.Selenium.Proxy();
                proxy.SslProxy = string.Format("127.0.0.1:{0}", proxyPort);
                proxy.HttpProxy = string.Format("127.0.0.1:{0}", proxyPort);
                return proxy;
            }
        }
    }
}
