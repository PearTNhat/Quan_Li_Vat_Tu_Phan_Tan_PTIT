using QuanLiVatTu_PhanTan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quản_lí_vật_tư
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            stsID.Text = "Mã NV = " + Program.username;
            stsName.Text = "Họ Tên = " + Program.mLogin;
            stsGroup.Text = "Nhóm = " + Program.mGroup;
        }

        private void stsID_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
    }
}
