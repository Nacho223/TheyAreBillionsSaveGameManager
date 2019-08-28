using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Savegame_Manager
{
    public partial class Form1 : Form
    {
        private static readonly string path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents\\My Games\\");
        private string selectedNode = string.Empty;

        public Form1()
        {
            InitializeComponent();
            PopulateTreeView();
           // PopulateTreeViewTo();
        }


        private void PopulateTreeView()
        {
            TreeNode rootNode;
            FromList.Nodes.Clear();
            DirectoryInfo info = new DirectoryInfo(Path.Combine(path, "Fuck Numantian Games\\Saves\\"));
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                FromList.Nodes.Add(rootNode);
            }
            FromList.ExpandAll();
        }

        private void PopulateTreeViewTo()
        {
            TreeNode rootNode;

            DirectoryInfo info = new DirectoryInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents\\"));
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                ToList.Nodes.Add(rootNode);
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            try
            {
                TreeNode aNode;
                DirectoryInfo[] subSubDirs;
                foreach (DirectoryInfo subDir in subDirs)
                {
                    aNode = new TreeNode(subDir.Name, 0, 0);
                    aNode.Tag = subDir;
                    aNode.ImageKey = "folder";
                    subSubDirs = subDir.GetDirectories();
                    if (subSubDirs.Length != 0)
                    {
                        GetDirectories(subSubDirs, aNode);
                    }
                    nodeToAddTo.Nodes.Add(aNode);
                }
            }
            catch (Exception) { }
        }


        private void FromList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedNode = e.Node.FullPath;
            Restore.Enabled = true;
            Delete.Enabled = true;
        }


        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedNode != string.Empty)
            {
                foreach (string file in Directory.GetFiles(Path.Combine(path, "Fuck Numantian Games\\" + selectedNode + "\\")))
                {
                    string[] division = file.Split('\\');
                    string localFile = division[division.Length - 1];
                    File.Copy(file, Path.Combine(path, "They Are Billions\\Saves\\" + localFile), true);
                }
                PopulateTreeView();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedNode != string.Empty)
            {
                foreach (string file in Directory.GetFiles(Path.GetFullPath(selectedNode)))
                {
                    File.Delete(file);
                }
                PopulateTreeView();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopulateTreeView();
        }
    }
}
