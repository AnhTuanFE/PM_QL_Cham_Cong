using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace QuanLyNhanSu.CT
{
    public partial class ctChamCong : UserControl
    {
        public ctChamCong()
        {
            InitializeComponent();
        }
        CauLenh cl = new CauLenh();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        int dem = 0;
        string manv = null, a = null, b = null;
        private void load()
        {
            label2.Text = null;
            lbMa.Text = null;
            lbTen.Text = null;
            lbTinhTrang.Text = null;
            dr = cl.LayChamCong("1",dtpNgay.Value);
            if (dr != null)
            {
                while (dr.Read())
                {
                    DateTime kq = dr.GetDateTime(0);
                    a = kq.ToString("yyyy/MM/dd");
                }
            }
            b = dtpNgay.Value.ToString("yyyy/MM/dd");
            
            if (a == b)
            {
                MessageBox.Show("Đã chấm công cho ngày hôm nay", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                dt.Clear();
                dataGridView1.Refresh();
                dt = cl.LayChamCong1("0",DateTime.Now);
                dataGridView1.DataSource = dt;
                dem = 1;
            }
            else
            {
                dt.Clear();
                dataGridView1.Refresh();
                dt = cl.LayTenNhanVien1("0");
                dataGridView1.DataSource = dt;
                dem = 0;
            }
            
        }
        private void ctChamCong_Load(object sender, EventArgs e)
        {
            load();
        }
        private void btCapNhat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                string tinhtrang = "", manv = "";
                if (dataGridView1.Rows[i].Cells["TinhTrang"].Value.ToString()== "")
                {
                    tinhtrang = "Nghỉ Không Phép";
                }
                else
                {
                    tinhtrang = dataGridView1.Rows[i].Cells["TinhTrang"].Value.ToString();
                }
                 manv = dataGridView1.Rows[i].Cells["Ma"].Value.ToString();
                dr = cl.ThemChamCong(manv, dtpNgay.Value, tinhtrang);
            }
            load();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0 && dem == 1)
            {
                lbMa.Text = dataGridView1.CurrentRow.Cells["Ma"].Value.ToString();
                lbTen.Text = dataGridView1.CurrentRow.Cells["Ten"].Value.ToString();
                lbTinhTrang.Text = dataGridView1.CurrentRow.Cells["TinhTrang"].Value.ToString();
            }
            else if(e.ColumnIndex != 0 && dem == 0)
            {
                lbMa.Text = dataGridView1.CurrentRow.Cells["Ma"].Value.ToString();
                lbTen.Text = dataGridView1.CurrentRow.Cells["Ten"].Value.ToString();
                lbTinhTrang.Text = "Chưa có giá trị";
            }
        }

        private void dtpNgay_ValueChanged(object sender, EventArgs e)
        {
            load();
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn thật sự muốn xóa?", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    dr = cl.XoaChamCongTheoNgay(dtpNgay.Value);
                    a = null;
                    b = null;
                    load();
                }
            }
            catch (Exception)
            {
                label2.Text = "Lỗi!!";
            }
           
        }

        private void cbCC_CheckedChanged(object sender, EventArgs e)
        {
            if(cbCC.Checked == true)
                for (int i = 0; i < dataGridView1.RowCount; i++)
                    dataGridView1.Rows[i].Cells["TinhTrang"].Value = "Đi Làm";
            else
                for (int i = 0; i < dataGridView1.RowCount; i++)
                    dataGridView1.Rows[i].Cells["TinhTrang"].Value = "";
        }

    }
}
