namespace CorpusExplorer.Tool4.KAMOKO
{
  partial class SelectLanguage
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectLanguage));
      this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
      this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
      this.btn_ok = new Telerik.WinControls.UI.RadButton();
      this.btn_abort = new Telerik.WinControls.UI.RadButton();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
      ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_ok)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_abort)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      this.SuspendLayout();
      // 
      // radLabel1
      // 
      this.radLabel1.Location = new System.Drawing.Point(67, 17);
      this.radLabel1.Name = "radLabel1";
      this.radLabel1.Size = new System.Drawing.Size(197, 23);
      this.radLabel1.TabIndex = 1;
      this.radLabel1.Text = "Tagger-Sprache auswählen...";
      // 
      // radLabel2
      // 
      this.radLabel2.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.radLabel2.Location = new System.Drawing.Point(67, 38);
      this.radLabel2.Name = "radLabel2";
      this.radLabel2.Size = new System.Drawing.Size(285, 19);
      this.radLabel2.TabIndex = 2;
      this.radLabel2.Text = "Für die Annotation wird der TreeTagger verwendet.";
      // 
      // btn_ok
      // 
      this.btn_ok.Location = new System.Drawing.Point(12, 115);
      this.btn_ok.Name = "btn_ok";
      this.btn_ok.Size = new System.Drawing.Size(110, 32);
      this.btn_ok.TabIndex = 4;
      this.btn_ok.Text = "Ok";
      this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
      // 
      // btn_abort
      // 
      this.btn_abort.Location = new System.Drawing.Point(366, 115);
      this.btn_abort.Name = "btn_abort";
      this.btn_abort.Size = new System.Drawing.Size(110, 32);
      this.btn_abort.TabIndex = 5;
      this.btn_abort.Text = "Abbrechen";
      this.btn_abort.Click += new System.EventHandler(this.btn_abort_Click);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.project_gear;
      this.pictureBox1.Location = new System.Drawing.Point(12, 12);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(48, 48);
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      // 
      // radDropDownList1
      // 
      this.radDropDownList1.Location = new System.Drawing.Point(12, 67);
      this.radDropDownList1.Name = "radDropDownList1";
      this.radDropDownList1.NullText = "Bitte auswählen...";
      this.radDropDownList1.Size = new System.Drawing.Size(464, 32);
      this.radDropDownList1.TabIndex = 6;
      // 
      // SelectLanguage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(488, 156);
      this.Controls.Add(this.radDropDownList1);
      this.Controls.Add(this.btn_abort);
      this.Controls.Add(this.btn_ok);
      this.Controls.Add(this.radLabel2);
      this.Controls.Add(this.radLabel1);
      this.Controls.Add(this.pictureBox1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SelectLanguage";
      // 
      // 
      // 
      this.RootElement.ApplyShapeToControl = true;
      this.Text = "Tagger-Sprache auswählen...";
      ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_ok)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_abort)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox1;
    private Telerik.WinControls.UI.RadLabel radLabel1;
    private Telerik.WinControls.UI.RadLabel radLabel2;
    private Telerik.WinControls.UI.RadButton btn_ok;
    private Telerik.WinControls.UI.RadButton btn_abort;
    private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
  }
}