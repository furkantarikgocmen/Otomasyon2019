using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Otomasyon2019
{
    public partial class formYeniMusteri : Form
    {
        public formYeniMusteri()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((txtAdi.Text != "") && (txtSoyadi.Text != ""))
            {

                string sql = " insert into musteri values('" + txtAdi.Text + "','" + txtSoyadi.Text + "','" + txtAdres.Text + "','" + txtTelefon.Text + "','" + txtFirmaAdi.Text + "') ";

                SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                baglan.Open();
                SqlCommand cmd = new SqlCommand(sql, baglan);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                baglan.Close();
                MessageBox.Show("Yeni Müşteri Başarıyla Oluşturuldu");
                temizle();
            }
            else
            {
                MessageBox.Show("Eksik Bilgi Girildi");
            }
        }
        public void temizle()
        {
            txtAdi.Text = "";
            txtAdres.Text = "";
            txtFirmaAdi.Text = "";
            txtSoyadi.Text = "";
            txtTelefon.Text = "";
        }
    }
}
