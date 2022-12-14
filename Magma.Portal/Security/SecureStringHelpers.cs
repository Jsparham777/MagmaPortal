using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Magma.Portal
{
    /// <summary>
    /// Helpers for the <see cref="SecureString"/> class
    /// </summary>
    public static class SecureStringHelpers
    {
        /// <summary>
        /// Unsecures a <see cref="SecureString"/> to plain text
        /// </summary>
        /// <param name="securePassword"></param>
        /// <returns></returns>
        public static string Unsecure(this SecureString secureString)
        {
            //Make sure a secure string is available
            if (secureString == null)
                return string.Empty;

            //Get a pointer for an unsecure string in memory
            var unmanagedString = IntPtr.Zero;

            try
            {
                //Unsecure the password
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);

            }
            finally
            {
                //Clean up any memory allocation
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }

        }
    }
}
