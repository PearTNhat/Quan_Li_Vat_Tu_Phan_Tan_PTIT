using QuanLiVatTu_PhanTan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quản_lí_vật_tư
{
    public partial class frmLogin : Form
    {
        public static SqlConnection conn_publisher = new SqlConnection();
        
        public frmLogin()
        {
            InitializeComponent();
        }

        public bool ConnectToMainServer()
        {
            if (conn_publisher != null && conn_publisher.State == ConnectionState.Open) conn_publisher.Close();
            try
            {
                conn_publisher.ConnectionString = Program.connstr_publisher;
                conn_publisher.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not connect to the main server",ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void LayDSPM(String cmd)
        {
            DataTable dt = new DataTable();
            if (conn_publisher.State == ConnectionState.Closed) Program.conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn_publisher);
            try
            {
                da.Fill(dt);
                conn_publisher.Close();
                Program.bds_dspm.DataSource = dt;
                cmbChiNhanh.DataSource = Program.bds_dspm; // nhận 1 table
                cmbChiNhanh.DisplayMember = "TENCN"; // cột hiện thị
                cmbChiNhanh.ValueMember = "TENSERVER"; // Giá trị trả về
              

            }
            catch (SqlException ex)
            {
                conn_publisher.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát không ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (txtPassword.Text.Trim() == "" || txtUsername.Text.Trim() == "")
            {
                MessageBox.Show("Error", "Username or password cann't be empty");
                return;
            }
            Program.mLogin = txtUsername.Text;
            Program.mPassword = txtPassword.Text;
            Console.WriteLine(Program.servername);
            if (!Program.Connect()) return;
            Program.mChinhanh = cmbChiNhanh.SelectedIndex;
            string queryStr = "exec [SP_LayThongTinNhanVien] '" + Program.mLogin + "'";

            Program.myReader = Program.ExecSqlDataReader(queryStr);

            if (Program.myReader == null) return;
            Program.myReader.Read();

            Program.username = Program.myReader.GetString(0);
            if (Convert.IsDBNull(Program.username))
            {
                MessageBox.Show("Bạn không có quyền truy cập dữ liệu, xem lại phân quyền");
                return;
            }
            Program.mHoten = Program.myReader.GetString(1);
            Program.mGroup = Program.myReader.GetString(2);
            Program.myReader.Close();
            Program.conn.Close();
            this.Hide();
            frmMain fMain = new frmMain();
            fMain.ShowDialog();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (!ConnectToMainServer()) return;
            LayDSPM("select * from [dbo].[Get_Subscribes]");
            cmbChiNhanh.SelectedIndex = 1;
            cmbChiNhanh.SelectedIndex = 0;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

       
        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {           
            Program.servername = cmbChiNhanh.SelectedValue.ToString();        }
        }
}
