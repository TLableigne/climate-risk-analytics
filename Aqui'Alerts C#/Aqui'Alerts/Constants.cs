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
        public static string pythonScriptPath = Path.GetFullPath(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\pythonScripts\")
        );
        public static string pythonScriptGetQueries => Path.Combine(pythonScriptPath, "get_queries.py");
        public static string scriptGraphicName => Path.Combine(pythonScriptPath, "choosenGraph.py");
        public static string scriptImportName => Path.Combine(pythonScriptPath, "temp.py");
        public static string scriptDataSetCollectionName => Path.Combine(pythonScriptPath, "dataSetCollection.py");
        public static string logoImgPath => Path.Combine(pythonScriptPath, @"img\Logo.png");
        public static string graphImgPath => Path.Combine(pythonScriptPath, @"img\Graph.png");
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

        public static string CmdActionWithOutput(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                RedirectStandardOutput = true, 
                RedirectStandardError = true,
                UseShellExecute = false,       
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                return output;
            }
        }
    }
}
