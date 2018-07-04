using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SGFParser;


namespace SGFParserTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
//             SGFBufferParser test = new SGFBufferParser();
//             string value = test.TestProcessPropertyValue("[A][B\\]] \r\n[C]  ");
//             System.Diagnostics.Debug.WriteLine(value);
            SGF_Parser parser = new SGF_Parser();
            //parser.OpenSGF(@"C:\DEV\SGF\examples\simple0.sgf");
            parser.OpenSGF(@"C:\DEV\SGF\examples\ff4_ex.sgf");
            Read(parser);

            SGF_Node root = parser.GetRoot();
            SGF_Node node = root.Child;
            var ff = node.GetProperty<SGF_Property_FF>().Reader;
            //SGF_Nullable<string> ap = node.GetProperty<SGF_Property_AP>().Reader.Value;
        }

//         private void ReadNode(TreeNode treenode, SGF_Node node)
//         {
//             TreeNode newtreenode = treenode.Nodes.Add("Node");
//             newtreenode.Tag = node;
//             if (node.Children.Count != 0)
//             {
//                 foreach (SGF_Node ch in node.Children)
//                 {
//                     ReadNode(newtreenode, ch);
//                 }
//             }
//         }

        private void AddNode(SGF_Node node, TreeNode treenode)
        {
            TreeNode newtreenode = treenode.Nodes.Add("Node");
            newtreenode.Tag = node;
            if (node.Child != null)
            {
                AddNode(node.Child, treenode);
            }
            if (node.StepChildren.Count != 0)
            {
                foreach (SGF_Node stepChild in node.StepChildren)
                {
                    AddBranch(stepChild, newtreenode);
                }
            }
        }

        private void AddBranch(SGF_Node node, TreeNode treenode)
        {
            TreeNode newtreenode = treenode.Nodes.Add("Branch");
            newtreenode.Tag = null;
            AddNode(node, newtreenode);
        }

        private void AddRoot(SGF_Node root)
        {
            treeView1.Nodes.Add("ROOT");
            if (root.StepChildren.Count != 0)
            {
                foreach (SGF_Node stepChild in root.StepChildren)
                {
                    AddBranch(stepChild, treeView1.Nodes[0]);
                }
            }
            if (root.Child != null)
            {
                AddNode(root.Child, treeView1.Nodes[0]);
            }
        }

        private void Read(SGF_Parser parser)
        {
            if (parser == null || parser.GetRoot() == null)
            {
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add("Empty");
                return;
            }
            AddRoot(parser.GetRoot());
        }



        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                listView1.Items.Clear();
                return;
            }
            SGF_Node node = e.Node.Tag as SGF_Node;
            listView1.Clear();
            foreach (SGF_Property p in node.Properties)
            {
                string value = string.Empty;
                foreach (string s in p.Values)
                {
                    value += s;
                }
                listView1.Items.Add(p.Name + " : " + value);
            } 
        }
    }
}
