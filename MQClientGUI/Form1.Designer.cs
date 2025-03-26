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
            groupBox1 = new GroupBox();
            textBox5 = new TextBox();
            label7 = new Label();
            button4 = new Button();
            button3 = new Button();
            label4 = new Label();
            label3 = new Label();
            groupBox2 = new GroupBox();
            label6 = new Label();
            label5 = new Label();
            richTextBox3 = new RichTextBox();
            richTextBox2 = new RichTextBox();
            richTextBox1 = new RichTextBox();
            button2 = new Button();
            button1 = new Button();
            textBox1 = new TextBox();
            AppID = new TextBox();
            label2 = new Label();
            button5 = new Button();
            button6 = new Button();
            groupBox1.SuspendLayout();
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
            // groupBox1
            // 
            groupBox1.Controls.Add(button6);
            groupBox1.Controls.Add(textBox5);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Controls.Add(richTextBox1);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(AppID);
            groupBox1.Location = new Point(16, 84);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1414, 497);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Siatema";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(615, 25);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(304, 23);
            textBox5.TabIndex = 15;
            textBox5.TextChanged += textBox5_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(574, 28);
            label7.Name = "label7";
            label7.Size = new Size(38, 15);
            label7.TabIndex = 14;
            label7.Text = "Tema:";
            label7.Click += label7_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(255, 255, 128);
            button4.Location = new Point(689, 410);
            button4.Name = "button4";
            button4.Size = new Size(136, 31);
            button4.TabIndex = 13;
            button4.Text = "Publicar";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(255, 192, 128);
            button3.Location = new Point(1139, 410);
            button3.Name = "button3";
            button3.Size = new Size(136, 31);
            button3.TabIndex = 12;
            button3.Text = "Obtener Mensaje ";
            button3.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 85);
            label4.Name = "label4";
            label4.Size = new Size(38, 15);
            label4.TabIndex = 7;
            label4.Text = "Tema:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 42);
            label3.Name = "label3";
            label3.Size = new Size(46, 15);
            label3.TabIndex = 11;
            label3.Text = "App ID:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(richTextBox3);
            groupBox2.Controls.Add(richTextBox2);
            groupBox2.Location = new Point(1011, 52);
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
            label6.Location = new Point(234, 32);
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
            // richTextBox3
            // 
            richTextBox3.Location = new Point(164, 52);
            richTextBox3.Name = "richTextBox3";
            richTextBox3.Size = new Size(202, 269);
            richTextBox3.TabIndex = 1;
            richTextBox3.Text = "";
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(24, 54);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(116, 269);
            richTextBox2.TabIndex = 0;
            richTextBox2.Text = "";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(570, 55);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(371, 349);
            richTextBox1.TabIndex = 8;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(255, 128, 128);
            button2.Location = new Point(297, 112);
            button2.Name = "button2";
            button2.Size = new Size(136, 31);
            button2.TabIndex = 6;
            button2.Text = "Desuscribirse";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(128, 255, 128);
            button1.Location = new Point(71, 112);
            button1.Name = "button1";
            button1.Size = new Size(136, 31);
            button1.TabIndex = 5;
            button1.Text = "Suscribirse";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(65, 82);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(378, 23);
            textBox1.TabIndex = 4;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // AppID
            // 
            AppID.Location = new Point(66, 39);
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
            // button5
            // 
            button5.Location = new Point(513, 22);
            button5.Name = "button5";
            button5.Size = new Size(101, 56);
            button5.TabIndex = 11;
            button5.Text = "Conectar";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(449, 35);
            button6.Name = "button6";
            button6.Size = new Size(86, 28);
            button6.TabIndex = 16;
            button6.Text = "Generar IP";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1454, 594);
            Controls.Add(button5);
            Controls.Add(groupBox1);
            Controls.Add(BrokerIPBox);
            Controls.Add(label2);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox BrokerIPBox;
        private GroupBox groupBox1;
        private Button button2;
        private Button button1;
        private TextBox textBox1;
        private TextBox AppID;
        private RichTextBox richTextBox1;
        private GroupBox groupBox2;
        private RichTextBox richTextBox3;
        private RichTextBox richTextBox2;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label5;
        private Button button4;
        private Button button3;
        private Label label6;
        private Label label7;
        private TextBox textBox5;
        private Button button5;
        private Button button6;
    }
}
