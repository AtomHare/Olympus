﻿using MonoMod.Utils;
using System;
using System.Diagnostics;
using System.IO;

namespace Olympus {
    public class CmdLaunchLoenn : Cmd<string, string> {

        public override bool Taskable => true;

        public override string Run(string root) {
            Process loenn = new Process();
            loenn.StartInfo.UseShellExecute = true;

            if (PlatformHelper.Is(Platform.Windows)) {
                loenn.StartInfo.FileName = Path.Combine(root, "Lönn.exe");
                loenn.StartInfo.WorkingDirectory = root;
            } else if (PlatformHelper.Is(Platform.Linux)) {
                // use the find-love script that olympus also uses
                loenn.StartInfo.FileName = Path.Combine(Program.RootDirectory, "find-love");
                loenn.StartInfo.Arguments = Path.Combine(root, "Lönn.love");
                loenn.StartInfo.WorkingDirectory = Program.RootDirectory;
            } else {
                // run the app
                loenn.StartInfo.FileName = "open";
                loenn.StartInfo.Arguments = "Lönn.app";
                loenn.StartInfo.WorkingDirectory = root;
            }


            Console.Error.WriteLine($"Starting Loenn process: {loenn.StartInfo.FileName} {loenn.StartInfo.Arguments} (in {root})");

            loenn.HandleLaunchWrapper("LOENN");
            loenn.Start();
            return null;
        }

    }
}
