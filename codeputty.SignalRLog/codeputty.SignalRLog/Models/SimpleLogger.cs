using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;
using SignalR;
using SignalR.Infrastructure;
using SignalR.Hosting.AspNet;

namespace codeputty.SignalRLog.Models
{
    public interface ISimpleLogger {
        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);
        void Log(string level, string message);
    }
    public class SimpleHubLogger<T> : ISimpleLogger where T: Hub
    {
        public class LogItem
        {
            public string Level { get; set; }
            public DateTime Timestamp { get; set; }
            public string Message { get; set; }

            public LogItem()
            {
                Timestamp = DateTime.UtcNow;
            }
        }
        public void Trace(string message)
        {
            Log("trace", message);
        }
        public void Info(string message)
        {
            Log("info", message);
        }
        public void Debug(string message)
        {
            Log("debug", message);
        }
        public void Warn(string message)
        {
            Log("warn", message);
        }
        public void Error(string message)
        {
            Log("error", message);
        }
        public void Fatal(string message)
        {
            Log("fatal", message);
        }
        public void Log(string loglevel, string message)
        {
            Log(new LogItem() { Level = loglevel, Message = message });
        }
        private void Log(LogItem item)
        {
            IConnectionManager connectionManager = AspNetHost.DependencyResolver.Resolve<IConnectionManager>();
            dynamic clients = connectionManager.GetClients<LogEndpoint>();
            clients[item.Level].receiveLogEntry(item);
        }
    }
}