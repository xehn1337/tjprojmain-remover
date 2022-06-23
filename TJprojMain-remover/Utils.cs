using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TJprojMain_remover
{
    public class Utils
    {
        private const int SM_CLEANBOOT = 67;

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int smIndex);

        public static bool IsSafeMode()
        {
            return GetSystemMetrics(SM_CLEANBOOT) != 0;
        }

        public static bool RegRemoveIfExists(string key, string name)
        {
            using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(key, writable: true))
            {
                if (regKey != null)
                {
                    if (regKey.GetValue(name) != null)
                    {
                        regKey.DeleteValue(name);
                        Log.Critical($"Registry key {name} found and autostart entry removed!");
                        return true;
                    }
                    else
                    {
                        Log.Error($"Registry key {name} not found, searching elsewhere...");
                        return false;
                    }
                }
                else
                {
                    Log.Error("Registry Key not found!");
                    return false;
                }
            }

        }

        public static void Unhide(string path)
        {
            Process.Start("attrib", $"-r -a -s -h \"{path}\"");
        }

        public static void FRemoveIfExists(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    Log.Critical($"Removed {path} successfully!");
                }
                else
                {
                    Log.Error($"File {path} not found!");
                }
            }catch(Exception ex)
            {
                Log.Error($"Could not delete file {path}: {ex.Message}");
            }
        }
    }
}
