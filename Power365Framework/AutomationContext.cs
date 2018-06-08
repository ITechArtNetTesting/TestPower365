using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Services;
using BinaryTree.Power365.AutomationFramework.Workflows;
using System;
using System.Collections.Concurrent;

namespace BinaryTree.Power365.AutomationFramework
{
    public class AutomationContext
    {
        public Settings Settings { get; private set; }
        public BrowserAutomation Browser { get { return _browser ?? (_browser = new BrowserAutomation(Settings, _context)); } }
        
        public CommonWorkflow Common { get{ return _common ?? (_common = new CommonWorkflow(Browser.Navigate<HomePage>(Settings.BaseUrl), _browser.WebDriver)); } }

        private BrowserAutomation _browser;
        private CommonWorkflow _common;

        private readonly object _context;

        private ConcurrentDictionary<Type, object> _serviceCache = new ConcurrentDictionary<Type, object>();
        
        public AutomationContext(Settings settings, object context = null)
        {
            Settings = settings;
            _context = context;
        }

        public void RegisterService<T>(T service)
        {
            if (_serviceCache.ContainsKey(typeof(T)))
                throw new Exception("Service already registered");

            _serviceCache.AddOrUpdate(typeof(T), service, (t, existing) =>
            {
                return service;
            });
        }

        public T GetService<T>()
        {
            object service;
            if (!_serviceCache.TryGetValue(typeof(T), out service))
                throw new Exception("Could not get service");

            return (T)service;
        }

        public void ResetBrowser()
        {
            if (_browser != null)
            {
                _browser.Dispose();
                _browser = null;
            }
        }

        public void ResetCommonWorkflow()
        {
            _common = null;
        }

        public void Dispose()
        {
            if(_browser != null)
                _browser.Dispose();
            
            foreach (var serviceKey in _serviceCache.Keys)
            {
                object service;
                if (_serviceCache.TryRemove(serviceKey, out service))
                {
                    IDisposable disposableService = service as IDisposable;
                    if (disposableService != null)
                        disposableService.Dispose();
                }
            }
        }
    }
}
