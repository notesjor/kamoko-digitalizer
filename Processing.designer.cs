namespace CorpusExplorer.Terminal.WinForm.Forms.Splash
{
  partial class Processing
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Processing));
      this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
      this.radWaitingBar1 = new Telerik.WinControls.UI.RadWaitingBar();
      this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
      ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      this.SuspendLayout();
      // 
      // radLabel2
      // 
      this.radLabel2.AutoSize = false;
      this.radLabel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.radLabel2.Font = new System.Drawing.Font("Segoe UI", 10F);
      this.radLabel2.Location = new System.Drawing.Point(5, 56);
      this.radLabel2.Name = "radLabel2";
      this.radLabel2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
      this.radLabel2.Size = new System.Drawing.Size(224, 61);
      this.radLabel2.TabIndex = 8;
      this.radLabel2.Text = "Lehnen Sie sich zurück und enspannen Sie sich, während CorpusExplorer für Sie rec" +
    "hnet.";
      this.radLabel2.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
      this.radLabel2.ThemeName = "TelerikMetro";
      // 
      // radWaitingBar1
      // 
      this.radWaitingBar1.Dock = System.Windows.Forms.DockStyle.Top;
      this.radWaitingBar1.Location = new System.Drawing.Point(5, 26);
      this.radWaitingBar1.Name = "radWaitingBar1";
      this.radWaitingBar1.Size = new System.Drawing.Size(224, 30);
      this.radWaitingBar1.TabIndex = 6;
      this.radWaitingBar1.Text = "radWaitingBar1";
      this.radWaitingBar1.ThemeName = "TelerikMetro";
      this.radWaitingBar1.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.Dash;
      // 
      // radLabel1
      // 
      this.radLabel1.AutoSize = false;
      this.radLabel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 12F);
      this.radLabel1.Location = new System.Drawing.Point(5, 1);
      this.radLabel1.Name = "radLabel1";
      this.radLabel1.Size = new System.Drawing.Size(224, 25);
      this.radLabel1.TabIndex = 7;
      this.radLabel1.Text = "Gut Ding will Weile haben...";
      this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
      this.radLabel1.ThemeName = "TelerikMetro";
      // 
      // Processing
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(234, 135);
      this.ControlBox = false;
      this.Controls.Add(this.radLabel2);
      this.Controls.Add(this.radWaitingBar1);
      this.Controls.Add(this.radLabel1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Processing";
      this.Padding = new System.Windows.Forms.Padding(5, 1, 5, 5);
      // 
      // 
      // 
      this.RootElement.ApplyShapeToControl = true;
      this.Text = "";
      ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private Telerik.WinControls.UI.RadLabel radLabel2;
    private Telerik.WinControls.UI.RadWaitingBar radWaitingBar1;
    private Telerik.WinControls.UI.RadLabel radLabel1;


  }
}