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
    public partial class anaForm : Form
    {
        public static string baglanti = "Data Source=FTG;Initial Catalog=otomasyon_2019;Integrated Security=True";
        public int kullaniciid;
        

        public anaForm()
        {
            InitializeComponent();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void anaForm_Load(object sender, EventArgs e)
        {
            yetkili();
            tarih.Text = DateTime.Now.ToLongDateString();

        }

        private void kullanıcıEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formYeniKullanici yeniKullanici = new formYeniKullanici();
            yeniKullanici.ShowDialog();
        }

        private void kullanıcıListeleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formKullaniciListesi k = new formKullaniciListesi();
            k.ShowDialog();
        }

        private void müşteriEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formYeniMusteri m = new formYeniMusteri();
            m.ShowDialog();
        }

        private void müşteriListeleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMusteriListesi ml = new formMusteriListesi();
            ml.ShowDialog();
        }

        private void kategoriİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formKategoriListesi kl = new formKategoriListesi();
            kl.ShowDialog();
        }

        private void ürünİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formUrunListesi ul = new formUrunListesi();
            ul.ShowDialog();
        }

        private void satışİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formSatis s = new formSatis();
            s.ShowDialog();
        }

        public void yetkili()
        {
            string sql = "select k_adi from kullanici where k_id = '" + kullaniciid + "' ";
            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                label13.Text = Convert.ToString(rd[0]);
            }
            else
            {
                MessageBox.Show("Yetkisiz Erişim: Güvenlik ihlal edildi. Uygulama Kapatılıyor");
                Application.Exit();
            }
                
            
            sqlconn.Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
            
            if (label1.Visible == true)
            {
                label1.Visible = false;
            }
            else
            {
                label1.Visible = true;
            }
            sayac();
        }
        
        private void Timer3_Tick(object sender, EventArgs e)
        {
            timer3.Interval = 60000;
            timer3.Start();
            tarih2.Text = DateTime.Now.ToShortTimeString();
        }

        public void sayac()
        {
            
            if (MusteriText.Text != "")
            {
                string sql2 = "select count(*) from musteri";
                SqlConnection sqlconn2 = new SqlConnection(baglanti);
                sqlconn2.Open();
                SqlCommand cmd2 = new SqlCommand(sql2, sqlconn2);
                cmd2.CommandType = CommandType.Text;
                SqlDataReader rd2 = cmd2.ExecuteReader();
                rd2.Read();
                MusteriText.Text = Convert.ToString(rd2[0]);
                sqlconn2.Close();
            }
            if (KategoriText.Text != "")
            {
                string sql = "select count(*) from urun_kategori";
                SqlConnection sqlconn = new SqlConnection(baglanti);
                sqlconn.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlconn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                KategoriText.Text = Convert.ToString(rd[0]);
                sqlconn.Close();
            }
            if (UrunText.Text != "")
            {
                string sql = "select count(*) from urun";
                SqlConnection sqlconn = new SqlConnection(baglanti);
                sqlconn.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlconn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                UrunText.Text = Convert.ToString(rd[0]);
                sqlconn.Close();
            }
            if (SatisText.Text != "")
            {
                string sql = "select count(*) from satis";
                SqlConnection sqlconn = new SqlConnection(baglanti);
                sqlconn.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlconn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                SatisText.Text = Convert.ToString(rd[0]);
                sqlconn.Close();
            }
            if(kasaText.Text != "")
            {
                string sql = "select sum(urun.u_fiyat*satis.s_adet) from satis left join urun on urun.u_id=satis.s_urun_id";
                SqlConnection sqlconn = new SqlConnection(baglanti);
                sqlconn.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlconn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                kasaText.Text = Convert.ToString(rd[0] + " TL");
                sqlconn.Close();
            }
            if(stokText.Text != "")
            {
                string sql = "select sum(u_stok) from urun";
                SqlConnection sqlconn = new SqlConnection(baglanti);
                sqlconn.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlconn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                stokText.Text = Convert.ToString(rd[0]+" Adet");
                sqlconn.Close();
            }
        }


        private void musteriDT_Click(object sender, EventArgs e)
        {
            formMusteriListesi fml = new formMusteriListesi();
            fml.ShowDialog();
        }

        private void kategoriDT_Click(object sender, EventArgs e)
        {
            formKategoriListesi fkl = new formKategoriListesi();
            fkl.ShowDialog();
        }

        private void urunDT_Click(object sender, EventArgs e)
        {
            formUrunListesi ful = new formUrunListesi();
            ful.ShowDialog();
        }

        private void satisDT_Click(object sender, EventArgs e)
        {
            formSatis fs = new formSatis();
            fs.ShowDialog();
        }
    }
}
