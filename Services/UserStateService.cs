using System.Collections.Concurrent;
using Models;

namespace Services
{
    public static class UserStateService
    {
        public static ConcurrentDictionary<long, LoginState> States = new();
        public static ConcurrentDictionary<long, string> TempLogin = new();
    }
}
