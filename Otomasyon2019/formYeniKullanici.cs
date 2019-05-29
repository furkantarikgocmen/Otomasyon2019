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
    public partial class formYeniKullanici : Form
    {
        public formYeniKullanici()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((txtKullaniciAdi.Text != "") && (txtKullaniciSifresi.Text != ""))
            {
                if (varmi())
                {
                    MessageBox.Show("Böyle Bir Kullanıcı Var");
                }
                else
                {
                    string sql = " insert into kullanici values('" + txtKullaniciAdi.Text + "','" + txtKullaniciSifresi.Text + "') ";

                    SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                    baglan.Open();
                    SqlCommand cmd = new SqlCommand(sql, baglan);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    baglan.Close();
                    MessageBox.Show("Yeni Kullanıcı Başarıyla Oluşturuldu");
                    temizle();
                }
            }
            else
            {
                MessageBox.Show("Eksik Bilgi Girdiniz");
            }
        }

        public bool varmi()
        {

            bool durum;
            string sql = " select * from kullanici where k_adi='" + txtKullaniciAdi.Text + "' ";

           SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
           sqlconn.Open();
           SqlCommand cmd = new SqlCommand(sql, sqlconn);
           cmd.CommandType = CommandType.Text;
           SqlDataReader rd = cmd.ExecuteReader();
           rd.Read();
           if (rd.HasRows == true)
           {
               durum = true;
           }
           else
               durum = false;
           return durum;
           
        }

        public void temizle()
        {
            txtKullaniciAdi.Text = "";
            txtKullaniciSifresi.Text = "";
        }
    }
}
