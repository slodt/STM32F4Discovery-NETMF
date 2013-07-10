using TestEmulator.Controls;

namespace TestEmulator
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.ledControl5 = new TestEmulator.Controls.LedControl();
            this.ledControl4 = new TestEmulator.Controls.LedControl();
            this.ledControl2 = new TestEmulator.Controls.LedControl();
            this.ledControl1 = new TestEmulator.Controls.LedControl();
            this.buttonControl1 = new TestEmulator.Controls.ButtonControl();
            this.ledControl3 = new TestEmulator.Controls.LedControl();
            this.buttonControl2 = new TestEmulator.Controls.ButtonControl();
            this.serialPortComponent1 = new TestEmulator.Controls.SerialPortComponent(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // ledControl5
            // 
            this.ledControl5.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ledControl5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ledControl5.Location = new System.Drawing.Point(132, 12);
            this.ledControl5.Name = "ledControl5";
            this.ledControl5.Pin = 7;
            this.ledControl5.Size = new System.Drawing.Size(24, 24);
            this.ledControl5.TabIndex = 6;
            // 
            // ledControl4
            // 
            this.ledControl4.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ledControl4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ledControl4.Location = new System.Drawing.Point(102, 12);
            this.ledControl4.Name = "ledControl4";
            this.ledControl4.Pin = 6;
            this.ledControl4.Size = new System.Drawing.Size(24, 24);
            this.ledControl4.TabIndex = 5;
            // 
            // ledControl2
            // 
            this.ledControl2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ledControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ledControl2.Location = new System.Drawing.Point(72, 12);
            this.ledControl2.Name = "ledControl2";
            this.ledControl2.Pin = 5;
            this.ledControl2.Size = new System.Drawing.Size(24, 24);
            this.ledControl2.TabIndex = 4;
            // 
            // ledControl1
            // 
            this.ledControl1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ledControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ledControl1.Location = new System.Drawing.Point(42, 12);
            this.ledControl1.Name = "ledControl1";
            this.ledControl1.Pin = 4;
            this.ledControl1.Size = new System.Drawing.Size(24, 24);
            this.ledControl1.TabIndex = 3;
            // 
            // buttonControl1
            // 
            this.buttonControl1.Location = new System.Drawing.Point(186, 12);
            this.buttonControl1.Name = "buttonControl1";
            this.buttonControl1.Pin = 18;
            this.buttonControl1.Size = new System.Drawing.Size(80, 29);
            this.buttonControl1.TabIndex = 7;
            // 
            // ledControl3
            // 
            this.ledControl3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ledControl3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ledControl3.Location = new System.Drawing.Point(12, 12);
            this.ledControl3.Name = "ledControl3";
            this.ledControl3.Pin = 3;
            this.ledControl3.Size = new System.Drawing.Size(24, 24);
            this.ledControl3.TabIndex = 8;
            // 
            // buttonControl2
            // 
            this.buttonControl2.Location = new System.Drawing.Point(272, 12);
            this.buttonControl2.Name = "buttonControl2";
            this.buttonControl2.Pin = 19;
            this.buttonControl2.Size = new System.Drawing.Size(80, 29);
            this.buttonControl2.TabIndex = 9;
            // 
            // serialPortComponent1
            // 
            this.serialPortComponent1.ComPortHandle = "Usart1";
            this.serialPortComponent1.OnWrite += new System.EventHandler<TestEmulator.Controls.SerialPortComponent.SerialDataEventArgs>(this.serialPortComponent1_OnWrite);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 52);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(335, 114);
            this.richTextBox1.TabIndex = 10;
            this.richTextBox1.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 178);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.buttonControl2);
            this.Controls.Add(this.ledControl3);
            this.Controls.Add(this.buttonControl1);
            this.Controls.Add(this.ledControl5);
            this.Controls.Add(this.ledControl4);
            this.Controls.Add(this.ledControl2);
            this.Controls.Add(this.ledControl1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private LedControl ledControl1;
        private LedControl ledControl2;
        private LedControl ledControl4;
        private LedControl ledControl5;
        private ButtonControl buttonControl1;
        private LedControl ledControl3;
        private ButtonControl buttonControl2;
        private SerialPortComponent serialPortComponent1;
        private System.Windows.Forms.RichTextBox richTextBox1;



    }
}

