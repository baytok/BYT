namespace BYT.UI
{
    partial class BYTIslemler
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label11 = new System.Windows.Forms.Label();
            this.txtsifre = new System.Windows.Forms.TextBox();
            this.xmlSonuc = new XmlRender.XmlBrowser();
            this.xmlGidecekVeri = new XmlRender.XmlBrowser();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnGIN = new System.Windows.Forms.Button();
            this.buttonETGB = new System.Windows.Forms.Button();
            this.btnKontrol = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnTescil = new System.Windows.Forms.Button();
            this.btnOzet = new System.Windows.Forms.Button();
            this.btnXml = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.btnSira = new System.Windows.Forms.Button();
            this.btnBeyanname = new System.Windows.Forms.Button();
            this.btnImza = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxGonderimTuru = new System.Windows.Forms.ComboBox();
            this.txtTescilTarih = new System.Windows.Forms.TextBox();
            this.txtBeyannameNo = new System.Windows.Forms.TextBox();
            this.btnIptal = new System.Windows.Forms.Button();
            this.btnTescilCevabi = new System.Windows.Forms.Button();
            this.TxtDurum = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TxtBasTarih = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TxtGidTarih = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtGuidTarih = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtTip = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtKullanici = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtGuid = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtRefID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rchImza = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(113, 683);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 17);
            this.label11.TabIndex = 102;
            this.label11.Text = "Şifre";
            // 
            // txtsifre
            // 
            this.txtsifre.Location = new System.Drawing.Point(158, 680);
            this.txtsifre.Margin = new System.Windows.Forms.Padding(4);
            this.txtsifre.Name = "txtsifre";
            this.txtsifre.PasswordChar = '*';
            this.txtsifre.Size = new System.Drawing.Size(132, 22);
            this.txtsifre.TabIndex = 101;
            // 
            // xmlSonuc
            // 
            this.xmlSonuc.Location = new System.Drawing.Point(735, 238);
            this.xmlSonuc.Margin = new System.Windows.Forms.Padding(4);
            this.xmlSonuc.MinimumSize = new System.Drawing.Size(27, 25);
            this.xmlSonuc.Name = "xmlSonuc";
            this.xmlSonuc.Size = new System.Drawing.Size(631, 430);
            this.xmlSonuc.TabIndex = 99;
            this.xmlSonuc.XmlDocument = null;
            this.xmlSonuc.XmlDocumentTransformType = XmlRender.XmlBrowser.XslTransformType.XSL;
            this.xmlSonuc.XmlText = "";
            // 
            // xmlGidecekVeri
            // 
            this.xmlGidecekVeri.Location = new System.Drawing.Point(2, 217);
            this.xmlGidecekVeri.Margin = new System.Windows.Forms.Padding(4);
            this.xmlGidecekVeri.MinimumSize = new System.Drawing.Size(27, 25);
            this.xmlGidecekVeri.Name = "xmlGidecekVeri";
            this.xmlGidecekVeri.Size = new System.Drawing.Size(725, 396);
            this.xmlGidecekVeri.TabIndex = 98;
            this.xmlGidecekVeri.XmlDocument = null;
            this.xmlGidecekVeri.XmlDocumentTransformType = XmlRender.XmlBrowser.XslTransformType.XSL;
            this.xmlGidecekVeri.XmlText = "";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(617, 184);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(77, 28);
            this.button3.TabIndex = 97;
            this.button3.Text = "TEXT";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(537, 184);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(77, 28);
            this.button4.TabIndex = 96;
            this.button4.Text = "XML";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnGIN);
            this.groupBox3.Controls.Add(this.buttonETGB);
            this.groupBox3.Controls.Add(this.btnKontrol);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.btnTescil);
            this.groupBox3.Controls.Add(this.btnOzet);
            this.groupBox3.Location = new System.Drawing.Point(738, 149);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(628, 64);
            this.groupBox3.TabIndex = 95;
            this.groupBox3.TabStop = false;
            // 
            // btnGIN
            // 
            this.btnGIN.Location = new System.Drawing.Point(539, 18);
            this.btnGIN.Margin = new System.Windows.Forms.Padding(4);
            this.btnGIN.Name = "btnGIN";
            this.btnGIN.Size = new System.Drawing.Size(81, 28);
            this.btnGIN.TabIndex = 49;
            this.btnGIN.Text = "GIN";
            this.btnGIN.UseVisualStyleBackColor = true;
            this.btnGIN.Click += new System.EventHandler(this.btnGIN_Click);
            // 
            // buttonETGB
            // 
            this.buttonETGB.Location = new System.Drawing.Point(429, 18);
            this.buttonETGB.Margin = new System.Windows.Forms.Padding(4);
            this.buttonETGB.Name = "buttonETGB";
            this.buttonETGB.Size = new System.Drawing.Size(100, 28);
            this.buttonETGB.TabIndex = 48;
            this.buttonETGB.Text = "ETGB";
            this.buttonETGB.UseVisualStyleBackColor = true;
            this.buttonETGB.Click += new System.EventHandler(this.buttonETGB_Click);
            // 
            // btnKontrol
            // 
            this.btnKontrol.Location = new System.Drawing.Point(8, 18);
            this.btnKontrol.Margin = new System.Windows.Forms.Padding(4);
            this.btnKontrol.Name = "btnKontrol";
            this.btnKontrol.Size = new System.Drawing.Size(77, 28);
            this.btnKontrol.TabIndex = 22;
            this.btnKontrol.Text = "Kontrol";
            this.btnKontrol.UseVisualStyleBackColor = true;
            this.btnKontrol.Click += new System.EventHandler(this.btnKontrol_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(183, 18);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 28);
            this.button2.TabIndex = 47;
            this.button2.Text = "Hat";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnTescil
            // 
            this.btnTescil.Location = new System.Drawing.Point(93, 18);
            this.btnTescil.Margin = new System.Windows.Forms.Padding(4);
            this.btnTescil.Name = "btnTescil";
            this.btnTescil.Size = new System.Drawing.Size(81, 28);
            this.btnTescil.TabIndex = 23;
            this.btnTescil.Text = "Tescil";
            this.btnTescil.UseVisualStyleBackColor = true;
            this.btnTescil.Click += new System.EventHandler(this.btnTescil_Click);
            // 
            // btnOzet
            // 
            this.btnOzet.Location = new System.Drawing.Point(273, 18);
            this.btnOzet.Margin = new System.Windows.Forms.Padding(4);
            this.btnOzet.Name = "btnOzet";
            this.btnOzet.Size = new System.Drawing.Size(148, 28);
            this.btnOzet.TabIndex = 24;
            this.btnOzet.Text = "Özet Beyan Gönder";
            this.btnOzet.UseVisualStyleBackColor = true;
            this.btnOzet.Click += new System.EventHandler(this.btnOzet_Click);
            // 
            // btnXml
            // 
            this.btnXml.Location = new System.Drawing.Point(453, 677);
            this.btnXml.Margin = new System.Windows.Forms.Padding(4);
            this.btnXml.Name = "btnXml";
            this.btnXml.Size = new System.Drawing.Size(148, 27);
            this.btnXml.TabIndex = 94;
            this.btnXml.Text = "Açık XML";
            this.btnXml.UseVisualStyleBackColor = true;
            this.btnXml.Visible = false;
            this.btnXml.Click += new System.EventHandler(this.btnXml_Click);
            // 
            // btnSil
            // 
            this.btnSil.Location = new System.Drawing.Point(1146, 69);
            this.btnSil.Margin = new System.Windows.Forms.Padding(4);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(220, 28);
            this.btnSil.TabIndex = 93;
            this.btnSil.Text = "İşlemde Kalmış Mesajı Silme";
            this.btnSil.UseVisualStyleBackColor = true;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // btnSira
            // 
            this.btnSira.Location = new System.Drawing.Point(1242, 30);
            this.btnSira.Margin = new System.Windows.Forms.Padding(4);
            this.btnSira.Name = "btnSira";
            this.btnSira.Size = new System.Drawing.Size(124, 28);
            this.btnSira.TabIndex = 92;
            this.btnSira.Text = "Sıra Kontrol";
            this.btnSira.UseVisualStyleBackColor = true;
            this.btnSira.Click += new System.EventHandler(this.btnSira_Click);
            // 
            // btnBeyanname
            // 
            this.btnBeyanname.Location = new System.Drawing.Point(94, 155);
            this.btnBeyanname.Margin = new System.Windows.Forms.Padding(4);
            this.btnBeyanname.Name = "btnBeyanname";
            this.btnBeyanname.Size = new System.Drawing.Size(220, 28);
            this.btnBeyanname.TabIndex = 91;
            this.btnBeyanname.Text = "Beyannemeyi Görüntüle";
            this.btnBeyanname.UseVisualStyleBackColor = true;
            this.btnBeyanname.Visible = false;
            // 
            // btnImza
            // 
            this.btnImza.Location = new System.Drawing.Point(300, 677);
            this.btnImza.Margin = new System.Windows.Forms.Padding(4);
            this.btnImza.Name = "btnImza";
            this.btnImza.Size = new System.Drawing.Size(145, 27);
            this.btnImza.TabIndex = 90;
            this.btnImza.Text = "İmzala";
            this.btnImza.UseVisualStyleBackColor = true;
            this.btnImza.Click += new System.EventHandler(this.btnImza_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxGonderimTuru);
            this.groupBox2.Location = new System.Drawing.Point(932, 31);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(183, 116);
            this.groupBox2.TabIndex = 89;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Gönderim Türü";
            this.groupBox2.Visible = false;
            // 
            // comboBoxGonderimTuru
            // 
            this.comboBoxGonderimTuru.FormattingEnabled = true;
            this.comboBoxGonderimTuru.Items.AddRange(new object[] {
            "EGEWS",
            "EGEWS-TEST"});
            this.comboBoxGonderimTuru.Location = new System.Drawing.Point(11, 25);
            this.comboBoxGonderimTuru.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxGonderimTuru.Name = "comboBoxGonderimTuru";
            this.comboBoxGonderimTuru.Size = new System.Drawing.Size(168, 24);
            this.comboBoxGonderimTuru.TabIndex = 0;
            // 
            // txtTescilTarih
            // 
            this.txtTescilTarih.BackColor = System.Drawing.Color.Lime;
            this.txtTescilTarih.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTescilTarih.ForeColor = System.Drawing.Color.DarkRed;
            this.txtTescilTarih.Location = new System.Drawing.Point(317, 20);
            this.txtTescilTarih.Margin = new System.Windows.Forms.Padding(4);
            this.txtTescilTarih.Name = "txtTescilTarih";
            this.txtTescilTarih.ReadOnly = true;
            this.txtTescilTarih.Size = new System.Drawing.Size(132, 24);
            this.txtTescilTarih.TabIndex = 88;
            this.txtTescilTarih.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTescilTarih.Visible = false;
            // 
            // txtBeyannameNo
            // 
            this.txtBeyannameNo.BackColor = System.Drawing.Color.Lime;
            this.txtBeyannameNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtBeyannameNo.ForeColor = System.Drawing.Color.DarkRed;
            this.txtBeyannameNo.Location = new System.Drawing.Point(12, 20);
            this.txtBeyannameNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtBeyannameNo.Name = "txtBeyannameNo";
            this.txtBeyannameNo.ReadOnly = true;
            this.txtBeyannameNo.Size = new System.Drawing.Size(296, 24);
            this.txtBeyannameNo.TabIndex = 87;
            this.txtBeyannameNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBeyannameNo.Visible = false;
            // 
            // btnIptal
            // 
            this.btnIptal.Location = new System.Drawing.Point(1116, 30);
            this.btnIptal.Margin = new System.Windows.Forms.Padding(4);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(119, 28);
            this.btnIptal.TabIndex = 86;
            this.btnIptal.Text = "Mesajı İptal Et";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Visible = false;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // btnTescilCevabi
            // 
            this.btnTescilCevabi.Location = new System.Drawing.Point(1186, 105);
            this.btnTescilCevabi.Margin = new System.Windows.Forms.Padding(4);
            this.btnTescilCevabi.Name = "btnTescilCevabi";
            this.btnTescilCevabi.Size = new System.Drawing.Size(180, 30);
            this.btnTescilCevabi.TabIndex = 85;
            this.btnTescilCevabi.Text = "Tescil Cevabı Kontrol Et";
            this.btnTescilCevabi.UseVisualStyleBackColor = true;
            this.btnTescilCevabi.Visible = false;
            this.btnTescilCevabi.Click += new System.EventHandler(this.btnTescilCevabi_Click);
            // 
            // TxtDurum
            // 
            this.TxtDurum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.TxtDurum.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.TxtDurum.Location = new System.Drawing.Point(462, 20);
            this.TxtDurum.Margin = new System.Windows.Forms.Padding(4);
            this.TxtDurum.Name = "TxtDurum";
            this.TxtDurum.ReadOnly = true;
            this.TxtDurum.Size = new System.Drawing.Size(213, 24);
            this.TxtDurum.TabIndex = 84;
            this.TxtDurum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.TxtBasTarih);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.TxtGidTarih);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.TxtGuidTarih);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.TxtTip);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.TxtKullanici);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.TxtGuid);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TxtRefID);
            this.groupBox1.Location = new System.Drawing.Point(4, 49);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(928, 101);
            this.groupBox1.TabIndex = 83;
            this.groupBox1.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(557, 47);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 17);
            this.label10.TabIndex = 21;
            this.label10.Text = "Başlangıç Tarih";
            // 
            // TxtBasTarih
            // 
            this.TxtBasTarih.Location = new System.Drawing.Point(669, 43);
            this.TxtBasTarih.Margin = new System.Windows.Forms.Padding(4);
            this.TxtBasTarih.Name = "TxtBasTarih";
            this.TxtBasTarih.ReadOnly = true;
            this.TxtBasTarih.Size = new System.Drawing.Size(213, 22);
            this.TxtBasTarih.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(580, 75);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 17);
            this.label9.TabIndex = 19;
            this.label9.Text = "Sonuç Tarih";
            // 
            // TxtGidTarih
            // 
            this.TxtGidTarih.Location = new System.Drawing.Point(669, 71);
            this.TxtGidTarih.Margin = new System.Windows.Forms.Padding(4);
            this.TxtGidTarih.Name = "TxtGidTarih";
            this.TxtGidTarih.ReadOnly = true;
            this.TxtGidTarih.Size = new System.Drawing.Size(213, 22);
            this.TxtGidTarih.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(580, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "Guid Tarih";
            // 
            // TxtGuidTarih
            // 
            this.TxtGuidTarih.Location = new System.Drawing.Point(669, 16);
            this.TxtGuidTarih.Margin = new System.Windows.Forms.Padding(4);
            this.TxtGuidTarih.Name = "TxtGuidTarih";
            this.TxtGuidTarih.ReadOnly = true;
            this.TxtGuidTarih.Size = new System.Drawing.Size(213, 22);
            this.TxtGuidTarih.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(379, 62);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Tip";
            // 
            // TxtTip
            // 
            this.TxtTip.ForeColor = System.Drawing.Color.DarkRed;
            this.TxtTip.Location = new System.Drawing.Point(441, 58);
            this.TxtTip.Margin = new System.Windows.Forms.Padding(4);
            this.TxtTip.Name = "TxtTip";
            this.TxtTip.ReadOnly = true;
            this.TxtTip.Size = new System.Drawing.Size(113, 22);
            this.TxtTip.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(376, 20);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "Kullanıcı";
            // 
            // TxtKullanici
            // 
            this.TxtKullanici.Location = new System.Drawing.Point(441, 16);
            this.TxtKullanici.Margin = new System.Windows.Forms.Padding(4);
            this.TxtKullanici.Name = "TxtKullanici";
            this.TxtKullanici.ReadOnly = true;
            this.TxtKullanici.Size = new System.Drawing.Size(113, 22);
            this.TxtKullanici.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 62);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Guid";
            // 
            // TxtGuid
            // 
            this.TxtGuid.Location = new System.Drawing.Point(55, 58);
            this.TxtGuid.Margin = new System.Windows.Forms.Padding(4);
            this.TxtGuid.Name = "TxtGuid";
            this.TxtGuid.ReadOnly = true;
            this.TxtGuid.Size = new System.Drawing.Size(304, 22);
            this.TxtGuid.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Refarans Numaraası";
            // 
            // TxtRefID
            // 
            this.TxtRefID.Location = new System.Drawing.Point(153, 16);
            this.TxtRefID.Margin = new System.Windows.Forms.Padding(4);
            this.TxtRefID.Name = "TxtRefID";
            this.TxtRefID.ReadOnly = true;
            this.TxtRefID.Size = new System.Drawing.Size(205, 22);
            this.TxtRefID.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 677);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 17);
            this.label3.TabIndex = 82;
            this.label3.Text = "İmzalı Blgi";
            // 
            // rchImza
            // 
            this.rchImza.Location = new System.Drawing.Point(2, 619);
            this.rchImza.Margin = new System.Windows.Forms.Padding(4);
            this.rchImza.Name = "rchImza";
            this.rchImza.Size = new System.Drawing.Size(691, 50);
            this.rchImza.TabIndex = 81;
            this.rchImza.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(743, 217);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 80;
            this.label1.Text = "Giden Data";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 196);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 17);
            this.label4.TabIndex = 79;
            this.label4.Text = "Gelen Data";
            // 
            // BYTIslemler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1404, 717);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtsifre);
            this.Controls.Add(this.xmlSonuc);
            this.Controls.Add(this.xmlGidecekVeri);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnXml);
            this.Controls.Add(this.btnSil);
            this.Controls.Add(this.btnSira);
            this.Controls.Add(this.btnBeyanname);
            this.Controls.Add(this.btnImza);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtTescilTarih);
            this.Controls.Add(this.txtBeyannameNo);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnTescilCevabi);
            this.Controls.Add(this.TxtDurum);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rchImza);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Name = "BYTIslemler";
            this.Text = "BYTIslemler";
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtsifre;
        public XmlRender.XmlBrowser xmlSonuc;
        public XmlRender.XmlBrowser xmlGidecekVeri;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnGIN;
        private System.Windows.Forms.Button buttonETGB;
        private System.Windows.Forms.Button btnKontrol;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnTescil;
        private System.Windows.Forms.Button btnOzet;
        private System.Windows.Forms.Button btnXml;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Button btnSira;
        private System.Windows.Forms.Button btnBeyanname;
        private System.Windows.Forms.Button btnImza;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxGonderimTuru;
        public System.Windows.Forms.TextBox txtTescilTarih;
        public System.Windows.Forms.TextBox txtBeyannameNo;
        private System.Windows.Forms.Button btnIptal;
        private System.Windows.Forms.Button btnTescilCevabi;
        public System.Windows.Forms.TextBox TxtDurum;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox TxtBasTarih;
        internal System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox TxtGidTarih;
        internal System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox TxtGuidTarih;
        internal System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox TxtTip;
        internal System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox TxtKullanici;
        internal System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox TxtGuid;
        internal System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox TxtRefID;
        internal System.Windows.Forms.Label label3;
        public System.Windows.Forms.RichTextBox rchImza;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label label4;
    }
}