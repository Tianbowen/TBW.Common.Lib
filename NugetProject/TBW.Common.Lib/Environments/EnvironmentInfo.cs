using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using TBW.Common.Lib.Extension.AssertExtension;
using TBW.Common.Lib.Extension.StringExtension;

namespace TBW.Common.Lib.Environments
{
    public enum PlatformFamily
    {
        Unknown,
        Windows,
        Linux,
        OSX
    }

    public class EnvironmentInfo
    {
        /// <summary>
        /// Returns whether the operating system is arm64 or not.
        /// </summary>
        public static bool IsArm64 => RuntimeInformation.OSArchitecture == Architecture.Arm64;

        /// <summary>
        /// Returns whether the operating system is 64-bit or not.
        /// </summary>
        public static bool Is64Bit => RuntimeInformation.OSArchitecture == Architecture.X64 ||
                                      RuntimeInformation.OSArchitecture == Architecture.Arm64;

        /// <summary>
        /// Returns whether the operating system is 32-bit or not.
        /// </summary>
        public static bool Is32Bit => !Is64Bit;

        /// <summary>
        /// Returns whether the operating system is a UNIX system.
        /// </summary>
        public static bool IsUnix => Platform == PlatformFamily.Linux ||
                                     Platform == PlatformFamily.OSX;

        /// <summary>
        /// Returns whether the operating system is a Windows system.
        /// </summary>
        public static bool IsWin => !IsUnix;

        /// <summary>
        /// Returns whether the operating system is a Linux system.
        /// </summary>
        public static bool IsLinux => Platform == PlatformFamily.Linux;

        /// <summary>
        /// Returns whether the operating system is a OSX system.
        /// </summary>
        public static bool IsOsx => Platform == PlatformFamily.OSX;

        /// <summary>
        /// Returns whether the operating system is running under Windows Subsystem for Linux.
        /// </summary>
        public static bool IsWsl
        {
            get
            {
                if (!IsLinux)
                    return false;

                try
                {
                    var version = File.ReadAllText("/proc/version");
                    return version.ContainsOrdinalIgnoreCase("Microsoft");
                }
                catch (IOException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns the framework the build is running on.
        /// </summary>
        public static FrameworkName Framework
            => new FrameworkName(Assembly.GetEntryAssembly().NotNull().GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName);

        /// <summary>
        /// Returns the platform the build is running on.
        /// </summary>
        public static PlatformFamily Platform
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return PlatformFamily.OSX;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return PlatformFamily.Linux;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return PlatformFamily.Windows;

                return PlatformFamily.Unknown;
            }
        }
    }
}
