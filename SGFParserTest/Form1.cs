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
        }

        private void ReadTree(SGF_Node node)
        {

        }

        private void ReadNode(SGF_Node node)
        {

        }

        private void Read(SGF_Parser parser)
        {
            if (parser == null || parser.GetRoot() == null)
            {
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add("Empty");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            SGF_Parser parser = new SGF_Parser();
            parser.OpenSGF(@"C:\DEV\SGF\examples\simple0.sgf");

            treeView1.Nodes.Add("A");
            treeView1.Nodes[0].Nodes.Add("B");
        }
    }
}
