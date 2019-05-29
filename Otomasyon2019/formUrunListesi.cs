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
    public partial class formUrunListesi : Form
    {
        public formUrunListesi()
        {
            InitializeComponent();
        }
        bool guncelleDurum = false;
        int guncelleId = 0;

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
        }

        public void listele()
        {
            string sql = " select urun.u_id as 'ÜRÜN KODU', " +
                " urun_kategori.uk_adi as  'KATEGORİ', urun.u_adi as 'ÜRÜN', " +
                " u_fiyat as 'FİYAT', u_stok as 'STOK'" +
                " from urun left join urun_kategori on " +
                " urun_kategori.uk_id=urun.uk_id  order by urun.u_id desc";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
            //cbKategori.Text = "Lütfen Bir Kategori Seçiniz";
        }

        private void formUrunListesi_Load(object sender, EventArgs e)
        {
            cbDoldur();
            listele();
            kapat();
            cbKategori.Text = "Lütfen Bir Kategori Seçiniz";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtAdi.Text != "" && txtFiyat.Text != "" && cbKategori.Text != "Lütfen Bir Kategori Seçiniz")
            {

                if (guncelleDurum != true)
                {
                    if (varmi() == false)
                    {

                       string sql = " insert into urun values('" + txtAdi.Text + "','" + Convert.ToInt32(cbKategori.SelectedValue) + "','" + txtFiyat.Text + "','" + txtStok.Text + "') ";

                       SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                       baglan.Open();
                       SqlCommand cmd = new SqlCommand(sql, baglan);
                       cmd.ExecuteNonQuery();
                       cmd.Dispose();
                       baglan.Close();
                       MessageBox.Show("Kayıt Yapıldı");
                       listele();
                       txtAdi.Text = "";
                       txtStok.Text = "";
                       txtFiyat.Text = "";

                    }
                    if (varmi() == true)
                    {
                        MessageBox.Show("Böyle Bir Ürün Var!");
                        txtAdi.Text = "";
                        txtStok.Text = "";
                        txtFiyat.Text = "";
                        listele();
                    }

                }

                if (guncelleDurum == true)
                {
                    string sql = " update urun set u_adi='" + txtAdi.Text + "', u_fiyat='" + txtFiyat.Text + "',uk_id='" + cbKategori.SelectedValue + "',u_stok='" + txtStok.Text + "' where u_id='" + guncelleId + "'";
                    SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                    baglan.Open();
                    SqlCommand cmd = new SqlCommand(sql, baglan);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    baglan.Close();
                    //this.Close();
                    MessageBox.Show("Ürün Bilgileri Güncellendi");
                    listele();
                    guncelleDurum = false;
                    guncelleId = 0;
                    button1.Text = "KAYDET";
                    listele();
                    txtAdi.Text = "";
                    txtStok.Text = "";
                    txtFiyat.Text = "";
                }
            }
            else
            {
                if(cbKategori.Text == "Lütfen Bir Kategori Seçiniz")
                {
                    MessageBox.Show("Lütfen Bir Kategori Seçiniz");
                }
                if(txtAdi.Text == "" && txtFiyat.Text == "")
                    MessageBox.Show("Girilen Bilgiler Eksikti");
            }

            kapat();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int uid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ÜRÜN KODU"].Value);

            string sql = " delete from urun where u_id=" + uid;

            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
            baglan.Open();
            SqlCommand cmd = new SqlCommand(sql, baglan);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            baglan.Close();
            listele();
            MessageBox.Show("Silme İşlemi Tamamlandı");
            kapat();
        }

        private void güncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            kapat();
            int uid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ÜRÜN KODU"].Value);
            txtAdi.Text = isim(uid);
            cbKategori.SelectedValue = kategori(uid);
            txtFiyat.Text = Convert.ToString(fiyat(uid));
            txtStok.Text = Convert.ToString(stok(uid));
            guncelleDurum = true;
            guncelleId = uid;
            button1.Text = "Güncelle";
            guncelle();
        }

        private void guncelle()
        {

             kapat();
             int uid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ÜRÜN KODU"].Value);
             txtAdi.Text = isim(uid);
             cbKategori.SelectedValue = kategori(uid);
             txtFiyat.Text = Convert.ToString(fiyat(uid));
             txtStok.Text = Convert.ToString(stok(uid));
             guncelleDurum = true;
             guncelleId = uid;
             button1.Text = "Güncelle";
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            kapat();
            int uid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ÜRÜN KODU"].Value);
            txtAdi.Text = isim(uid);
            cbKategori.SelectedValue = kategori(uid);
            txtFiyat.Text = Convert.ToString(fiyat(uid));
            txtStok.Text = Convert.ToString(stok(uid));
            guncelleDurum = true;
            guncelleId = uid;
            button1.Text = "Güncelle";

            //guncelle();
        }


        private bool varmi()
        {
            string sql = " select * from urun where u_adi like '" + txtAdi.Text + "'";

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
            //ürün varsa 1 yoksa 0 döner
        }

        private string isim(int ID)
        {
            string isim = "hata";
            string sql = " select u_adi from urun where u_id like '" + ID + "'";

            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows == true)
            {
                isim = Convert.ToString(rd["u_adi"]);
            }

            return isim;

        }
        private int kategori(int ID)
        {
            int kategori = 0;
            string sql = " select uk_id from urun where u_id = '" + ID + "'";

            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows == true)
            {
                kategori = Convert.ToInt32(rd["uk_id"]);
            }
            else
                MessageBox.Show("Hataaaa");

            return kategori;

        }
        private int stok(int ID)
        {
            int stok = 0;
            string sql = " select u_stok from urun where u_id like '" + ID + "'";

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

            return stok;

        }
        private int fiyat(int ID)
        {
            int fiyat = 0;
            string sql = " select u_fiyat from urun where u_id like '" + ID + "'";

            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows == true)
            {
                fiyat = Convert.ToInt32(rd["u_fiyat"]);
            }

            return fiyat;
        }

        private void txtAdi_TextChanged(object sender, EventArgs e)
        {
            string sql = " select urun.u_id as 'ÜRÜN KODU', " +
                " urun_kategori.uk_adi as  'KATEGORİ', urun.u_adi as 'ÜRÜN', " +
                " u_fiyat as 'FİYAT', u_stok as 'STOK'" +
                " from urun left join urun_kategori on " +
                " urun_kategori.uk_id=urun.uk_id where u_adi like '%" + txtAdi.Text + "%' order by urun.u_id desc";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
            if(txtAdi.Text == "")
            {
                listele();
                guncelleDurum = false;
                guncelleId = 0;
                button1.Text = "KAYDET";
                txtAdi.Text = "";
                txtStok.Text = "";
                txtFiyat.Text = "";
            }
            kapat();
        }

        private void txtFiyat_TextChanged(object sender, EventArgs e)
        {
            string sql = " select urun.u_id as 'ÜRÜN KODU', " +
                " urun_kategori.uk_adi as  'KATEGORİ', urun.u_adi as 'ÜRÜN', " +
                " u_fiyat as 'FİYAT', u_stok as 'STOK' " +
                " from urun left join urun_kategori on " +
                " urun_kategori.uk_id=urun.uk_id where u_fiyat like '%" + txtFiyat.Text + "%'  order by urun.u_id desc";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
            if (txtFiyat.Text == "")
            {
                listele();
            }
            kapat();
        }

        private void cbKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = " select urun.u_id as 'ÜRÜN KODU', " +
                " urun_kategori.uk_adi as  'KATEGORİ', urun.u_adi as 'ÜRÜN', " +
                " u_fiyat as 'FİYAT', u_stok as 'STOK' " +
                " from urun left join urun_kategori on " +
                " urun_kategori.uk_id=urun.uk_id where urun.uk_id like '" + cbKategori.SelectedValue + "' order by urun.u_id desc";

            SqlConnection con = new SqlConnection(anaForm.baglanti);
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
            kapat();
        }

        private void formUrunListesi_Activated(object sender, EventArgs e)
        {
            cbDoldur();
            listele();
            kapat();
            cbKategori.Text = "Lütfen Bir Kategori Seçiniz";
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int uid = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ÜRÜN KODU"].Value);

            string sql = " delete from urun where u_id=" + uid;

            SqlConnection baglan = new SqlConnection(anaForm.baglanti);
            baglan.Open();
            SqlCommand cmd = new SqlCommand(sql, baglan);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            baglan.Close();
            listele();
            MessageBox.Show("Silme İşlemi Tamamlandı");
            kapat();
        }

        public void kapat()
        {
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            btnGuncelle.Enabled = true;
            btnSil.Enabled = true;
        }
    }
}
