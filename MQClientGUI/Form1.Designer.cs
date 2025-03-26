namespace MQClientGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BrokerIPBox = new TextBox();
            Frame1 = new GroupBox();
            button1 = new Button();
            label1 = new Label();
            TemasSubs = new RichTextBox();
            PublishB = new Button();
            button3 = new Button();
            label4 = new Label();
            groupBox2 = new GroupBox();
            label6 = new Label();
            label5 = new Label();
            HistorialBox = new RichTextBox();
            MensajeBox = new RichTextBox();
            button2 = new Button();
            Subs = new Button();
            TextTopic = new TextBox();
            GeneraIP = new Button();
            label3 = new Label();
            AppID = new TextBox();
            label2 = new Label();
            ConectarB = new Button();
            Frame1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // BrokerIPBox
            // 
            BrokerIPBox.Location = new Point(130, 25);
            BrokerIPBox.Name = "BrokerIPBox";
            BrokerIPBox.Size = new Size(377, 23);
            BrokerIPBox.TabIndex = 1;
            BrokerIPBox.TextChanged += textBox2_TextChanged;
            // 
            // Frame1
            // 
            Frame1.Controls.Add(button1);
            Frame1.Controls.Add(label1);
            Frame1.Controls.Add(TemasSubs);
            Frame1.Controls.Add(PublishB);
            Frame1.Controls.Add(button3);
            Frame1.Controls.Add(label4);
            Frame1.Controls.Add(groupBox2);
            Frame1.Controls.Add(MensajeBox);
            Frame1.Controls.Add(button2);
            Frame1.Controls.Add(Subs);
            Frame1.Controls.Add(TextTopic);
            Frame1.Location = new Point(16, 84);
            Frame1.Name = "Frame1";
            Frame1.Size = new Size(1414, 497);
            Frame1.TabIndex = 5;
            Frame1.TabStop = false;
            Frame1.Text = "Siatema";
            Frame1.Enter += groupBox1_Enter;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(255, 255, 128);
            button1.Location = new Point(1202, 233);
            button1.Name = "button1";
            button1.Size = new Size(136, 31);
            button1.TabIndex = 16;
            button1.Text = "Actualizar";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BorderStyle = BorderStyle.Fixed3D;
            label1.ForeColor = SystemColors.ControlText;
            label1.ImageAlign = ContentAlignment.TopRight;
            label1.Location = new Point(1020, 33);
            label1.Name = "label1";
            label1.RightToLeft = RightToLeft.Yes;
            label1.Size = new Size(92, 17);
            label1.TabIndex = 15;
            label1.Text = "Temas Suscritos";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // TemasSubs
            // 
            TemasSubs.Location = new Point(1011, 55);
            TemasSubs.Name = "TemasSubs";
            TemasSubs.Size = new Size(168, 416);
            TemasSubs.TabIndex = 14;
            TemasSubs.Text = "";
            TemasSubs.TextChanged += TemasSubs_TextChanged;
            // 
            // PublishB
            // 
            PublishB.BackColor = Color.FromArgb(255, 255, 128);
            PublishB.Location = new Point(228, 410);
            PublishB.Name = "PublishB";
            PublishB.Size = new Size(136, 31);
            PublishB.TabIndex = 13;
            PublishB.Text = "Publicar";
            PublishB.UseVisualStyleBackColor = false;
            PublishB.Click += button4_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(255, 192, 128);
            button3.Location = new Point(662, 410);
            button3.Name = "button3";
            button3.Size = new Size(136, 31);
            button3.TabIndex = 12;
            button3.Text = "Obtener Mensaje ";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(114, 30);
            label4.Name = "label4";
            label4.Size = new Size(38, 15);
            label4.TabIndex = 7;
            label4.Text = "Tema:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(HistorialBox);
            groupBox2.Location = new Point(542, 52);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(372, 352);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BorderStyle = BorderStyle.Fixed3D;
            label6.ForeColor = SystemColors.ControlText;
            label6.ImageAlign = ContentAlignment.TopRight;
            label6.Location = new Point(213, 32);
            label6.Name = "label6";
            label6.RightToLeft = RightToLeft.Yes;
            label6.Size = new Size(65, 17);
            label6.TabIndex = 8;
            label6.Text = "Contenido";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BorderStyle = BorderStyle.Fixed3D;
            label5.ForeColor = SystemColors.ControlText;
            label5.ImageAlign = ContentAlignment.TopRight;
            label5.Location = new Point(65, 33);
            label5.Name = "label5";
            label5.RightToLeft = RightToLeft.Yes;
            label5.Size = new Size(37, 17);
            label5.TabIndex = 7;
            label5.Text = "Tema";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            label5.Click += label5_Click;
            // 
            // HistorialBox
            // 
            HistorialBox.Location = new Point(46, 52);
            HistorialBox.Name = "HistorialBox";
            HistorialBox.Size = new Size(320, 269);
            HistorialBox.TabIndex = 1;
            HistorialBox.Text = "";
            HistorialBox.TextChanged += richTextBox3_TextChanged;
            // 
            // MensajeBox
            // 
            MensajeBox.Location = new Point(114, 55);
            MensajeBox.Name = "MensajeBox";
            MensajeBox.Size = new Size(371, 349);
            MensajeBox.TabIndex = 8;
            MensajeBox.Text = "";
            MensajeBox.TextChanged += richTextBox1_TextChanged;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(255, 128, 128);
            button2.Location = new Point(373, 410);
            button2.Name = "button2";
            button2.Size = new Size(136, 31);
            button2.TabIndex = 6;
            button2.Text = "Desuscribirse";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // Subs
            // 
            Subs.BackColor = Color.FromArgb(128, 255, 128);
            Subs.Location = new Point(81, 410);
            Subs.Name = "Subs";
            Subs.Size = new Size(136, 31);
            Subs.TabIndex = 5;
            Subs.Text = "Suscribirse";
            Subs.UseVisualStyleBackColor = false;
            Subs.Click += button1_Click;
            // 
            // TextTopic
            // 
            TextTopic.Location = new Point(166, 24);
            TextTopic.Name = "TextTopic";
            TextTopic.Size = new Size(296, 23);
            TextTopic.TabIndex = 4;
            TextTopic.TextChanged += textBox1_TextChanged;
            // 
            // GeneraIP
            // 
            GeneraIP.Location = new Point(987, 21);
            GeneraIP.Name = "GeneraIP";
            GeneraIP.Size = new Size(86, 28);
            GeneraIP.TabIndex = 16;
            GeneraIP.Text = "Generar IP";
            GeneraIP.UseVisualStyleBackColor = true;
            GeneraIP.Click += button6_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(553, 28);
            label3.Name = "label3";
            label3.Size = new Size(46, 15);
            label3.TabIndex = 11;
            label3.Text = "App ID:";
            // 
            // AppID
            // 
            AppID.Location = new Point(604, 25);
            AppID.Name = "AppID";
            AppID.Size = new Size(377, 23);
            AppID.TabIndex = 3;
            AppID.TextChanged += textBox4_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(37, 28);
            label2.Name = "label2";
            label2.Size = new Size(92, 15);
            label2.TabIndex = 10;
            label2.Text = "MQ Broker port:";
            // 
            // ConectarB
            // 
            ConectarB.Location = new Point(1145, 12);
            ConectarB.Name = "ConectarB";
            ConectarB.Size = new Size(101, 56);
            ConectarB.TabIndex = 11;
            ConectarB.Text = "Conectar";
            ConectarB.UseVisualStyleBackColor = true;
            ConectarB.Click += button5_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1454, 594);
            Controls.Add(GeneraIP);
            Controls.Add(ConectarB);
            Controls.Add(Frame1);
            Controls.Add(BrokerIPBox);
            Controls.Add(label2);
            Controls.Add(AppID);
            Controls.Add(label3);
            Name = "Form1";
            Text = "Form1";
            Frame1.ResumeLayout(false);
            Frame1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox BrokerIPBox;
        private GroupBox Frame1;
        private Button button2;
        private Button Subs;
        private TextBox TextTopic;
        private TextBox AppID;
        private RichTextBox MensajeBox;
        private GroupBox groupBox2;
        private RichTextBox HistorialBox;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label5;
        private Button PublishB;
        private Button button3;
        private Label label6;
        private Button ConectarB;
        private Button GeneraIP;
        private Button button1;
        private Label label1;
        private RichTextBox TemasSubs;
    }
}
