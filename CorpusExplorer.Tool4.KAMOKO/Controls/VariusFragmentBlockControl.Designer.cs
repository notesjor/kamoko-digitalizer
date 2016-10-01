namespace CorpusExplorer.Tool4.KAMOKO.Controls
{
  partial class VariusFragmentBlockControl
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
      this.panel2 = new System.Windows.Forms.Panel();
      this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
      this.radScrollablePanel1 = new Telerik.WinControls.UI.RadScrollablePanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btn_item_add = new Telerik.WinControls.UI.RadDropDownButton();
      this.btn_item_add_const = new Telerik.WinControls.UI.RadMenuItem();
      this.btn_item_add_vars = new Telerik.WinControls.UI.RadMenuItem();
      this.btn_item_remove = new Telerik.WinControls.UI.RadButton();
      this.panel2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).BeginInit();
      this.radScrollablePanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.btn_item_add)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_item_remove)).BeginInit();
      this.SuspendLayout();
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.radLabel1);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel2.Location = new System.Drawing.Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(630, 33);
      this.panel2.TabIndex = 8;
      // 
      // radLabel1
      // 
      this.radLabel1.AutoSize = false;
      this.radLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.radLabel1.Location = new System.Drawing.Point(0, 0);
      this.radLabel1.Name = "radLabel1";
      this.radLabel1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.radLabel1.Size = new System.Drawing.Size(630, 33);
      this.radLabel1.TabIndex = 3;
      this.radLabel1.Text = "Varianten";
      this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // radScrollablePanel1
      // 
      this.radScrollablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.radScrollablePanel1.Location = new System.Drawing.Point(0, 33);
      this.radScrollablePanel1.Name = "radScrollablePanel1";
      // 
      // radScrollablePanel1.PanelContainer
      // 
      this.radScrollablePanel1.PanelContainer.Size = new System.Drawing.Size(628, 490);
      this.radScrollablePanel1.Size = new System.Drawing.Size(630, 492);
      this.radScrollablePanel1.TabIndex = 9;
      this.radScrollablePanel1.Text = "radScrollablePanel1";
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.Honeydew;
      this.panel1.Controls.Add(this.btn_item_add);
      this.panel1.Controls.Add(this.btn_item_remove);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
      this.panel1.Location = new System.Drawing.Point(630, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(33, 525);
      this.panel1.TabIndex = 7;
      // 
      // btn_item_add
      // 
      this.btn_item_add.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.btn_item_add.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.add_button;
      this.btn_item_add.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
      this.btn_item_add.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btn_item_add_const,
            this.btn_item_add_vars});
      this.btn_item_add.Location = new System.Drawing.Point(0, 492);
      this.btn_item_add.Name = "btn_item_add";
      this.btn_item_add.ShowArrow = false;
      this.btn_item_add.Size = new System.Drawing.Size(33, 33);
      this.btn_item_add.TabIndex = 3;
      // 
      // btn_item_add_const
      // 
      this.btn_item_add_const.AccessibleDescription = "Konstanten Text einfügen";
      this.btn_item_add_const.AccessibleName = "Konstanten Text einfügen";
      this.btn_item_add_const.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.formating_highlight;
      this.btn_item_add_const.Name = "btn_item_add_const";
      this.btn_item_add_const.Text = "Konstanten Text einfügen";
      this.btn_item_add_const.Click += new System.EventHandler(this.btn_item_add_const_Click);
      // 
      // btn_item_add_vars
      // 
      this.btn_item_add_vars.AccessibleDescription = "Varianten einfügen";
      this.btn_item_add_vars.AccessibleName = "Varianten einfügen";
      this.btn_item_add_vars.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.formating_highlight_date;
      this.btn_item_add_vars.Name = "btn_item_add_vars";
      this.btn_item_add_vars.Text = "Varianten einfügen";
      this.btn_item_add_vars.Click += new System.EventHandler(this.btn_item_add_vars_Click);
      // 
      // btn_item_remove
      // 
      this.btn_item_remove.Dock = System.Windows.Forms.DockStyle.Top;
      this.btn_item_remove.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.delete_button_error;
      this.btn_item_remove.Location = new System.Drawing.Point(0, 0);
      this.btn_item_remove.Name = "btn_item_remove";
      this.btn_item_remove.Size = new System.Drawing.Size(33, 33);
      this.btn_item_remove.TabIndex = 0;
      this.btn_item_remove.Click += new System.EventHandler(this.btn_item_remove_Click);
      // 
      // VariusFragmentBlockControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.radScrollablePanel1);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Name = "VariusFragmentBlockControl";
      this.Size = new System.Drawing.Size(663, 525);
      this.panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).EndInit();
      this.radScrollablePanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.btn_item_add)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_item_remove)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel2;
    private Telerik.WinControls.UI.RadLabel radLabel1;
    private Telerik.WinControls.UI.RadScrollablePanel radScrollablePanel1;
    private Telerik.WinControls.UI.RadButton btn_item_remove;
    private System.Windows.Forms.Panel panel1;
    private Telerik.WinControls.UI.RadDropDownButton btn_item_add;
    private Telerik.WinControls.UI.RadMenuItem btn_item_add_const;
    private Telerik.WinControls.UI.RadMenuItem btn_item_add_vars;
  }
}
