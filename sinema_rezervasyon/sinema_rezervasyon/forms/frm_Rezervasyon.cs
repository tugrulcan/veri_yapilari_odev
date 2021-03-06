﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinkedListImplementation;
using sinema_rezervasyon.forms;

namespace sinema_rezervasyon
{
    public partial class frm_RezervasyonListe : Form
    {
        public frm_RezervasyonListe()
        {
            InitializeComponent();
            BL.koltukListesi.InsertFirst(new Koltuk() { rezerveEdilebilirMi = true }); //61 node, koltuk adlari karismasin diye.  
        }

        

        private void frm_RezervasyonListe_Load(object sender, EventArgs e)
        {
            butonlariYarat();
            
        }

        private void butonlariYarat()
        {
            Point start_location = new Point() { X = 10, Y = 40 };
            Size button_size = new Size(40, 40);

            for (int i = 0; i < 5; i++)
            {
                start_location.Y += 60;
                start_location.X = 10;
                for (int j = 0; j < 12; j++)
                {
                    int koltuk_no = ((i * 12) + j+1);
                    Button b = new Button() {
                        Location = start_location,
                        Name = "btn_musteri_" + koltuk_no.ToString(),
                        Text = koltuk_no.ToString(),
                        Size = button_size,
                        BackColor = Color.Chartreuse,
                        UseVisualStyleBackColor = false 
                    };
                    b.Click += new System.EventHandler(this.btn_musteri_n_Click);

                    this.Controls.Add(b);
                    start_location.X += 60;

                    BL.koltukListesi.InsertLast(new Koltuk() { rezerveEdilebilirMi = true, koltukNo = koltuk_no });
                }
            }
            var s = BL.koltukListesi.DisplayElements();
            this.txt_toplamMusteriSayisi.Text = BL.koltukListesi.getAvailableSeatCount().ToString();
        }

        public void koltuklariGuncelle()
        {
            Node h = BL.koltukListesi.Head;
            h = h.Next; // pass the empty 0'th
            int koltukNo = 1;
            while (h.Next != null)
            {
                var  b = Controls.Find("btn_musteri_" + koltukNo.ToString(), true)[0];
                
                b = (Button)b; 

                if (h.Data.rezerveEdilebilirMi)
                    b.BackColor = Color.Chartreuse;
                else
                    b.BackColor = Color.Red;

                h = h.Next;
                koltukNo++;
            }
            var t = BL.koltukListesi.getAvailableSeatCount();
            var s = BL.koltukListesi.DisplayElements();
            this.txt_toplamMusteriSayisi.Text = t.ToString();
           

        }

        private void btn_musteri_n_Click(object sender, EventArgs e)
        {
            
            Button b = (Button) sender;
            b.BackColor = Color.Red;

            int koltukNo = Convert.ToInt32(b.Text);

            Node tiklanan = BL.koltukListesi.GetElement(koltukNo);

            if (tiklanan.Data.rezerveEdilebilirMi)
            {
                frm_MusteriBilgi mbilgi = new frm_MusteriBilgi(koltukNo, this);
                mbilgi.Show();
            }
            else
            {
                string s = tiklanan.Data.Ad + " " + tiklanan.Data.Soyad + " koltuk no: " + tiklanan.Data.koltukNo;
                if (MessageBox.Show( s+ " için rezervasyonun iptal edilmesini onaylıyor musunuz?", "Rezervasyon iptali", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    BL.koltukListesi.DeletePos(tiklanan.Data.koltukNo);
                    BL.koltukListesi.InsertPos(tiklanan.Data.koltukNo, new Koltuk() { rezerveEdilebilirMi = true, koltukNo = tiklanan.Data.koltukNo });

                }
            }
            this.koltuklariGuncelle();
            

        }

        private void btn_koltukIptal_Click(object sender, EventArgs e)
        {
            frm_koltukSilme f = new frm_koltukSilme(this);
            f.Show();
        }
    }
}
