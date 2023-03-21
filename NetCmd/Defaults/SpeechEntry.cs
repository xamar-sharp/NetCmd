using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal sealed class SpeechEntry : IEntry
    {
        public string CommandName { get; } = "speech";
        public string HelpText { get; } = "Text to speech service.\n It has 1 parameters: \n 1 - Your custom text for speech";
        public int ParameterCount { get; } = 1;
        public ConsoleColor HelpColor { get; } = ConsoleColor.Magenta;
#pragma warning disable CA1416
        private readonly SpeechSynthesizer _synthesizer = new SpeechSynthesizer();
        ~SpeechEntry()
        {
            _synthesizer.Dispose();
        }
        public void React(string[] args)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Print("Service supports only in Windows!!!");
            }
            else
            {
                _synthesizer.SpeakAsync(args[0]);
            }
            StartupProgresser.Current.Notify(CommandName, 2);
        }
        public void Print(string msg)
        {
            Console.ForegroundColor = HelpColor;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
