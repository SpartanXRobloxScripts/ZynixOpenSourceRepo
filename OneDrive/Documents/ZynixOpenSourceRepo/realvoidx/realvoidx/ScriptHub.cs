using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using ZynixAPI;

namespace realvoidx
{
    public partial class ScriptHub : Form
    {

        private Executor ex;

        private void LoadScriptHubScripts()
        {
            string SavedSettings = Path.Combine(Application.StartupPath, "SavedSettings");

            string ScriptHubFolder = Path.Combine(SavedSettings, "Scripts");

            string[] ScriptHubScripts = Directory.GetDirectories(ScriptHubFolder);

            foreach (string script in ScriptHubScripts)
            {
                string ScriptNameFile = Path.Combine(script, "Name.txt");
                string ScriptName = File.ReadAllText(ScriptNameFile);

                ScriptList.Items.Add(ScriptName);
            }
        }

        public ScriptHub(Executor x)
        {
            InitializeComponent();
            ex = x;
        }

        private void cuiButton8_Click(object sender, EventArgs e)
        {
            this.Hide();
            //Application.Exit();
        }

        private void ScriptHub_Load(object sender, EventArgs e)
        {
            LoadScriptHubScripts();
        }

        private void ScriptList_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ScriptList.SelectedItems != null)
            {
                string ScriptsFolder = Path.Combine(Application.StartupPath, "SavedSettings", "Scripts");

                string[] Scripts = Directory.GetDirectories(ScriptsFolder);
                foreach (string script in Scripts)
                {
                    string ScriptNameFile = Path.Combine(script, "Name.txt");
                    string ScriptName = File.ReadAllText(ScriptNameFile);

                    string SelectedScript = ScriptList.SelectedItem.ToString();

                    if (ScriptName == SelectedScript)
                    {
                        string CodeFile = Path.Combine(script, "Code.txt");
                        string Code = File.ReadAllText(CodeFile);

                        ZynixAPI.Functions.ExecuteLuaCode(Code);
                    }
                }
            }
        }

        private void loadIntoScriptEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ScriptList.SelectedItem != null)
            {
                string ScriptsFolder = Path.Combine(Application.StartupPath, "SavedSettings", "Scripts");

                string[] Scripts = Directory.GetDirectories(ScriptsFolder);
                foreach (string script in Scripts)
                {
                    string ScriptNameFile = Path.Combine(script, "Name.txt");
                    string ScriptName = File.ReadAllText(ScriptNameFile);

                    string SelectedScript = ScriptList.SelectedItem.ToString();

                    if (ScriptName == SelectedScript)
                    {
                        string CodeFile = Path.Combine(script, "Code.txt");

                        if (File.Exists(CodeFile))
                        {
                            string Code = File.ReadAllText(CodeFile);
                            string SafeCode = JsonConvert.SerializeObject(Code);

                            ex.SetEditorText(SafeCode);
                        }
                        else
                        {
                            MessageBox.Show("Could not find Code.txt");
                        }
                    }
                }
            }
        }

        private void ScriptList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ScriptList.SelectedItem != null)
            {
                string ScriptsFolder = Path.Combine(Application.StartupPath, "SavedSettings", "Scripts");

                string[] Scripts = Directory.GetDirectories(ScriptsFolder);
                foreach (string script in Scripts)
                {
                    string ScriptNameFile = Path.Combine(script, "Name.txt");
                    string ScriptName = File.ReadAllText(ScriptNameFile);

                    string SelectedScript = ScriptList.SelectedItem.ToString();

                    if (ScriptName == SelectedScript)
                    {
                        string AboutFile = Path.Combine(script, "About.txt");
                        string ScriptImage = Path.Combine(script, "ScriptImage.png");

                        if (File.Exists(AboutFile))
                        {
                            string AboutScript = File.ReadAllText(AboutFile);

                            AboutLabel.Text = AboutScript;
                        }
                        else
                        {
                            MessageBox.Show("Could not find About.txt");
                        }

                        if (File.Exists(ScriptImage))
                        {
                            using (var imgStream = new FileStream(ScriptImage, FileMode.Open, FileAccess.Read))
                            {
                                System.Drawing.Image img = System.Drawing.Image.FromStream(imgStream);
                                ScriptPicture.BackgroundImage = new Bitmap(img); // clone it to release the file
                            }
                        }
                    }
                }
            }
        }
    }
}
