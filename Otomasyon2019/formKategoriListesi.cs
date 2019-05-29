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
    public partial class formKategoriListesi : Form
    {
        public formKategoriListesi()
        {
            InitializeComponent();
        }
        bool guncelleDurum = false;
        int guncelleId = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtAdi.Text != "")
            {
                if (varmi() == false)
                {
                    if (guncelleDurum == false)
                    {
                        string sql = " insert into urun_kategori values('" + txtAdi.Text + "') ";

                        SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                        baglan.Open();
                        SqlCommand cmd = new SqlCommand(sql, baglan);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        baglan.Close();
                        MessageBox.Show("Yeni Kategori Başarıyla Oluşturuldu");
                        txtAdi.Text = "";
                    }
                    else
                    {
                        string sql = " update urun_kategori set uk_adi='" + txtAdi.Text + "' where uk_id='" + guncelleId + "'";
                        SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                        baglan.Open();
                        SqlCommand cmd = new SqlCommand(sql, baglan);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        baglan.Close();
                        //this.Close();
                        MessageBox.Show("Kategori Adı Güncellendi");
                        doldur();
                        guncelleDurum = false;
                        guncelleId = 0;
                        button1.Text = "KAYDET";
                        txtAdi.Text = "";
                    }
                }
                else
                    MessageBox.Show("Böyle Bir Kategori Var!");
            }
            else
                MessageBox.Show("Lütfen Bir Kategori Adı Giriniz");
            kapat();
        }

        private void txtKullaniciAdi_TextChanged(object sender, EventArgs e)
        {
            string sql = " select uk_id as 'KATEGORİ NO' , uk_adi as 'KATEGORİ ADI' from urun_kategori " +
            " where uk_adi like '%" + txtAdi.Text + "%' order by uk_id desc";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
            kapat();
        }

        private void formKategoriListesi_Load(object sender, EventArgs e)
        {
            doldur();
            kapat();
        }


        public void doldur()
        {
            string sql = " select uk_id as 'KATEGORİ NO' , uk_adi as 'KATEGORİ ADI' from urun_kategori order by uk_id desc";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
        }

        private void silToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int ukid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["KATEGORİ NO"].Value);

            string sql = " delete from urun_kategori where uk_id=" + ukid;

            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
            baglan.Open();
            SqlCommand cmd = new SqlCommand(sql, baglan);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            baglan.Close();
            doldur();
            MessageBox.Show("Silme İşlemi Tamamlandı");
            kapat();
        }

        private void güncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ukid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["KATEGORİ NO"].Value);
            txtAdi.Text = isim(ukid);
            guncelleDurum = true;
            guncelleId = ukid;
            button1.Text = "GÜNCELLE";
            kapat();
        }

        private bool varmi()
        {
            string sql = " select * from urun_kategori where uk_adi like '" + txtAdi.Text + "'";

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

        private string isim(int ID)
        {
            string isim = "hata";
            string sql = " select * from urun_kategori where uk_id like '" +ID+ "'";

            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows == true)
            {
                isim = Convert.ToString(rd["uk_adi"]);
            }

            return isim;
            
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            int ukid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["KATEGORİ NO"].Value);
            txtAdi.Text = isim(ukid);
            guncelleDurum = true;
            guncelleId = ukid;
            button1.Text = "GÜNCELLE";
            kapat();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int ukid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["KATEGORİ NO"].Value);

            string sql = " delete from urun_kategori where uk_id=" + ukid;

            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
            baglan.Open();
            SqlCommand cmd = new SqlCommand(sql, baglan);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            baglan.Close();
            doldur();
            MessageBox.Show("Silme İşlemi Tamamlandı");
            kapat();
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

        private void txtAdi_Click(object sender, EventArgs e)
        {
            kapat();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            ac();
        }
    }
}
