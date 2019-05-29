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
    public partial class formKullaniciGuncelle : Form
    {
        public formKullaniciGuncelle()
        {
            InitializeComponent();
        }

        public int k_id;

        private void formKullaniciGuncelle_Load(object sender, EventArgs e)
        {
            string sql = " select * from kullanici where k_id='" + k_id + "' ";
            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows == true)
            {
                txtKullaniciAdi.Text = Convert.ToString(rd["k_adi"]);
                txtKullaniciSifresi.Text = Convert.ToString(rd["k_sifre"]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtKullaniciAdi.Text != "" && txtKullaniciSifresi.Text != "" && !varmi())
            {
                string sql = " update kullanici set k_adi='" + txtKullaniciAdi.Text + "' , " +
                " k_sifre='" + txtKullaniciSifresi.Text + "' where k_id=" + k_id;

                SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                baglan.Open();
                SqlCommand cmd = new SqlCommand(sql, baglan);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                baglan.Close();
                this.Close();
            }
            else
                MessageBox.Show("Böyle Bir Kullanıcı Var!");
            
        }

        private bool varmi()
        {
            string sql = " select * from kullanici where k_adi like '" + txtKullaniciAdi.Text + "'";

            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
