using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Octgn.Chat
{
    public class Handshake
    {
        public string Username { get; set; }
        public string Password {
            get {
                IntPtr unmanagedString = IntPtr.Zero;
                try {
                    unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(_password);
                    return Marshal.PtrToStringUni(unmanagedString);
                } finally {
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
                }
            }
            set {
                _password.Clear();
                foreach (char c in value?.ToCharArray()) {
                    _password.AppendChar(c);
                }
            }
        }

        private readonly SecureString _password = new SecureString();
    }

    public class HandshakeResponse
    {
        public string SessionId { get; set; }
    }
}
