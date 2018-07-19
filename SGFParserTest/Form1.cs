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

            SGF_Node_Root[] roots = parser.Roots;
            SGF_Node_Root root = roots[0];
            var ff = root.GetPropertyAs<SGF_Property_FF>();
            if (!ff.IsBlank)
            {
                ff.Value.Number = 3;
            }
            var SZ = root.GetPropertyAs<SGF_Property_SZ>();
            int szx = SZ.Value.Left.Number;
            int szy = SZ.Value.Right.Number;
            SZ.Value.Right.Number = 20;
            root.SetProperty(SZ);
            {
                SGF_Property_FF ff1 = new SGF_Property_FF();
                ff1.Value.Number = 15;
                root.SetProperty(ff1);
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

        private void AddRoot(SGF_Tree tree)
        {
            treeView1.Nodes.Add("ROOT");
            SGF_Node_Root[] roots = tree.Roots;
            if (roots.Length > 1)
            {
                for (int i = 1; i < roots.Length; i++ )
                {
                    AddBranch(roots[i], treeView1.Nodes[0]);
                }
            }
            if (roots.Length != 0)
            {
                AddNode(roots[0], treeView1.Nodes[0]);
            }
        }

        private void Read(SGF_Tree parser)
        {
            if (parser == null || parser.Roots.Length == 0)
            {
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add("Empty");
                return;
            }
            AddRoot(parser);
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
