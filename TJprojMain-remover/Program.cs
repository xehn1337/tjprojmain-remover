using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJprojMain_remover
{
    class Program
    {
        private const string AUTOSTART_REGKEY = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";
        private const string AUTOSTART_REGKEY2 = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\RunOnce";

        static void Main(string[] args)
        {
            Log.Info("TJprojMain-remover by xehn1337", ConsoleColor.Blue);
            Console.WriteLine();

            CheckSafeBoot();
            DisableAutostart();

            RemoveFiles();

            Log.Info("Done.");
            Console.ReadLine();
        }

        static void CheckSafeBoot()
        {
            var safeBoot = Utils.IsSafeMode();
            if (!safeBoot)
            {
                Log.Critical("You did not boot into safe mode, which means that the processes cannot be deleted.");
                Log.Info("Here is how to boot into safe mode: https://www.digitalcitizen.life/4-ways-boot-safe-mode-windows-10/");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        static bool DisableAutostart()
        {
            Log.Info("Removing autostart registry keys...");
            Console.WriteLine();
            bool success = false;
            try
            {
              
                success |= Utils.RegRemoveIfExists(AUTOSTART_REGKEY, "svchost");
                success |= Utils.RegRemoveIfExists(AUTOSTART_REGKEY, "Explorer");

                success |= Utils.RegRemoveIfExists(AUTOSTART_REGKEY2, "svchost");
                success |= Utils.RegRemoveIfExists(AUTOSTART_REGKEY2, "Explorer");
                Console.WriteLine();
                
            }
            catch (Exception ex)
            {
                Log.Error("Failed to remove autostart keys: " + ex.Message);
            }
            if (success)
            {
                Log.Info("Removed Autostart keys successfully!");
            }
            return success;
        }

        static void RemoveFiles()
        {
            Console.WriteLine();
            try
            {
                Utils.Unhide(@"C:\Windows\Resources\*.*");
                Utils.Unhide(@"C:\Windows\Resources\Themes\*.*");


                Utils.FRemoveIfExists(@"C:\Windows\Resources\svchost.exe");
                Utils.FRemoveIfExists(@"C:\Windows\Resources\spoolsv.exe");
                Utils.FRemoveIfExists(@"C:\Windows\Resources\Themes\explorer.exe");
                Utils.FRemoveIfExists(@"C:\Windows\Resources\Themes\icsys.icn.exe");
                Utils.FRemoveIfExists(@"C:\Windows\Resources\Themes\icsys.icn");
                Utils.FRemoveIfExists(@"C:\Windows\Resources\Themes\tjcm.cmn");
                Console.WriteLine();

            }catch(Exception ex)
            {
                Log.Error("Could not remove files: " + ex.Message);
            }
        }


    }
}
