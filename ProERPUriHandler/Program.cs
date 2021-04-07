using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace WindowsUriHandler
{
    class Program
    {
        const string URI = @"test";
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    CreateUriRegisterKeyIfNotExists();
                }

                var path = HttpUtility.UrlDecode(args.FirstOrDefault().Replace(URI + "://", String.Empty));
                if (Directory.Exists(path)) {
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace.ToString());
            }
        }

        private static void CreateUriRegisterKeyIfNotExists()
        {
            RelaunchIfNotAdmin();
            using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64))
            using (var subKey = baseKey.OpenSubKey($@"{URI}\shell\open\command"))
            {
                if (subKey is Registry)
                {
                    return;
                }
            }

            RegistryKey key = Registry.ClassesRoot.CreateSubKey(URI);
            key.SetValue("URL Protocol", "");
            key = key.CreateSubKey("shell");
            key = key.CreateSubKey("open");
            key = key.CreateSubKey("command");
            key.SetValue("", System.Reflection.Assembly.GetExecutingAssembly().Location + " %1");
            Console.WriteLine("Registry key created");
            Thread.Sleep(5000);
            System.Environment.Exit(1);
        }

        public static void RelaunchIfNotAdmin()
        {
            if (!RunningAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Assembly.GetEntryAssembly().CodeBase;
                proc.Verb = "runas";
                try
                {
                    Process.Start(proc);
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("This program must be run as an administrator! \n\n" + ex.ToString());
                }
            }
        }

        private static bool RunningAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

}

