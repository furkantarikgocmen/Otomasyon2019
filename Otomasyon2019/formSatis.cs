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
    public partial class formSatis : Form
    {
        public formSatis()
        {
            InitializeComponent();
        }
        bool guncelleDurum = false;
        int guncelleId = 0;
        int urunStok;
        int guncelStok;
        int urunid;

        public void urunListele()
        {
            string sql = " select urun.u_id as 'ÜRÜN KODU', " +
                " urun_kategori.uk_adi as  'KATEGORİ', urun.u_adi as 'ÜRÜN', " +
                " u_fiyat as 'FİYAT', u_stok as 'STOK' " +
                " from urun left join urun_kategori on " +
                " urun_kategori.uk_id=urun.uk_id where urun.u_stok != '0' order by urun.u_id desc";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
            con.Close();
        }

        public void musteriListele()
        {
            string sql = " select m_id as 'MÜŞTERİ NO' , m_firmaadi as 'FİRMA', " +
                            " m_adi as 'MÜŞTERİ', m_tel as 'TELEFON',m_adres as 'ADRES'  " +
                            " from musteri order by m_id desc ";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
        }
        public void satisListele()
        {
            string sql = " select " +
                    " satis.s_id as 'SATIŞ NO' , musteri.m_firmaadi as 'FİRMA', " +
                    " urun_kategori.uk_adi as 'KATEGORİ' , urun.u_adi as 'ÜRÜN ADI', " +
                    " satis.s_adet as 'ADET' , urun.u_fiyat as 'FİYAT', " +
                    " (urun.u_fiyat*satis.s_adet) as 'TUTAR' " +
                    " from satis " +
                    " left join musteri on satis.s_m_id=musteri.m_id " +
                    " left join urun on urun.u_id=satis.s_urun_id " +
                    " left join urun_kategori on urun_kategori.uk_id=urun.uk_id  order by satis.s_id desc";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView3.DataSource = ds.Tables[0];
            con.Close();
        }
        public void cbDoldur()
        {
            string sql_text = "SELECT * FROM urun_kategori  order by uk_id desc";
            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand sorgu = new SqlCommand(sql_text, sqlconn);
            sorgu.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = sorgu;
            DataSet ds = new DataSet();
            da.Fill(ds);
            cbKategori.DataSource = ds.Tables[0];
            cbKategori.DisplayMember = "uk_adi";
            cbKategori.ValueMember = "uk_id";
            sqlconn.Close();
            cbKategori.Text = "Lütfen Bir Kategori Seçiniz";
        }

        private void formSatis_Load(object sender, EventArgs e)
        {
            cbDoldur();
            urunListele();
            musteriListele();
            satisListele();
            kapat();
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
            dataGridView3.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int musteriid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["MÜŞTERİ NO"].Value);
            int urunid = Convert.ToInt32(dataGridView2.CurrentRow.Cells["ÜRÜN KODU"].Value);

            if (guncelleDurum != true)
            {
                if(txtAdet.Text != "")
                {
                    if ((musteriid > 0) && (urunid > 0))
                    {
                        if ((stokGetir() >= Convert.ToInt32(txtAdet.Text)))
                        {
                            string sql = " insert into satis values('" + musteriid + "','" + urunid + "','" + Convert.ToInt32(txtAdet.Text) + "') ";

                            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                            baglan.Open();
                            SqlCommand cmd = new SqlCommand(sql, baglan);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            baglan.Close();
                            MessageBox.Show("Satış Yapıldı");
                            stokDus();
                        }
                        else
                            MessageBox.Show("Stokta Yeterli Ürün Yok. Maksiumum " + stokGetir() + "");
                    }
                    else
                    {
                        MessageBox.Show("Lütfen Bir Müşteri veya Ürün Seçin");
                    }
                }
                else
                    MessageBox.Show("Lütfen Adet Giriniz");

            }

            if (guncelleDurum == true && txtAdet.Text != "")
            {
                if(txtAdet.Text != "")
                {
                    if ((stokGetir() >= Convert.ToInt32(txtAdet.Text)))
                    {
                        if(txtAdet.Text == "")
                        {
                            MessageBox.Show("Stokta Yeterli Ürün Yok. Maksiumum " + stokGetir() + "");
                        }
                        stokGuncelleSag();
                        string sql = " update satis set s_m_id='" + musteriid + "', s_urun_id='" + urunid + "', s_adet='" + txtAdet.Text + "'   where s_id='" + guncelleId + "'";
                        SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                        baglan.Open();
                        SqlCommand cmd = new SqlCommand(sql, baglan);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        baglan.Close();

                        MessageBox.Show("Satış Güncellendi");
                        satisListele();
                        guncelleDurum = false;
                        guncelleId = 0;
                        button1.Text = "KAYDET";
                    }
                    
                        
                }
                else
                    MessageBox.Show("Lütfen Adet Giriniz");
            }

            txtAdet.Text = "";
            txtAdi.Text = "";
            txtUrunAdi.Text = "";
            cbKategori.Text = "Lütfen Bir Kategori Seçiniz";
            satisListele();
            urunListele();
            kapat();
            clearSelection();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int sid = Convert.ToInt32(dataGridView3.CurrentRow.Cells["SATIŞ NO"].Value);
            string urunAdi = Convert.ToString(dataGridView3.CurrentRow.Cells["ÜRÜN ADI"].Value);
            int satilanStok = Convert.ToInt32(dataGridView3.CurrentRow.Cells["ADET"].Value);

            stokİptal(urunAdi, satilanStok);
            stokArtir();

            string sql = " delete from satis where s_id=" + sid;

            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
            baglan.Open();
            SqlCommand cmd = new SqlCommand(sql, baglan);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            baglan.Close();
            
            satisListele();
            MessageBox.Show("Silme İşlemi Tamamlandı");
            urunListele();
            kapat();
        }

        private void güncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int urunKodu = Convert.ToInt32(dataGridView2.CurrentRow.Cells["ÜRÜN KODU"].Value);
            string sql = "select uk_id from urun where u_id = '" + urunKodu + "'";
            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            cbKategori.SelectedValue = rd["uk_id"];
            sqlconn.Close();


            int sid = Convert.ToInt32(dataGridView3.CurrentRow.Cells["SATIŞ NO"].Value);
            txtAdi.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["MÜŞTERİ"].Value);
            txtAdet.Text = Convert.ToString(dataGridView3.CurrentRow.Cells["ADET"].Value);
            txtUrunAdi.Text = Convert.ToString(dataGridView3.CurrentRow.Cells["ÜRÜN ADI"].Value);

            guncelleDurum = true;
            guncelleId = sid;
            button1.Text = "Güncelle";
            kapat();

        }

        private void cbKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = " select urun.u_id as 'ÜRÜN KODU', " +
                " urun_kategori.uk_adi as  'KATEGORİ', urun.u_adi as 'ÜRÜN', " +
                " u_fiyat as 'FİYAT' u_stok as 'STOK' " +
                " from urun left join urun_kategori on " +
                " urun_kategori.uk_id=urun.uk_id where urun_kategori.uk_id like '"+ cbKategori.SelectedValue + "' and urun.u_stok != '0'  order by urun.u_id desc ";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
            con.Close();
            kapat();
        }

        private void txtAdi_TextChanged(object sender, EventArgs e)
        {
            string sql = " select m_id as 'MÜŞTERİ NO' , m_firmaadi as 'FİRMA', " +
                            " m_adi as 'MÜŞTERİ', m_tel as 'TELEON',m_adres as 'ADRES'  " +
                            " from musteri where m_adi like '%"+ txtAdi.Text + "%' or m_firmaadi like '%" + txtAdi.Text + "%' or m_soyadi like '%" + txtAdi.Text + "%' or m_tel like '%" + txtAdi.Text + "%' or m_adres like '%" + txtAdi.Text + "%'  order by m_id desc ";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
            kapat();
        }

        private void txtUrunAdi_TextChanged(object sender, EventArgs e)
        {
            string sql = " select urun.u_id as 'ÜRÜN KODU', " +
                " urun_kategori.uk_adi as  'KATEGORİ', urun.u_adi as 'ÜRÜN', " +
                " u_fiyat as 'FİYAT', u_stok as 'STOK'  " +
                " from urun left join urun_kategori on " +
                " urun_kategori.uk_id=urun.uk_id where urun.u_adi like '%"+ txtUrunAdi.Text + "%' order by urun.u_id desc";
            // or urun_kategori.uk_adi like '%"+ txtUrunAdi.Text +"%' or urun.u_fiyat like '%"+ txtAdi.Text +"%' 
            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
            con.Close();
            kapat();
        }

       /* private string isim(int ID)
        {
            string isim = "hata";
            string sql = " select * from urun_kategori where uk_id like '" + ID + "'";

            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            sqlconn.Close();
            if (rd.HasRows == true)
            {
                isim = Convert.ToString(rd["uk_adi"]);
            }

            return isim;

        }*/

        private int stokGetir()
        {
            int uid = Convert.ToInt32(dataGridView2.CurrentRow.Cells["ÜRÜN KODU"].Value);
            string sql = " select u_stok from urun where u_id like '" + uid + "'";
            int stok = 0;
            
            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows == true)
            {
                stok = Convert.ToInt32(rd["u_stok"]);
            }
            sqlconn.Close();
            return stok;
        }
        
        private void stokDus()
        {
            int hStok = Convert.ToInt32(dataGridView2.CurrentRow.Cells["STOK"].Value);
            int yStok = hStok - Convert.ToInt32(txtAdet.Text);
            int uid = Convert.ToInt32(dataGridView2.CurrentRow.Cells["ÜRÜN KODU"].Value);

            string sql = " update urun set  u_stok='" + yStok + "'   where u_id='" + uid + "'";
            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
            baglan.Open();
            SqlCommand cmd = new SqlCommand(sql, baglan);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            baglan.Close();
        }
        private void stokİptal(string uadi, int satilanStok)
        {
            
            string sql = " select * from urun left join satis satis on urun.u_id = satis.s_urun_id where u_adi='" + uadi + "'";
            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows == true)
            {
                urunid = Convert.ToInt32(rd["u_id"]);
                urunStok = Convert.ToInt32(rd["u_stok"]);
                guncelStok = urunStok + satilanStok;
            }
            sqlconn.Close();
            //stokArtir();
        }

        private void stokArtir()
        {
            string sql = " update urun set u_stok='" + guncelStok + "'   where u_id='" + urunid + "'";
            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
            baglan.Open();
            SqlCommand cmd = new SqlCommand(sql, baglan);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            baglan.Close();
        }

        private void stokGuncelle()
        {
            string sql = " update urun set u_stok='" + guncelStok + "'   where u_id='" + urunid + "'";
            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
            baglan.Open();
            SqlCommand cmd = new SqlCommand(sql, baglan);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            baglan.Close();
        }

        private void stokGuncelleSag()
        {
            string urunAdi = Convert.ToString(dataGridView3.CurrentRow.Cells["ÜRÜN ADI"].Value);
            int satilanAdet = Convert.ToInt32(dataGridView3.CurrentRow.Cells["ADET"].Value);
            int yenideger;

            int hazirStok = stokGetir();
            if(satilanAdet > Convert.ToInt32(txtAdet.Text)) // Azaltma Yapılacağı Zaman
            {
                yenideger = hazirStok + (satilanAdet - Convert.ToInt32(txtAdet.Text));


                string sql = " update urun set u_stok='" + yenideger + "'   where u_adi='" + urunAdi + "'";
                SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                baglan.Open();
                SqlCommand cmd = new SqlCommand(sql, baglan);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                baglan.Close();
            }
            if (satilanAdet < Convert.ToInt32(txtAdet.Text)) //Artırma Yapılacağı Zaman
            {
                int aradakiFark = satilanAdet - Convert.ToInt32(txtAdet.Text);


                yenideger = hazirStok + aradakiFark;

                string sql = " update urun set u_stok='" + yenideger + "'   where u_adi='" + urunAdi + "'";
                SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                baglan.Open();
                SqlCommand cmd = new SqlCommand(sql, baglan);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                baglan.Close();
            }
            
        }

        private void iptalEtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            int sid = Convert.ToInt32(dataGridView3.CurrentRow.Cells["SATIŞ NO"].Value);
            string urunAdi = Convert.ToString(dataGridView3.CurrentRow.Cells["ÜRÜN ADI"].Value);
            int satilanStok = Convert.ToInt32(dataGridView3.CurrentRow.Cells["ADET"].Value);

            stokİptal(urunAdi, satilanStok);
            stokArtir();

            string sql = " delete from satis where s_id=" + sid;

            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
            baglan.Open();
            SqlCommand cmd = new SqlCommand(sql, baglan);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            baglan.Close();

            satisListele();
            MessageBox.Show("Silme İşlemi Tamamlandı");
            urunListele();
            kapat();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int sid = Convert.ToInt32(dataGridView3.CurrentRow.Cells["SATIŞ NO"].Value);

            string sql = " delete from satis where s_id=" + sid;

            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
            baglan.Open();
            SqlCommand cmd = new SqlCommand(sql, baglan);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            baglan.Close();

            satisListele();
            MessageBox.Show("Satış İptal Edildi");
            urunListele();
            kapat();
        }

        private void btnİptal_Click(object sender, EventArgs e)
        {
            if(dataGridView3.CurrentRow.Cells["SATIŞ NO"].Value != null)
            {
                int sid = Convert.ToInt32(dataGridView3.CurrentRow.Cells["SATIŞ NO"].Value);
                string urunAdi = Convert.ToString(dataGridView3.CurrentRow.Cells["ÜRÜN ADI"].Value);
                int satilanStok = Convert.ToInt32(dataGridView3.CurrentRow.Cells["ADET"].Value);

                stokİptal(urunAdi, satilanStok);
                stokArtir();

                string sql = " delete from satis where s_id=" + sid;

                SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                baglan.Open();
                SqlCommand cmd = new SqlCommand(sql, baglan);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                baglan.Close();

                satisListele();
                MessageBox.Show("Silme İşlemi Tamamlandı");
                urunListele();
            }
            else
                MessageBox.Show("Lütfen Bir Satış Seçiniz");
            kapat();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow.Cells["ÜRÜN KODU"].Value != null)
            {
                int urunKodu = Convert.ToInt32(dataGridView2.CurrentRow.Cells["ÜRÜN KODU"].Value);
                string sql = "select uk_id from urun where u_id = '" + urunKodu + "'";
                SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
                sqlconn.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlconn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                cbKategori.SelectedValue = rd["uk_id"];
                sqlconn.Close();
                int sid = Convert.ToInt32(dataGridView3.CurrentRow.Cells["SATIŞ NO"].Value);
                txtAdi.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["MÜŞTERİ"].Value);
                txtAdet.Text = Convert.ToString(dataGridView3.CurrentRow.Cells["ADET"].Value);
                txtUrunAdi.Text = Convert.ToString(dataGridView3.CurrentRow.Cells["ÜRÜN ADI"].Value);

                guncelleDurum = true;
                guncelleId = sid;
                button1.Text = "Güncelle";
            }
            else
                MessageBox.Show("Lütfen Bir Satış Seçiniz");
            kapat();
        }

        private void ac()
        {
            btnSil.Enabled = true;
            btnGuncelle.Enabled = true;
            btnİptal.Enabled = true;
        }

        private void kapat()
        {
            btnSil.Enabled = false;
            btnGuncelle.Enabled = false;
            btnİptal.Enabled = false;
        }
        private void clearSelection()
        {
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
            dataGridView3.ClearSelection();
        }

        private void dataGridView3_Click(object sender, EventArgs e)
        {
            ac();
        }

        private void txtAdet_Click(object sender, EventArgs e)
        {
            kapat();
        }
    }
}
