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
    public partial class formKullaniciListesi : Form
    {
        public formKullaniciListesi()
        {
            InitializeComponent();
        }

        public void fnk_liste_doldur()
        {
            string sql = " select k_id as 'KULLANICI NO' , k_adi as 'KULLANICI ADI', " +
                            " k_sifre as 'KULLANICI ŞİFRE' from kullanici order by k_id desc ";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
        }

        


        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int kullaniciid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["KULLANICI NO"].Value);

            string sql = " delete from kullanici where k_id=" + kullaniciid;

            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
            baglan.Open();
            SqlCommand cmd = new SqlCommand(sql, baglan);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            baglan.Close();
            fnk_liste_doldur();
            MessageBox.Show("Silme İşlemi Tamamlandı");
            kapat();
        }

        private void güncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int kullaniciid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["KULLANICI NO"].Value);

            formKullaniciGuncelle a = new formKullaniciGuncelle();
            a.k_id = kullaniciid;
            a.ShowDialog();
            kapat();
        }

       

        private void formKullaniciListesi_Load(object sender, EventArgs e)
        {
            fnk_liste_doldur();
            kapat();
        }

        private void formKullaniciListesi_Activated(object sender, EventArgs e)
        {
            fnk_liste_doldur();
        }

        private void txtKullaniciAdi_TextChanged(object sender, EventArgs e)
        {
            string sql = " select k_id as 'KULLANICI NO' , k_adi as 'KULLANICI ADI', " +
                            " k_sifre as 'KULLANICI ŞİFRE' from kullanici " +
            " where k_adi like '%" + txtKullaniciAdi.Text + "%' order by k_id desc ";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
            kapat();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["KULLANICI NO"].Value != null)
            {
                int kullaniciid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["KULLANICI NO"].Value);

                formKullaniciGuncelle a = new formKullaniciGuncelle();
                a.k_id = kullaniciid;
                a.ShowDialog();
            }
            else
                MessageBox.Show("Lütfen Bir Kullanıcı Seçiniz");
            kapat();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["KULLANICI NO"].Value != null)
            {
                int kullaniciid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["KULLANICI NO"].Value);

                string sql = " delete from kullanici where k_id=" + kullaniciid;

                SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                baglan.Open();
                SqlCommand cmd = new SqlCommand(sql, baglan);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                baglan.Close();
                fnk_liste_doldur();
                MessageBox.Show("Silme İşlemi Tamamlandı");
            }
            else
                MessageBox.Show("Lütfen Bir Kullanıcı Seçiniz");
            kapat();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            ac();
        }
        
        private void kapat()
        {
            btnSil.Enabled = false;
            btnGuncelle.Enabled = false;
        }
        
        private void ac()
        {
            btnSil.Enabled = true;
            btnGuncelle.Enabled = true;
        }

        private void txtKullaniciAdi_Click(object sender, EventArgs e)
        {
            kapat();
        }
    }
}
