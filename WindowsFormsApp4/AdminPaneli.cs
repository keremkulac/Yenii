﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class AdminPaneli : Form
    {


        static VeritabaniSinifi connect = new VeritabaniSinifi();
        public static SqlConnection _connection = new SqlConnection(connect.BaglantiAdresi);
        string secilmisItem = null;
        public void LoginFormDon()
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }
        public AdminPaneli()
        {
            InitializeComponent();
        }
        KullanicilarDatabase data = new KullanicilarDatabase();
        ItemlerDatabase itemlerData = new ItemlerDatabase();


        void LoadKullanicilar()
        {
            DataSet ds = data.KullanicilariCek();
            dataGridView1.DataSource = ds.Tables[0];

        }

        void OnaylanmamisBakiyeKullanicilari()
        {
            DataSet ds = data.BakiyeleriOnaylanmamisKullanicilar();
            dataGridView3.DataSource = ds.Tables[0];
        }

        void LoadItemler()
        {
            DataSet ds = itemlerData.ItemleriCekByItemOnay(1); // itemOnay 1 olanları çekicek
            dataGridView4.DataSource = ds.Tables[0];
        }

        

        private void AdminPaneli_Load(object sender, EventArgs e)
        {
            LoadKullanicilar();
            LoadItemler();
            OnaylanmamisBakiyeKullanicilari();
        }


        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                secilmisItem = row.Cells[0].Value.ToString();
                //...
 
            }
        }


     

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            _connection.Open();
            string itemId = dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[5].Value.ToString();
            string komutString = String.Format("UPDATE itemler SET itemOnay=@itemOnay WHERE itemId = {0}", Int32.Parse(itemId));
            SqlCommand komut = new SqlCommand(komutString, _connection);
            komut.Parameters.AddWithValue("@itemOnay", 0);
            //Parametrelerimize Form üzerinde ki kontrollerden girilen verileri aktarıyoruz.
            komut.ExecuteNonQuery();
            //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
            _connection.Close();
            MessageBox.Show("Item onay bilgisi Güncellendi.");

            LoadItemler();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            _connection.Open();
            string kulAdi = dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[0].Value.ToString();
            int beklemedekiBakiye = int.Parse(dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[10].Value.ToString());
            data.BakiyeEkle(kulAdi, beklemedekiBakiye);
            OnaylanmamisBakiyeKullanicilari();
            LoadKullanicilar();
            _connection.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
