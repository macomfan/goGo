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
            SGF_Tree parser = new SGF_Tree();
            //parser.OpenSGF(@"C:\DEV\SGF\examples\simple0.sgf");
            parser.OpenSGF(@"C:\DEV\SGF\examples\ff4_ex.sgf");
            Read(parser);

            SGF_Node root = parser.GetRoot();
            SGF_Node node = root.Child;
            var ff = node.GetPropertyAs<SGF_Property_FF>();
            if (!ff.IsBlank)
            {
                ff.Value = 3;
            }

            {
                SGF_Property_FF ff1 = new SGF_Property_FF();
                ff1.Value = 4;
            }
            
            parser.SaveSGF(@"C:\DEV\SGF\examples\new.sgf");
        }

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

        private void Read(SGF_Tree parser)
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
                foreach (byte[] s in p.Values)
                {
                    value += Encoding.Default.GetString(s);
                }
                listView1.Items.Add(p.Name + " : " + value);
            }
        }
    }
}
