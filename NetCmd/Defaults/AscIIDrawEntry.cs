using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal sealed class AscIIDrawEntry : IEntry
    {
        public string CommandName { get; } = "draw";
        public string HelpText { get; } = "Draw ASC-II art to console, based on special markup syntax.\nSyntax: {amount of repeating symbol}{special-symbol-code}.{etc...}\nSpecial symbol codes:\n" +
            "BL - black cell;B - blue cell;G - green cell;R - red cell;C - cyan cell;GR - gray cell\n" + "Y - yellow cell;W - white cell;M - magenta cell;D[B,C,M,G,A,R] - dark [blue,cyan,etc...] cell;\n"+
            "NL - next line symbol; Another symbol or #[B,G,R,C,M,Y,W] will interprete as raw ASC-II symbol."+
            "\nIt has 2 parameters:\n 1 - text or relative file name(without extension!)(extension should be .ascdraw!) for draw-output\n 2 - F(if 1 parameter is file name), Other if text.\n 3 - If 1 parameter is text, this is file name to load picture into filesystem.\n File name without extension! Maybe _ if don`t used!";
        public int ParameterCount { get; } = 3;
        public ConsoleColor HelpColor { get; } = ConsoleColor.DarkCyan;
        private readonly AscIIDrawer _drawer = new AscIIDrawer();
        public void React(string[] args)
        {
            lock (this)
            {
                Console.WriteLine();
                if (args[1] == "F")
                {
                    _drawer.SetFile(Path.Combine(IStartup.Current.CurrentDirectory, args[0]+".ascdraw")).DrawArt();
                }
                else
                {
                    var formatted = args[0].ToUpper();
                    _drawer.SetText(formatted).DrawArt();
                    if(args[2] != "_")
                    {
                        File.WriteAllText(Path.Combine(IStartup.Current.CurrentDirectory, args[2] + ".ascdraw"), formatted);
                    }
                }
                StartupProgresser.Current.Notify(CommandName, 2);
            }
        }
        public void Print(string msg)
        {
            Console.ForegroundColor = HelpColor;
            Console.WriteLine(CommandName+":"+msg);
            Console.ResetColor();
        }
    }
    internal sealed class AscIIDrawer
    {
        public string Data { get; private set; } = String.Empty;
        public AscIIDrawer()
        {

        }
        public AscIIDrawer(string src, bool isFile)
        {
            if (isFile)
                SetFile(src);
            else
                SetText(src);
        }
        public AscIIDrawer SetFile(string fileName)
        {
            Data = File.ReadAllText(fileName);
            return this;
        }
        public AscIIDrawer SetText(string txt)
        {
            Data = txt;
            return this;
        }
        public AscIIDrawer DrawArt()
        {
            foreach(string chunk in Data.Split('.'))
            {
                RenderChunk(chunk);
            }
            return this;
        }
        public void RenderChunk(string chunk)
        {
            string amount = string.Empty;
            string specialCode = string.Empty;
            for(int x = 0; x < chunk.Length; x++)
            {
                int currentCharacterCode = chunk[x];
                if (currentCharacterCode > 47 && currentCharacterCode < 58)
                {
                    amount += chunk[x];
                }
                else
                {
                    specialCode += chunk[x];
                }
            }
            char intentedChar = RenderMap.SetupOutput(specialCode);
            for(int x = 0; x < int.Parse(amount); x++)
            {
                Console.Write(intentedChar);
            }
            Console.ResetColor();
        }
        public static implicit operator AscIIDrawer(string src)
        {
            return new AscIIDrawer() { Data = src };
        }
    }
}
