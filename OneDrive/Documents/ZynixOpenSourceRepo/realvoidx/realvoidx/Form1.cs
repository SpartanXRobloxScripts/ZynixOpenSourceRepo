using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ZynixAPI;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace realvoidx
{
    public partial class Executor : Form
    {

        private Executor ex;

        bool OfficialInjected = false;
        bool RobloxOpen = false;

        public void Execute(string code)
        {
            if (ZynixAPI.Functions.ExploitLaunched())
            {
                ZynixAPI.Functions.ExecuteLuaCode(code);
            }
            else
            {
                MessageBox.Show("Please inject before executing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Executor()
        {
            InitializeComponent();
        }

        private void cuiStarRating1_Click(object sender, EventArgs e)
        {

        }

        public void SetEditorText(string code)
        {
            Editor.Document.InvokeScript("eval", new object[] { $"editor.setValue({code})" });
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cuiButton5_Click(object sender, EventArgs e)
        {
            if (ZynixAPI.Functions.ExploitLaunched())
            {
                ScriptHub sh = new ScriptHub(this);
                sh.Show();
            }
            else
            {
                MessageBox.Show("Please inject before opening Script Hub.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cuiButton1_Click(object sender, EventArgs e)
        {
            string script = Editor.Document.InvokeScript("getValue").ToString();

            Execute(script);
        }

        private async void Inject()
        {
            if (ZynixAPI.Miscellaneous.RobloxApplicationOpen())
            {
                Title.Text = "Zynix - (initializing)...";
                await Task.Delay(150);
                Title.Text = "Zynix - (checking modules)...";
                await Task.Delay(500);
                Title.Text = "Zynix - (checking config)...";
                await Task.Delay(500);
                Title.Text = "Zynix - (injecting)...";
                OfficialInjected = true;
                RobloxOpen = true;
                await Task.Run(() =>
                {
                    ZynixAPI.Functions.LaunchExploit();
                });
                await Task.Delay(500);
                Title.Text = "Zynix - injected!";
            }
            else
            {
                MessageBox.Show("Please open Roblox before injecting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cuiButton6_Click(object sender, EventArgs e)
        {
            this.TopMost = true;
            Inject();
            await Task.Delay(15000);
            this.TopMost = false;

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(3500);

                    if (RobloxOpen && OfficialInjected && !ZynixAPI.Miscellaneous.RobloxApplicationOpen())
                    {
                        RobloxOpen = false;
                    }
                    else if (!RobloxOpen && OfficialInjected && ZynixAPI.Miscellaneous.RobloxApplicationOpen())
                    {
                        Inject();
                    }
                }
            });
        }

        private void LoadScriptsIntoScriptList()
        {
            string scriptsFolder = Path.Combine(Application.StartupPath, "scripts");

            string[] scripts = Directory.GetFiles(scriptsFolder);

            foreach (string script in scripts)
            {
                MainScriptList.Items.Add(Path.GetFileName(script));
            }
        }

        private async void LoadEditorData()
        {
            string DataFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            string monacoPath = Path.Combine(Directory.GetCurrentDirectory(), "Monaco/index.html");
            Editor.Navigate(new Uri($"file:///{monacoPath}"));

            await Task.Delay(100);

            LoadEditor();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Editor.Visible = true;
            cuiButton1.Visible = true;
            cuiButton2.Visible = true;
            cuiButton3.Visible = true;
            cuiButton4.Visible = true;
            label1.Visible = true;

            MainScriptList.Visible = false;
            WelcomePanel.Visible = false;

            LoadEditorData();
            LoadScriptsIntoScriptList();
        }

        private async void cuiButton8_Click(object sender, EventArgs e)
        {
            SaveCode();
            await Task.Delay(50);
            Application.Exit();
        }

        private void SaveCode()
        {
            string SavedSettings = Path.Combine(Application.StartupPath, "SavedSettings");

            string SavedCodeFile = Path.Combine(SavedSettings, "SavedCode.txt");

            string script = Editor.Document.InvokeScript("getValue").ToString();

            File.WriteAllText(SavedCodeFile, script);
        }

        private void LoadEditor()
        {
            string SavedSettings = Path.Combine(Application.StartupPath, "SavedSettings");

            string SavedCodeFile = Path.Combine(SavedSettings, "SavedCode.txt");
            string SavedCode = File.ReadAllText(SavedCodeFile);

            string safeCode = JsonConvert.SerializeObject(SavedCode);

            Editor.Document.InvokeScript("eval", new object[] { $"editor.setValue({safeCode})" });
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveCode();
            await Task.Delay(50);
            Application.Exit();
        }

        private void cuiButton2_Click(object sender, EventArgs e)
        {
            Editor.Document.InvokeScript("eval", new object[] { "editor.setValue('')" });
        }

        private void cuiButton4_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sf = new SaveFileDialog())
            {
                sf.Title = "Save your code file.";
                sf.Filter = "Lua files (*.lua)|*.lua|All files (*.*)|*.*";

                if (sf.ShowDialog() == DialogResult.OK)
                {
                    string FileSaved = sf.FileName;

                    string script = Editor.Document.InvokeScript("getValue").ToString();

                    File.WriteAllText(FileSaved, script);
                }
            }
        }

        private void cuiButton3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog of = new OpenFileDialog())
            {
                of.Title = "Load a file into your script editor.";
                of.Filter = "Lua files (*.lua)|*.lua|All files (*.*)|*.*";

                if (of.ShowDialog() == DialogResult.OK)
                {
                    string FileSaved = of.FileName;
                    string FileCode = File.ReadAllText(FileSaved);

                    string rawScript = JsonConvert.SerializeObject(FileCode);

                    Editor.Document.InvokeScript("eval", new object[] { $"editor.setValue({rawScript})" });
                }
            }
        }

        private async void Maximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SaveCode();
            await Task.Delay(1500);
            LoadEditorData();
        }

        private void Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void cuiButton9_Click(object sender, EventArgs e)
        {
            Editor.Visible = false;
            cuiButton1.Visible = false;
            cuiButton2.Visible = false;
            cuiButton3.Visible = false;
            cuiButton4.Visible = false;
            label1.Visible = false;
            MainScriptList.Visible = false;

            WelcomePanel.Visible = true;
        }

        private void cuiButton7_Click(object sender, EventArgs e)
        {
            Editor.Visible = true;
            cuiButton1.Visible = true;
            cuiButton2.Visible = true;
            cuiButton3.Visible = true;
            cuiButton4.Visible = true;
            label1.Visible = true;

            MainScriptList.Visible = false;
            WelcomePanel.Visible = false;
        }

        private void cuiButton10_Click(object sender, EventArgs e)
        {
            Editor.Visible = false;
            cuiButton1.Visible = false;
            cuiButton2.Visible = false;
            cuiButton3.Visible = false;
            cuiButton4.Visible = false;
            label1.Visible = false;
            WelcomePanel.Visible = false;

            MainScriptList.Visible = true;
        }

        private void MainScriptList_DoubleClick(object sender, EventArgs e)
        {
            string scriptsFolder = Path.Combine(Application.StartupPath, "scripts");

            string selectedScript = MainScriptList.SelectedItem.ToString();
            string selectedScriptPath = Path.Combine(scriptsFolder, selectedScript);

            string selectedScriptCode = File.ReadAllText(selectedScriptPath);
            string safeCode = JsonConvert.SerializeObject(selectedScriptCode);

            SetEditorText(safeCode);
        }

        private void loadToEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scriptsFolder = Path.Combine(Application.StartupPath, "scripts");

            string selectedScript = MainScriptList.SelectedItem.ToString();
            string selectedScriptPath = Path.Combine(scriptsFolder, selectedScript);

            string selectedScriptCode = File.ReadAllText(selectedScriptPath);
            string safeCode = JsonConvert.SerializeObject(selectedScriptCode);

            SetEditorText(safeCode);
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scriptsFolder = Path.Combine(Application.StartupPath, "scripts");

            string selectedScript = MainScriptList.SelectedItem.ToString();
            string selectedScriptPath = Path.Combine(scriptsFolder, selectedScript);

            string selectedScriptCode = File.ReadAllText(selectedScriptPath);

            Execute(selectedScriptCode);
        }
    }
}
