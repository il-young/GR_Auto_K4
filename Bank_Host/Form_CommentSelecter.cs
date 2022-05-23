using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Host
{
    public partial class Form_CommentSelecter : Form
    {
        public delegate void SelectedComment(string msg);
        public event SelectedComment SelectedComment_event;

        public delegate void UnSelect();
        public event UnSelect UnSelect_event;

        List<string> lComment = new List<string>();

        public Form_CommentSelecter()
        {
            InitializeComponent();
        }

        public Form_CommentSelecter(List<string> com)
        {
            lComment = com;
            InitializeComponent();
        }

        private void Form_CommentSelecter_Load(object sender, EventArgs e)
        {
            cb_comment.Items.Clear();

            for(int i = 0; i< lComment.Count; i++)
            {
                cb_comment.Items.Add(i + 1);
            }

            cb_comment.Text = "1";
            tb_comment.Text = lComment[0];

            cb_comment.Focus();
        }

        private void cb_comment_SelectedIndexChanged(object sender, EventArgs e)
        {
            tb_comment.Text = lComment[int.Parse(cb_comment.Text) - 1];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UnSelect_event();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedComment_event(lComment[int.Parse(cb_comment.Text) - 1]);
            Close();
        }

        private void cb_comment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                button1_Click(sender, e);
        }
    }
}
