using System;

namespace GTE.Clients
{
    public static class AuthServiceProvider
    {
        private static IAuthService? _instance;

        public static IAuthService Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("AuthService no ha sido registrado. Llame a Register() primero.");
                }
                return _instance;
            }
        }

        public static void Register(IAuthService instance)
        {
            _instance = instance;
        }
    }
}
