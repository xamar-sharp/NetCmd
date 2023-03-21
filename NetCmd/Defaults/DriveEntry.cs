using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal class DriveEntry : IEntry
    {
        public int ParameterCount { get; } = 1;//drive name or _ (all)
        public string CommandName { get; } = "drive";
        public string HelpText { get; } = "it`s command for reading information about drives on your machine\nIt has 1 parameters:\n 1 - drive_name OR _ (all drives)";
        public ConsoleColor HelpColor { get; } = ConsoleColor.Green;
        public void React(string[] args)
        {
            Print("About Drives:");
            if(args[0] == "_")
            {
                FormatInfoAboutAllDisks();
            }
            else
            {
                FormatInfoAboutSingleDisk(DriveInfo.GetDrives().First(drive=>drive.Name == args[0]));
            }
            StartupProgresser.Current.Notify(CommandName, 2);
        }
        void FormatInfoAboutSingleDisk(DriveInfo drive)
        {
            Print($"Drive name: {drive.Name}");
            if (drive.IsReady)
            {
                Print($"Free space: {drive.AvailableFreeSpace / (1024.0 * 1024)} Mbytes");
                Print($"Total space: {drive.TotalSize / (1024.0 * 1024)} Mbytes");
                Print($"Drive format: {drive.DriveFormat}");
                Print($"Drive type: {drive.DriveType}");
            }
        }
        void FormatInfoAboutAllDisks()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                Print("|*************************|");
                FormatInfoAboutSingleDisk(drive);
            }
        }
        public void Print(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
