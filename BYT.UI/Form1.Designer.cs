namespace BYT.UI
{
    partial class Form1
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
            this.btnKontrolGonder = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnBeyanOlustur = new System.Windows.Forms.Button();
            this.btnSonucSorgula = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.btnKalemOlustur = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnKontrolGonder
            // 
            this.btnKontrolGonder.Location = new System.Drawing.Point(983, 116);
            this.btnKontrolGonder.Name = "btnKontrolGonder";
            this.btnKontrolGonder.Size = new System.Drawing.Size(176, 46);
            this.btnKontrolGonder.TabIndex = 0;
            this.btnKontrolGonder.Text = "Kontrol Gönderimi";
            this.btnKontrolGonder.UseVisualStyleBackColor = true;
            this.btnKontrolGonder.Visible = false;
            this.btnKontrolGonder.Click += new System.EventHandler(this.btnKontrolGonder_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(965, 140);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridView1_RowStateChanged);
            // 
            // btnBeyanOlustur
            // 
            this.btnBeyanOlustur.Location = new System.Drawing.Point(983, 12);
            this.btnBeyanOlustur.Name = "btnBeyanOlustur";
            this.btnBeyanOlustur.Size = new System.Drawing.Size(176, 46);
            this.btnBeyanOlustur.TabIndex = 2;
            this.btnBeyanOlustur.Text = "Beyanname Oluştur";
            this.btnBeyanOlustur.UseVisualStyleBackColor = true;
            this.btnBeyanOlustur.Click += new System.EventHandler(this.btnBeyanOlustur_Click);
            // 
            // btnSonucSorgula
            // 
            this.btnSonucSorgula.Location = new System.Drawing.Point(853, 380);
            this.btnSonucSorgula.Name = "btnSonucSorgula";
            this.btnSonucSorgula.Size = new System.Drawing.Size(176, 46);
            this.btnSonucSorgula.TabIndex = 3;
            this.btnSonucSorgula.Text = "Sonuç Sorgula";
            this.btnSonucSorgula.UseVisualStyleBackColor = true;
            this.btnSonucSorgula.Visible = false;
            this.btnSonucSorgula.Click += new System.EventHandler(this.btnSonucSorgula_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(12, 167);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(1017, 206);
            this.dataGridView2.TabIndex = 4;
            this.dataGridView2.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridView2_RowStateChanged);
            // 
            // btnKalemOlustur
            // 
            this.btnKalemOlustur.Location = new System.Drawing.Point(983, 64);
            this.btnKalemOlustur.Name = "btnKalemOlustur";
            this.btnKalemOlustur.Size = new System.Drawing.Size(176, 46);
            this.btnKalemOlustur.TabIndex = 5;
            this.btnKalemOlustur.Text = "Kalem Oluştur";
            this.btnKalemOlustur.UseVisualStyleBackColor = true;
            this.btnKalemOlustur.Visible = false;
            this.btnKalemOlustur.Click += new System.EventHandler(this.btnKalemOlustur_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(89, 406);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 29);
            this.button1.TabIndex = 6;
            this.button1.Text = "TCK Kontrol";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(202, 409);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(169, 22);
            this.textBox1.TabIndex = 7;
            this.textBox1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 456);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnKalemOlustur);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.btnSonucSorgula);
            this.Controls.Add(this.btnBeyanOlustur);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnKontrolGonder);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnKontrolGonder;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnBeyanOlustur;
        private System.Windows.Forms.Button btnSonucSorgula;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btnKalemOlustur;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

