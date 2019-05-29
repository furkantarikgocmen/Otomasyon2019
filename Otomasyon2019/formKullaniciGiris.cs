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
    public partial class formKullaniciGiris : Form
    {
        public formKullaniciGiris()
        {
            InitializeComponent();
        }
        int kullaniciid;
        public static string baglanti = "Data Source=FTG;Initial Catalog=otomasyon_2019;Integrated Security=True";

        private void button1_Click(object sender, EventArgs e)
        {
            if ((txtKullaniciAdi.Text != "") && (txtSifre.Text != ""))
            {
                string sql = " select * from kullanici where k_adi='" + txtKullaniciAdi.Text + "' and  " +
                    " k_sifre='" + txtSifre.Text + "'";

                SqlConnection sqlconn = new SqlConnection(baglanti);
                sqlconn.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlconn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                if (rd.HasRows == true)
                {
                    kullaniciid = Convert.ToInt32(rd["k_id"]);
                    this.Hide();
                    anaForm an = new anaForm();
                    an.kullaniciid = kullaniciid;
                    an.ShowDialog();
                }
                else
                {
                    MessageBox.Show(" Yanlış Bilgi Girildi ! ");
                    txtKullaniciAdi.Text = "";
                    txtSifre.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Eksik Bilgi Girildi");
            }
        }

        private void formKullaniciGiris_Load(object sender, EventArgs e)
        {
            txtSifre.PasswordChar = '\u25CF';
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
