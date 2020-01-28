namespace CorpusExplorer.Tool4.KAMOKO.GUI.Forms
{
  partial class ErrorConsole
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
      if (disposing && (this.components != null))
      {
        this.components.Dispose();
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorConsole));
      this.radTreeView1 = new Telerik.WinControls.UI.RadTreeView();
      this.btn_save = new Telerik.WinControls.UI.RadButton();
      this.btn_clear = new Telerik.WinControls.UI.RadButton();
      this.btn_export = new Telerik.WinControls.UI.RadButton();
      this.btn_clean = new Telerik.WinControls.UI.RadButton();
      this.btn_ok = new Telerik.WinControls.UI.RadButton();
      ((System.ComponentModel.ISupportInitialize)(this.radTreeView1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_save)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_clear)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_export)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_clean)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_ok)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      this.SuspendLayout();
      // 
      // radTreeView1
      // 
      this.radTreeView1.ItemHeight = 40;
      this.radTreeView1.Location = new System.Drawing.Point(12, 12);
      this.radTreeView1.Name = "radTreeView1";
      this.radTreeView1.Size = new System.Drawing.Size(652, 340);
      this.radTreeView1.TabIndex = 1;
      this.radTreeView1.Text = "radTreeView1";
      this.radTreeView1.TreeIndent = 40;
      // 
      // btn_save
      // 
      this.btn_save.Location = new System.Drawing.Point(0, 0);
      this.btn_save.Name = "btn_save";
      this.btn_save.Size = new System.Drawing.Size(110, 32);
      this.btn_save.TabIndex = 0;
      // 
      // btn_clear
      // 
      this.btn_clear.Location = new System.Drawing.Point(0, 0);
      this.btn_clear.Name = "btn_clear";
      this.btn_clear.Size = new System.Drawing.Size(110, 24);
      this.btn_clear.TabIndex = 0;
      // 
      // btn_export
      // 
      this.btn_export.Location = new System.Drawing.Point(13, 359);
      this.btn_export.Name = "btn_export";
      this.btn_export.Size = new System.Drawing.Size(180, 32);
      this.btn_export.TabIndex = 2;
      this.btn_export.Text = "Speichern";
      this.btn_export.Click += new System.EventHandler(this.btn_save_Click);
      // 
      // btn_clean
      // 
      this.btn_clean.Location = new System.Drawing.Point(259, 358);
      this.btn_clean.Name = "btn_clean";
      this.btn_clean.Size = new System.Drawing.Size(180, 32);
      this.btn_clean.TabIndex = 3;
      this.btn_clean.Text = "Löschen";
      this.btn_clean.Click += new System.EventHandler(this.btn_clear_Click);
      // 
      // btn_ok
      // 
      this.btn_ok.Location = new System.Drawing.Point(484, 358);
      this.btn_ok.Name = "btn_ok";
      this.btn_ok.Size = new System.Drawing.Size(180, 32);
      this.btn_ok.TabIndex = 3;
      this.btn_ok.Text = "Ok";
      this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
      // 
      // ErrorConsole
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(676, 403);
      this.Controls.Add(this.btn_ok);
      this.Controls.Add(this.btn_clean);
      this.Controls.Add(this.btn_export);
      this.Controls.Add(this.radTreeView1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ErrorConsole";
      // 
      // 
      // 
      this.RootElement.ApplyShapeToControl = true;
      this.Text = "Fehlerbericht";
      ((System.ComponentModel.ISupportInitialize)(this.radTreeView1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_save)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_clear)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_export)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_clean)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_ok)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private Telerik.WinControls.UI.RadTreeView radTreeView1;
    private Telerik.WinControls.UI.RadButton btn_save;
    private Telerik.WinControls.UI.RadButton btn_clear;
    private Telerik.WinControls.UI.RadButton btn_export;
    private Telerik.WinControls.UI.RadButton btn_clean;
    private Telerik.WinControls.UI.RadButton btn_ok;
  }
}