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
    public partial class formMusteriGuncelle : Form
    {
        public int musteriid;

        public formMusteriGuncelle()
        {
            InitializeComponent();
        }

        private void formMusteriGuncelle_Load(object sender, EventArgs e)
        {
            string sql = " select * from musteri where m_id='" + musteriid + "' ";
            SqlConnection sqlconn = new SqlConnection(anaForm.baglanti);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlconn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows == true)
            {
                txtAdi.Text = Convert.ToString(rd["m_adi"]);
                txtSoyadı.Text = Convert.ToString(rd["m_soyadi"]);
                txtAdres.Text = Convert.ToString(rd["m_adres"]);
                txtFirmaAdi.Text = Convert.ToString(rd["m_firmaadi"]);
                txtTelefon.Text = Convert.ToString(rd["m_tel"]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if((txtAdi.Text != "") && (txtSoyadı.Text != ""))
            {
                string sql = " update musteri set m_adi='" + txtAdi.Text + "' , m_soyadi='" + txtSoyadı.Text + "' , m_adres='" + txtAdres.Text + "' , m_tel='" + txtTelefon.Text + "' , m_firmaadi='" + txtFirmaAdi.Text + "'  where m_id=" + musteriid;

                SqlConnection baglan = new SqlConnection(anaForm.baglanti);
                baglan.Open();
                SqlCommand cmd = new SqlCommand(sql, baglan);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                baglan.Close();
                this.Close();
            }
        }
    }
}
