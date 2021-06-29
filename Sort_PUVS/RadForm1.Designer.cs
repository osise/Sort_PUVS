
namespace Sort_PUVS
{
    partial class RadForm1
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
            this.radOpenFileDialog1 = new Telerik.WinControls.UI.RadOpenFileDialog();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton3 = new Telerik.WinControls.UI.RadButton();
            this.radButton4 = new Telerik.WinControls.UI.RadButton();
            this.progressBar1 = new Telerik.WinControls.UI.RadProgressBar();
            this.radButton5 = new Telerik.WinControls.UI.RadButton();
            this.radButton6 = new Telerik.WinControls.UI.RadButton();
            this.tbProgress = new System.Windows.Forms.TextBox();
            this.radRichTextEditor1 = new Telerik.WinControls.UI.RadTextBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRichTextEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(13, 13);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(180, 41);
            this.radButton1.TabIndex = 1;
            this.radButton1.Text = "Загрузить";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(12, 60);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(180, 41);
            this.radButton2.TabIndex = 2;
            this.radButton2.Text = "Открыть папку с файлами";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radButton3
            // 
            this.radButton3.Location = new System.Drawing.Point(13, 344);
            this.radButton3.Name = "radButton3";
            this.radButton3.Size = new System.Drawing.Size(180, 41);
            this.radButton3.TabIndex = 3;
            this.radButton3.Text = "Посмотреть логи";
            this.radButton3.Click += new System.EventHandler(this.radButton3_Click);
            // 
            // radButton4
            // 
            this.radButton4.Location = new System.Drawing.Point(13, 407);
            this.radButton4.Name = "radButton4";
            this.radButton4.Size = new System.Drawing.Size(180, 41);
            this.radButton4.TabIndex = 4;
            this.radButton4.Text = "Выход";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(214, 361);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(493, 47);
            this.progressBar1.TabIndex = 7;
            this.progressBar1.Text = "radProgressBar1";
            // 
            // radButton5
            // 
            this.radButton5.Location = new System.Drawing.Point(12, 239);
            this.radButton5.Name = "radButton5";
            this.radButton5.Size = new System.Drawing.Size(81, 41);
            this.radButton5.TabIndex = 10;
            this.radButton5.Text = "Start";
            this.radButton5.Click += new System.EventHandler(this.radButton5_Click);
            // 
            // radButton6
            // 
            this.radButton6.Location = new System.Drawing.Point(99, 239);
            this.radButton6.Name = "radButton6";
            this.radButton6.Size = new System.Drawing.Size(93, 41);
            this.radButton6.TabIndex = 11;
            this.radButton6.Text = "stop";
            this.radButton6.Click += new System.EventHandler(this.radButton6_Click);
            // 
            // tbProgress
            // 
            this.tbProgress.Location = new System.Drawing.Point(51, 316);
            this.tbProgress.Name = "tbProgress";
            this.tbProgress.Size = new System.Drawing.Size(100, 20);
            this.tbProgress.TabIndex = 12;
            // 
            // radRichTextEditor1
            // 
            this.radRichTextEditor1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.radRichTextEditor1.Location = new System.Drawing.Point(214, 12);
            this.radRichTextEditor1.Multiline = true;
            this.radRichTextEditor1.Name = "radRichTextEditor1";
            this.radRichTextEditor1.Size = new System.Drawing.Size(493, 338);
            this.radRichTextEditor1.TabIndex = 13;
            // 
            // RadForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 460);
            this.Controls.Add(this.radRichTextEditor1);
            this.Controls.Add(this.tbProgress);
            this.Controls.Add(this.radButton6);
            this.Controls.Add(this.radButton5);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.radButton4);
            this.Controls.Add(this.radButton3);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton1);
            this.Name = "RadForm1";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "richTextEditorRibbonBar1";
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRichTextEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadOpenFileDialog radOpenFileDialog1;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton3;
        private Telerik.WinControls.UI.RadButton radButton4;
        private Telerik.WinControls.UI.RadProgressBar progressBar1;
        private Telerik.WinControls.UI.RadButton radButton5;
        private Telerik.WinControls.UI.RadButton radButton6;
        private System.Windows.Forms.TextBox tbProgress;
        private Telerik.WinControls.UI.RadTextBoxControl radRichTextEditor1;
    }
}