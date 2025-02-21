﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    class SatinAlmaEmriDatabase
    {

        static VeritabaniSinifi connect = new VeritabaniSinifi();
        public static SqlConnection _connection = new SqlConnection(connect.BaglantiAdresi);
        public void VeritabaninaEmirVer(SatinAlmaEmri AlmaEmri)
        {

            ConnectionControl();
            SqlCommand command = new SqlCommand(
                "Insert into AlimEmirleri values(@EmirSahibi,@AlinacakItem,@Miktar,@Fiyat)", _connection);
            command.Parameters.AddWithValue("@EmirSahibi", AlmaEmri.emirSahibi);
            command.Parameters.AddWithValue("@AlinacakItem", AlmaEmri.alinacakItem);
            command.Parameters.AddWithValue("@Miktar", AlmaEmri.miktar);
            command.Parameters.AddWithValue("@Fiyat", AlmaEmri.fiyat);
            System.Windows.Forms.MessageBox.Show("Sayın " + AlmaEmri.emirSahibi + " Satın alma emrinizi aldık! " + AlmaEmri.fiyat + " fiyatında " + AlmaEmri.alinacakItem + " ürünü satışa çıkarsa otomatik alım sağlanacak.");
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public DataSet EmirleriCek(string urunAdi, double urunFiyat)
        {
            ConnectionControl();
            SqlDataAdapter da = new SqlDataAdapter(String.Format("SELECT [Emir_ID]=EmirID, [Urun_Adi]=AlinacakItem, [Urun_Fiyat]=Fiyat, [Urun_Miktar]=Miktar, [Alici]=EmirSahibi FROM AlimEmirleri WHERE AlinacakItem='{0}' AND Fiyat={1}", urunAdi, urunFiyat), _connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            _connection.Close();
            return ds;

        }

        public void EmiriSil(int emirID)
        {
            ConnectionControl();
            SqlCommand command = new SqlCommand(string.Format("DELETE FROM AlimEmirleri WHERE EmirID={0}", emirID), _connection);
            command.ExecuteNonQuery();
            System.Windows.Forms.MessageBox.Show(String.Format("{0} Id'li alım emri veri tabanından başarıyla silindi", emirID));
            _connection.Close();
        }

        public void ConnectionControl()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }
    }
}
