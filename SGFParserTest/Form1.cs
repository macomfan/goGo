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
        }

        private void ReadNode(TreeNode treenode, SGF_Node node)
        {
            TreeNode newtreenode = treenode.Nodes.Add("Node");
            newtreenode.Tag = node;
            if (node.GetChildren().Count != 0)
            {
                foreach (SGF_Node ch in node.GetChildren())
                {
                    ReadNode(newtreenode, ch);
                }
            }
        }

        private void Read(SGF_Parser parser)
        {
            if (parser == null || parser.GetRoot() == null)
            {
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add("Empty");
            }
            treeView1.Nodes.Add("ROOT");
            ReadNode(treeView1.Nodes[0], parser.GetRoot());
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                return;
            }
            SGF_Node node = e.Node.Tag as SGF_Node;
            listView1.Clear();
            foreach (SGF_Property p in node.GetProperties())
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
