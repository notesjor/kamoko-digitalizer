namespace CorpusExplorer.Tool4.KAMOKO.GUI.Controls
{
  partial class VoteBarControl
  {
    /// <summary> 
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Komponenten-Designer generierter Code

    /// <summary> 
    /// Erforderliche Methode für die Designerunterstützung. 
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.radScrollablePanel1 = new Telerik.WinControls.UI.RadScrollablePanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.btn_item_add = new Telerik.WinControls.UI.RadButton();
      ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).BeginInit();
      this.radScrollablePanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_item_add)).BeginInit();
      this.SuspendLayout();
      // 
      // radScrollablePanel1
      // 
      this.radScrollablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.radScrollablePanel1.Location = new System.Drawing.Point(61, 0);
      this.radScrollablePanel1.Name = "radScrollablePanel1";
      this.radScrollablePanel1.Padding = new System.Windows.Forms.Padding(0);
      // 
      // radScrollablePanel1.PanelContainer
      // 
      this.radScrollablePanel1.PanelContainer.Location = new System.Drawing.Point(0, 0);
      this.radScrollablePanel1.PanelContainer.MinimumSize = new System.Drawing.Size(0, 66);
      this.radScrollablePanel1.PanelContainer.Size = new System.Drawing.Size(431, 66);
      this.radScrollablePanel1.Size = new System.Drawing.Size(431, 66);
      this.radScrollablePanel1.TabIndex = 2;
      ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radScrollablePanel1.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.pictureBox1);
      this.panel1.Controls.Add(this.btn_item_add);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(61, 66);
      this.panel1.TabIndex = 3;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pictureBox1.Image = global::CorpusExplorer.Tool4.KAMOKO.GUI.Properties.Resources.user_unknown;
      this.pictureBox1.Location = new System.Drawing.Point(0, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(61, 34);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      // 
      // btn_item_add
      // 
      this.btn_item_add.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.btn_item_add.Image = global::CorpusExplorer.Tool4.KAMOKO.GUI.Properties.Resources.add_button;
      this.btn_item_add.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
      this.btn_item_add.Location = new System.Drawing.Point(0, 34);
      this.btn_item_add.Name = "btn_item_add";
      this.btn_item_add.Size = new System.Drawing.Size(61, 32);
      this.btn_item_add.TabIndex = 1;
      this.btn_item_add.Click += new System.EventHandler(this.btn_item_add_Click);
      // 
      // VoteBarControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.radScrollablePanel1);
      this.Controls.Add(this.panel1);
      this.Name = "VoteBarControl";
      this.Size = new System.Drawing.Size(492, 66);
      ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).EndInit();
      this.radScrollablePanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_item_add)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox1;
    private Telerik.WinControls.UI.RadScrollablePanel radScrollablePanel1;
    private System.Windows.Forms.Panel panel1;
    private Telerik.WinControls.UI.RadButton btn_item_add;
  }
}
