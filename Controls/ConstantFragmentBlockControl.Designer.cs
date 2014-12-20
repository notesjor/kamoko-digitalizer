﻿namespace CorpusExplorer.Tool4.KAMOKO.Controls
{
  partial class ConstantFragmentBlockControl
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
      this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
      this.radTextBox1 = new Telerik.WinControls.UI.RadTextBox();
      this.radCollapsiblePanel1 = new Telerik.WinControls.UI.RadCollapsiblePanel();
      this.voteBarControl1 = new CorpusExplorer.Tool4.KAMOKO.Controls.VoteBarControl();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btn_item_add = new Telerik.WinControls.UI.RadDropDownButton();
      this.btn_item_add_vars = new Telerik.WinControls.UI.RadMenuItem();
      this.btn_item_add_const = new Telerik.WinControls.UI.RadMenuItem();
      this.btn_item_remove = new Telerik.WinControls.UI.RadButton();
      ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
      this.radPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).BeginInit();
      this.radCollapsiblePanel1.PanelContainer.SuspendLayout();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.btn_item_add)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_item_remove)).BeginInit();
      this.SuspendLayout();
      // 
      // radPanel1
      // 
      this.radPanel1.Controls.Add(this.radTextBox1);
      this.radPanel1.Controls.Add(this.radCollapsiblePanel1);
      this.radPanel1.Controls.Add(this.panel1);
      this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.radPanel1.Location = new System.Drawing.Point(0, 0);
      this.radPanel1.Name = "radPanel1";
      this.radPanel1.Size = new System.Drawing.Size(531, 194);
      this.radPanel1.TabIndex = 0;
      // 
      // radTextBox1
      // 
      this.radTextBox1.AutoSize = false;
      this.radTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.radTextBox1.Location = new System.Drawing.Point(0, 0);
      this.radTextBox1.Multiline = true;
      this.radTextBox1.Name = "radTextBox1";
      this.radTextBox1.Size = new System.Drawing.Size(498, 32);
      this.radTextBox1.TabIndex = 8;
      // 
      // radCollapsiblePanel1
      // 
      this.radCollapsiblePanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.radCollapsiblePanel1.ExpandDirection = Telerik.WinControls.UI.RadDirection.Up;
      this.radCollapsiblePanel1.HeaderText = "Sprecherbewertung";
      this.radCollapsiblePanel1.Location = new System.Drawing.Point(0, 32);
      this.radCollapsiblePanel1.Name = "radCollapsiblePanel1";
      this.radCollapsiblePanel1.OwnerBoundsCache = new System.Drawing.Rectangle(0, 46, 498, 200);
      // 
      // radCollapsiblePanel1.PanelContainer
      // 
      this.radCollapsiblePanel1.PanelContainer.Controls.Add(this.voteBarControl1);
      this.radCollapsiblePanel1.PanelContainer.Size = new System.Drawing.Size(496, 126);
      this.radCollapsiblePanel1.Size = new System.Drawing.Size(498, 162);
      this.radCollapsiblePanel1.TabIndex = 7;
      this.radCollapsiblePanel1.Text = "radCollapsiblePanel1";
      // 
      // voteBarControl1
      // 
      this.voteBarControl1.BackColor = System.Drawing.Color.White;
      this.voteBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.voteBarControl1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.voteBarControl1.Location = new System.Drawing.Point(0, 0);
      this.voteBarControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.voteBarControl1.Name = "voteBarControl1";
      this.voteBarControl1.Size = new System.Drawing.Size(496, 126);
      this.voteBarControl1.TabIndex = 0;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.btn_item_add);
      this.panel1.Controls.Add(this.btn_item_remove);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
      this.panel1.Location = new System.Drawing.Point(498, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(33, 194);
      this.panel1.TabIndex = 6;
      // 
      // btn_item_add
      // 
      this.btn_item_add.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.btn_item_add.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.add_button;
      this.btn_item_add.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
      this.btn_item_add.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btn_item_add_vars,
            this.btn_item_add_const});
      this.btn_item_add.Location = new System.Drawing.Point(0, 161);
      this.btn_item_add.Name = "btn_item_add";
      this.btn_item_add.ShowArrow = false;
      this.btn_item_add.Size = new System.Drawing.Size(33, 33);
      this.btn_item_add.TabIndex = 2;
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
      // btn_item_add_const
      // 
      this.btn_item_add_const.AccessibleDescription = "Konstanten Text hinzufügen";
      this.btn_item_add_const.AccessibleName = "Konstanten Text hinzufügen";
      this.btn_item_add_const.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.formating_highlight;
      this.btn_item_add_const.Name = "btn_item_add_const";
      this.btn_item_add_const.Text = "Konstanten Text hinzufügen";
      this.btn_item_add_const.Click += new System.EventHandler(this.btn_item_add_const_Click);
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
      // ConstantFragmentBlockControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.radPanel1);
      this.Name = "ConstantFragmentBlockControl";
      this.Size = new System.Drawing.Size(531, 194);
      ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
      this.radPanel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).EndInit();
      this.radCollapsiblePanel1.PanelContainer.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).EndInit();
      this.panel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.btn_item_add)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.btn_item_remove)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private Telerik.WinControls.UI.RadPanel radPanel1;
    private System.Windows.Forms.Panel panel1;
    private Telerik.WinControls.UI.RadButton btn_item_remove;
    private Telerik.WinControls.UI.RadCollapsiblePanel radCollapsiblePanel1;
    private VoteBarControl voteBarControl1;
    private Telerik.WinControls.UI.RadTextBox radTextBox1;
    private Telerik.WinControls.UI.RadDropDownButton btn_item_add;
    private Telerik.WinControls.UI.RadMenuItem btn_item_add_const;
    private Telerik.WinControls.UI.RadMenuItem btn_item_add_vars;
  }
}
