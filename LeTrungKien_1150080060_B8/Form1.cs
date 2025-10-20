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

namespace LeTrungKien_1150080060_B8
{
    public partial class Form1 : Form
    {
        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;
                        AttachDbFilename=D:\OOP\LeTrungKien_1150080060_B8\LeTrungKien_1150080060_B8\DbConnect.mdf;
                        Integrated Security=True";
        SqlConnection sqlCon = null;

        public Form1()
        {
            InitializeComponent();
        }
        private void MoKetNoi()
        {
            if (sqlCon == null)
                sqlCon = new SqlConnection(strCon);
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
        }

        private void DongKetNoi()
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open)
                sqlCon.Close();
        }
        private void HienThiDanhSachXB()
        {
            MoKetNoi();
            SqlCommand sqlCmd = new SqlCommand("HienThiXB", sqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = sqlCmd.ExecuteReader();
            lsvDanhSach.Items.Clear();

            while (reader.Read())
            {
                string maXB = reader.GetString(0);
                string tenXB = reader.GetString(1);
                string diaChi = reader.GetString(2);

                ListViewItem lvi = new ListViewItem(maXB);
                lvi.SubItems.Add(tenXB);
                lvi.SubItems.Add(diaChi);
                lsvDanhSach.Items.Add(lvi);
            }
            reader.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetupListView();
            HienThiDanhSachXB();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void SetupListView()
        {
            lsvDanhSach.View = View.Details;
            lsvDanhSach.FullRowSelect = true;
            lsvDanhSach.GridLines = true;

            if (lsvDanhSach.Columns.Count == 0)
            {
                lsvDanhSach.Columns.Add("Mã XB", 100);
                lsvDanhSach.Columns.Add("Tên XB", 180);
                lsvDanhSach.Columns.Add("Địa chỉ", 260);
            }
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            MoKetNoi();
            SqlCommand sqlCmd = new SqlCommand("ThemDuLieu", sqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.AddWithValue("@MaXB", txtMaXB.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@TenXB", txtTenXB.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());

            int kq = sqlCmd.ExecuteNonQuery();
            if (kq > 0)
            {
                MessageBox.Show("Thêm dữ liệu thành công!");
                HienThiDanhSachXB();
                txtMaXB.Clear(); txtTenXB.Clear(); txtDiaChi.Clear();
            }
        }
        private void FillInputsFromSelectedItem()
        {
            if (lsvDanhSach.SelectedItems.Count == 0) return;
            var item = lsvDanhSach.SelectedItems[0];

            txtMaXB.Text = item.SubItems[0].Text;
            txtTenXB.Text = item.SubItems[1].Text;
            txtDiaChi.Text = item.SubItems[2].Text;

            btnSua.Enabled = btnXoa.Enabled = true;
        }


        private void lsvDanhSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvDanhSach.SelectedItems.Count == 0)
            {
                btnSua.Enabled = btnXoa.Enabled = false;
                return;
            }
            FillInputsFromSelectedItem();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                MoKetNoi();
                SqlCommand sqlCmd = new SqlCommand("SuaDuLieu", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@MaXB", txtMaXB.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@TenXB", txtTenXB.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());

                int kq = sqlCmd.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    HienThiDanhSachXB();
                }
            }
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            MoKetNoi();
            SqlCommand sqlCmd = new SqlCommand("XoaDuLieu", sqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@MaXB", txtMaXB.Text.Trim());

            int kq = sqlCmd.ExecuteNonQuery();
            if (kq > 0)
            {
                MessageBox.Show("Xóa thành công!");
                HienThiDanhSachXB();
            }
        }
    }
}
