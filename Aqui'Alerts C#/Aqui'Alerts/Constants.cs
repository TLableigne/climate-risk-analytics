using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqui_Alerts
{
    public static class Constants
    {
        public static readonly Color colorBackground = Color.FromArgb(255, 120, 2, 30);
        public static string pythonScriptPath = @"\\iut.bx\Etudiants\Home\lleguennec\Documents\Projet\s2.04-05\pythonScripts";
        public static string scriptName = "choosenGraph.py";
        public static Process GenerateCmd()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.WorkingDirectory = pythonScriptPath;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            return cmd;
        }

        public static void CmdAction(string line, Process cmd)
        {
            cmd.StandardInput.WriteLine(line);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string erreursPython = cmd.StandardError.ReadToEnd();
            cmd.WaitForExit();

            if (!string.IsNullOrEmpty(erreursPython) && erreursPython.Contains("Traceback"))
            {
                MessageBox.Show("Le script Python a rencontré une erreur :\n\n" + erreursPython, "Erreur Python", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
