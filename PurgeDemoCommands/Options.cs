﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace PurgeDemoCommands
{
    internal class Options
    {
        [ValueList(typeof(List<string>))]
        public IList<string> Files { get; set; }

        [Option('s', "suffix", DefaultValue = "_purged", HelpText = "suffix of generated file")]
        public string Suffix { get; set; }

        [Option('t', "skipTest", DefaultValue = false, HelpText = "skips test if purged file can be parsed again")]
        public bool SkipTest { get; set; }

        [Option('o', "overwrite", DefaultValue = false, HelpText = "overwrites existing (purged) files")]
        public bool Overwrite { get; set; }

        [Option('p', "successfullPurges", DefaultValue = false, HelpText = "shows a summary after purgeing")]
        public bool ShowSummary { get; set; }

        [HelpOption(HelpText = "Display this help screen.")]
        public string GetUsage()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            var help = new HelpText
            {
                Heading = new HeadingInfo("PurgeDemoCommands", version.ToString()),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine(" ");
            help.AddPreOptionsLine("Usage: PurgeDemoCommands.exe awesome.dem other.dem");
            help.AddPreOptionsLine("       PurgeDemoCommands.exe \"C:\\path\\to\\demos\\awesome.dem\"");
            help.AddPreOptionsLine("       PurgeDemoCommands.exe \"C:\\path\\to\\demos\"");
            help.AddPreOptionsLine("       PurgeDemoCommands.exe awesome.dem -s _clean");
            help.AddPreOptionsLine(string.Empty);
            help.AddPreOptionsLine("Options:");
            help.AddOptions(this);
            return help;
        }
    }
}