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
    public partial class formMusteriListesi : Form
    {
        public formMusteriListesi()
        {
            InitializeComponent();
        }

        private void formMusteriListesi_Load(object sender, EventArgs e)
        {
            doldur();
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
        }

        public void doldur()
        {
            string sql = " select m_id as 'MÜŞTERİ NO' , m_adi as 'ADI', " +
                            " m_soyadi as 'SOYADI', m_adres as 'ADRES',m_tel as 'TELEFON' ,m_firmaadi as 'FİRMA' " +
                            " from musteri  order by m_id desc ";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
        }

        private void txtKullaniciAdi_TextChanged(object sender, EventArgs e)
        {
            string sql = " select m_id as 'MÜŞTERİ NO' , m_firmaadi as 'FİRMA', " +
            " m_adi as 'MÜŞTERİ', m_tel as 'TELEFON',m_adres as 'ADRES'  " +
            " from musteri  " +
            " where m_adi like '%" + txtKullaniciAdi.Text + "%' or  " +
            " m_firmaadi like '%" + txtKullaniciAdi.Text + "%' or" +
            " m_soyadi like '%" + txtKullaniciAdi.Text + "%' or" +
            " m_adres like '%" + txtKullaniciAdi.Text + "%' or" +
            " m_tel like '%" + txtKullaniciAdi.Text + "%' order by m_id desc";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
            kapat();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int musteriid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["MÜŞTERİ NO"].Value);

            string sql = " delete from musteri where m_id=" + musteriid;

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
            int musteriid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["MÜŞTERİ NO"].Value);

            formMusteriGuncelle a = new formMusteriGuncelle();
            a.musteriid = musteriid;
            a.ShowDialog();
            kapat();
        }

        private void formMusteriListesi_Activated(object sender, EventArgs e)
        {
            doldur();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["MÜŞTERİ NO"].Value != null)
            {
                int musteriid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["MÜŞTERİ NO"].Value);

                formMusteriGuncelle a = new formMusteriGuncelle();
                a.musteriid = musteriid;
                a.ShowDialog();
                kapat();
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["MÜŞTERİ NO"].Value != null)
            {
                int musteriid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["MÜŞTERİ NO"].Value);

                string sql = " delete from musteri where m_id=" + musteriid;

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
            else
                MessageBox.Show("Lütfen Bir Müşteri Seçiniz!");
            
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            btnGuncelle.Enabled = true;
            btnSil.Enabled = true;
        }

        public void kapat()
        {
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
        }
    }
}
