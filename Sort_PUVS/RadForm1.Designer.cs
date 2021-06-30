
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RadForm1));
            this.radOpenFileDialog1 = new Telerik.WinControls.UI.RadOpenFileDialog();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton4 = new Telerik.WinControls.UI.RadButton();
            this.progressBar1 = new Telerik.WinControls.UI.RadProgressBar();
            this.radButton5 = new Telerik.WinControls.UI.RadButton();
            this.radButton6 = new Telerik.WinControls.UI.RadButton();
            this.radRichTextEditor1 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.fluentTheme1 = new Telerik.WinControls.Themes.FluentTheme();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.telerikMetroBlueTheme1 = new Telerik.WinControls.Themes.TelerikMetroBlueTheme();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRichTextEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButton2
            // 
            this.radButton2.Font = new System.Drawing.Font("Maiandra GD", 12F);
            this.radButton2.Location = new System.Drawing.Point(13, 12);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(180, 41);
            this.radButton2.TabIndex = 2;
            this.radButton2.Text = "Открыть папку с файлами";
            this.radButton2.ThemeName = "Fluent";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radButton4
            // 
            this.radButton4.Font = new System.Drawing.Font("Maiandra GD", 12F);
            this.radButton4.Location = new System.Drawing.Point(12, 367);
            this.radButton4.Name = "radButton4";
            this.radButton4.Size = new System.Drawing.Size(180, 41);
            this.radButton4.TabIndex = 4;
            this.radButton4.Text = "Выход";
            this.radButton4.ThemeName = "Fluent";
            this.radButton4.Click += new System.EventHandler(this.radButton4_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(211, 367);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(455, 41);
            this.progressBar1.TabIndex = 7;
            this.progressBar1.ThemeName = "Fluent";
            // 
            // radButton5
            // 
            this.radButton5.Font = new System.Drawing.Font("Maiandra GD", 12F);
            this.radButton5.Location = new System.Drawing.Point(13, 131);
            this.radButton5.Name = "radButton5";
            this.radButton5.Size = new System.Drawing.Size(180, 41);
            this.radButton5.TabIndex = 10;
            this.radButton5.Text = "Начать";
            this.radButton5.ThemeName = "Fluent";
            this.radButton5.Click += new System.EventHandler(this.radButton5_Click);
            // 
            // radButton6
            // 
            this.radButton6.Font = new System.Drawing.Font("Maiandra GD", 12F);
            this.radButton6.Location = new System.Drawing.Point(13, 188);
            this.radButton6.Name = "radButton6";
            this.radButton6.Size = new System.Drawing.Size(180, 41);
            this.radButton6.TabIndex = 11;
            this.radButton6.Text = "Остановить";
            this.radButton6.ThemeName = "Fluent";
            this.radButton6.Click += new System.EventHandler(this.radButton6_Click);
            // 
            // radRichTextEditor1
            // 
            this.radRichTextEditor1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.radRichTextEditor1.Location = new System.Drawing.Point(211, 12);
            this.radRichTextEditor1.Multiline = true;
            this.radRichTextEditor1.Name = "radRichTextEditor1";
            this.radRichTextEditor1.Size = new System.Drawing.Size(455, 338);
            this.radRichTextEditor1.TabIndex = 13;
            this.radRichTextEditor1.ThemeName = "Fluent";
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(13, 308);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(180, 41);
            this.radButton1.TabIndex = 14;
            this.radButton1.Text = "Посмотреть лог-файл";
            this.radButton1.ThemeName = "Fluent";
            // 
            // RadForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 422);
            this.ControlBox = false;
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.radRichTextEditor1);
            this.Controls.Add(this.radButton6);
            this.Controls.Add(this.radButton5);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.radButton4);
            this.Controls.Add(this.radButton2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RadForm1";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "   Программа для формирования и сортировки файлов \"Сорт-ПУВС\"";
            this.ThemeName = "TelerikMetroBlue";
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRichTextEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadOpenFileDialog radOpenFileDialog1;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton4;
        private Telerik.WinControls.UI.RadProgressBar progressBar1;
        private Telerik.WinControls.UI.RadButton radButton5;
        private Telerik.WinControls.UI.RadButton radButton6;
        private Telerik.WinControls.UI.RadTextBoxControl radRichTextEditor1;
        private Telerik.WinControls.Themes.FluentTheme fluentTheme1;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.Themes.TelerikMetroBlueTheme telerikMetroBlueTheme1;
    }
}