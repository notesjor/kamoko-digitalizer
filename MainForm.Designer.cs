namespace CorpusExplorer.Tool4.KAMOKO
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
      this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
      this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
      this.btn_course_new = new Telerik.WinControls.UI.CommandBarButton();
      this.btn_course_open = new Telerik.WinControls.UI.CommandBarButton();
      this.btn_course_save = new Telerik.WinControls.UI.CommandBarButton();
      this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
      this.btn_page_add = new Telerik.WinControls.UI.CommandBarButton();
      this.commandBarLabel1 = new Telerik.WinControls.UI.CommandBarLabel();
      this.txt_index_document = new Telerik.WinControls.UI.CommandBarTextBox();
      this.btn_document_prev = new Telerik.WinControls.UI.CommandBarButton();
      this.lbl_page_view = new Telerik.WinControls.UI.CommandBarLabel();
      this.btn_document_next = new Telerik.WinControls.UI.CommandBarButton();
      this.commandBarLabel3 = new Telerik.WinControls.UI.CommandBarLabel();
      this.btn_sentence_add = new Telerik.WinControls.UI.CommandBarButton();
      this.commandBarLabel2 = new Telerik.WinControls.UI.CommandBarLabel();
      this.txt_index_sentence = new Telerik.WinControls.UI.CommandBarTextBox();
      this.btn_sentence_prev = new Telerik.WinControls.UI.CommandBarButton();
      this.lbl_sentence_view = new Telerik.WinControls.UI.CommandBarLabel();
      this.btn_sentence_next = new Telerik.WinControls.UI.CommandBarButton();
      this.commandBarSeparator2 = new Telerik.WinControls.UI.CommandBarSeparator();
      this.btn_export = new Telerik.WinControls.UI.CommandBarButton();
      this.radScrollablePanel1 = new Telerik.WinControls.UI.RadScrollablePanel();
      ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).BeginInit();
      this.radScrollablePanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      this.SuspendLayout();
      // 
      // radCommandBar1
      // 
      this.radCommandBar1.Dock = System.Windows.Forms.DockStyle.Top;
      this.radCommandBar1.Location = new System.Drawing.Point(0, 0);
      this.radCommandBar1.Name = "radCommandBar1";
      this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
      this.radCommandBar1.Size = new System.Drawing.Size(877, 69);
      this.radCommandBar1.TabIndex = 0;
      this.radCommandBar1.Text = "radCommandBar1";
      // 
      // commandBarRowElement1
      // 
      this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
      this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement1});
      // 
      // commandBarStripElement1
      // 
      this.commandBarStripElement1.DisplayName = "commandBarStripElement1";
      this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.btn_course_new,
            this.btn_course_open,
            this.btn_course_save,
            this.commandBarSeparator1,
            this.btn_page_add,
            this.commandBarLabel1,
            this.txt_index_document,
            this.btn_document_prev,
            this.lbl_page_view,
            this.btn_document_next,
            this.commandBarLabel3,
            this.btn_sentence_add,
            this.commandBarLabel2,
            this.txt_index_sentence,
            this.btn_sentence_prev,
            this.lbl_sentence_view,
            this.btn_sentence_next,
            this.commandBarSeparator2,
            this.btn_export});
      this.commandBarStripElement1.Name = "commandBarStripElement1";
      // 
      // btn_course_new
      // 
      this.btn_course_new.AccessibleDescription = "Neuer Kurs";
      this.btn_course_new.AccessibleName = "Neuer Kurs";
      this.btn_course_new.DisplayName = "commandBarButton1";
      this.btn_course_new.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.document;
      this.btn_course_new.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_course_new.Name = "btn_course_new";
      this.btn_course_new.Text = "Neuer Kurs";
      this.btn_course_new.Click += new System.EventHandler(this.btn_course_new_Click);
      // 
      // btn_course_open
      // 
      this.btn_course_open.AccessibleDescription = "Kurs laden";
      this.btn_course_open.AccessibleName = "Kurs laden";
      this.btn_course_open.DisplayName = "commandBarButton2";
      this.btn_course_open.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.folder;
      this.btn_course_open.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_course_open.Name = "btn_course_open";
      this.btn_course_open.Text = "Kurs laden";
      this.btn_course_open.Click += new System.EventHandler(this.btn_course_open_Click);
      // 
      // btn_course_save
      // 
      this.btn_course_save.AccessibleDescription = "Kurs speichern";
      this.btn_course_save.AccessibleName = "Kurs speichern";
      this.btn_course_save.DisplayName = "commandBarButton3";
      this.btn_course_save.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.save;
      this.btn_course_save.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_course_save.Name = "btn_course_save";
      this.btn_course_save.Text = "Kurs speichern";
      this.btn_course_save.Click += new System.EventHandler(this.btn_course_save_Click);
      // 
      // commandBarSeparator1
      // 
      this.commandBarSeparator1.AccessibleDescription = "commandBarSeparator1";
      this.commandBarSeparator1.AccessibleName = "commandBarSeparator1";
      this.commandBarSeparator1.DisplayName = "commandBarSeparator1";
      this.commandBarSeparator1.Name = "commandBarSeparator1";
      this.commandBarSeparator1.VisibleInOverflowMenu = false;
      // 
      // btn_page_add
      // 
      this.btn_page_add.AccessibleDescription = "Blatt hinzufügen";
      this.btn_page_add.AccessibleName = "Blatt hinzufügen";
      this.btn_page_add.DisplayName = "commandBarButton1";
      this.btn_page_add.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.add_button_blue;
      this.btn_page_add.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_page_add.Name = "btn_page_add";
      this.btn_page_add.Text = "Blatt hinzufügen";
      this.btn_page_add.Click += new System.EventHandler(this.btn_page_add_Click);
      // 
      // commandBarLabel1
      // 
      this.commandBarLabel1.AccessibleDescription = "Blatt (Nr.):";
      this.commandBarLabel1.AccessibleName = "Blatt (Nr.):";
      this.commandBarLabel1.DisplayName = "commandBarLabel1";
      this.commandBarLabel1.Name = "commandBarLabel1";
      this.commandBarLabel1.Text = "Blatt (Nr.):";
      // 
      // txt_index_document
      // 
      this.txt_index_document.AccessibleDescription = "commandBarTextBox1";
      this.txt_index_document.AccessibleName = "commandBarTextBox1";
      this.txt_index_document.DisplayName = "commandBarTextBox1";
      this.txt_index_document.Name = "txt_index_document";
      this.txt_index_document.Text = "";
      this.txt_index_document.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // btn_document_prev
      // 
      this.btn_document_prev.AccessibleDescription = "Dokument zurück";
      this.btn_document_prev.AccessibleName = "Dokument zurück";
      this.btn_document_prev.DisplayName = "commandBarButton1";
      this.btn_document_prev.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.button_blue_arrow_left;
      this.btn_document_prev.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_document_prev.Name = "btn_document_prev";
      this.btn_document_prev.Text = "Dokument zurück";
      this.btn_document_prev.Click += new System.EventHandler(this.btn_document_prev_Click);
      // 
      // lbl_page_view
      // 
      this.lbl_page_view.AccessibleDescription = "0 / 0";
      this.lbl_page_view.AccessibleName = "0 / 0";
      this.lbl_page_view.DisplayName = "commandBarLabel4";
      this.lbl_page_view.Name = "lbl_page_view";
      this.lbl_page_view.Text = "0 / 0";
      // 
      // btn_document_next
      // 
      this.btn_document_next.AccessibleDescription = "Dokument vor";
      this.btn_document_next.AccessibleName = "Dokument vor";
      this.btn_document_next.DisplayName = "commandBarButton2";
      this.btn_document_next.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.button_blue_arrow_right;
      this.btn_document_next.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_document_next.Name = "btn_document_next";
      this.btn_document_next.Text = "Dokument vor";
      this.btn_document_next.Click += new System.EventHandler(this.btn_document_next_Click);
      // 
      // commandBarLabel3
      // 
      this.commandBarLabel3.AccessibleDescription = " / ";
      this.commandBarLabel3.AccessibleName = " / ";
      this.commandBarLabel3.DisplayName = "commandBarLabel3";
      this.commandBarLabel3.Name = "commandBarLabel3";
      this.commandBarLabel3.Text = "/";
      // 
      // btn_sentence_add
      // 
      this.btn_sentence_add.AccessibleDescription = "Satz hinzufügen";
      this.btn_sentence_add.AccessibleName = "Satz hinzufügen";
      this.btn_sentence_add.DisplayName = "commandBarButton1";
      this.btn_sentence_add.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.add_button;
      this.btn_sentence_add.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_sentence_add.Name = "btn_sentence_add";
      this.btn_sentence_add.Text = "Satz hinzufügen";
      this.btn_sentence_add.Click += new System.EventHandler(this.btn_sentence_add_Click);
      // 
      // commandBarLabel2
      // 
      this.commandBarLabel2.AccessibleDescription = "/ Satz (Nr.):";
      this.commandBarLabel2.AccessibleName = "/ Satz (Nr.):";
      this.commandBarLabel2.DisplayName = "commandBarLabel2";
      this.commandBarLabel2.Name = "commandBarLabel2";
      this.commandBarLabel2.Text = "Satz (Nr.):";
      // 
      // txt_index_sentence
      // 
      this.txt_index_sentence.AccessibleDescription = "commandBarTextBox2";
      this.txt_index_sentence.AccessibleName = "commandBarTextBox2";
      this.txt_index_sentence.DisplayName = "commandBarTextBox2";
      this.txt_index_sentence.Name = "txt_index_sentence";
      this.txt_index_sentence.Text = "";
      // 
      // btn_sentence_prev
      // 
      this.btn_sentence_prev.AccessibleDescription = "Satz zurück";
      this.btn_sentence_prev.AccessibleName = "Satz zurück";
      this.btn_sentence_prev.DisplayName = "commandBarButton4";
      this.btn_sentence_prev.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.button_green_arrow_left;
      this.btn_sentence_prev.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_sentence_prev.Name = "btn_sentence_prev";
      this.btn_sentence_prev.Text = "Satz zurück";
      this.btn_sentence_prev.Click += new System.EventHandler(this.btn_sentence_prev_Click);
      // 
      // lbl_sentence_view
      // 
      this.lbl_sentence_view.AccessibleDescription = "0 / 0";
      this.lbl_sentence_view.AccessibleName = "0 / 0";
      this.lbl_sentence_view.DisplayName = "commandBarLabel5";
      this.lbl_sentence_view.Name = "lbl_sentence_view";
      this.lbl_sentence_view.Text = "0 / 0";
      // 
      // btn_sentence_next
      // 
      this.btn_sentence_next.AccessibleDescription = "Satz vor";
      this.btn_sentence_next.AccessibleName = "Satz vor";
      this.btn_sentence_next.DisplayName = "commandBarButton5";
      this.btn_sentence_next.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.button_green_arrow_right;
      this.btn_sentence_next.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_sentence_next.Name = "btn_sentence_next";
      this.btn_sentence_next.Text = "Satz vor";
      this.btn_sentence_next.Click += new System.EventHandler(this.btn_sentence_next_Click);
      // 
      // commandBarSeparator2
      // 
      this.commandBarSeparator2.AccessibleDescription = "commandBarSeparator2";
      this.commandBarSeparator2.AccessibleName = "commandBarSeparator2";
      this.commandBarSeparator2.DisplayName = "commandBarSeparator2";
      this.commandBarSeparator2.Name = "commandBarSeparator2";
      this.commandBarSeparator2.VisibleInOverflowMenu = false;
      // 
      // btn_export
      // 
      this.btn_export.AccessibleDescription = "Exportieren";
      this.btn_export.AccessibleName = "Exportieren";
      this.btn_export.DisplayName = "commandBarButton1";
      this.btn_export.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.export_to_database;
      this.btn_export.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_export.Name = "btn_export";
      this.btn_export.Text = "Exportieren";
      this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
      // 
      // radScrollablePanel1
      // 
      this.radScrollablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.radScrollablePanel1.Location = new System.Drawing.Point(0, 69);
      this.radScrollablePanel1.Name = "radScrollablePanel1";
      // 
      // radScrollablePanel1.PanelContainer
      // 
      this.radScrollablePanel1.PanelContainer.Size = new System.Drawing.Size(875, 382);
      this.radScrollablePanel1.Size = new System.Drawing.Size(877, 384);
      this.radScrollablePanel1.TabIndex = 1;
      this.radScrollablePanel1.Text = "radScrollablePanel1";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(877, 453);
      this.Controls.Add(this.radScrollablePanel1);
      this.Controls.Add(this.radCommandBar1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(875, 502);
      this.Name = "MainForm";
      // 
      // 
      // 
      this.RootElement.ApplyShapeToControl = true;
      this.RootElement.MaxSize = new System.Drawing.Size(0, 0);
      this.Text = "KAMOKO - Digitalizer";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).EndInit();
      this.radScrollablePanel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Telerik.WinControls.UI.RadCommandBar radCommandBar1;
    private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
    private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
    private Telerik.WinControls.UI.CommandBarButton btn_course_new;
    private Telerik.WinControls.UI.CommandBarButton btn_course_open;
    private Telerik.WinControls.UI.CommandBarButton btn_course_save;
    private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
    private Telerik.WinControls.UI.CommandBarLabel commandBarLabel1;
    private Telerik.WinControls.UI.CommandBarTextBox txt_index_document;
    private Telerik.WinControls.UI.CommandBarButton btn_document_prev;
    private Telerik.WinControls.UI.CommandBarButton btn_document_next;
    private Telerik.WinControls.UI.CommandBarLabel commandBarLabel2;
    private Telerik.WinControls.UI.CommandBarTextBox txt_index_sentence;
    private Telerik.WinControls.UI.CommandBarButton btn_sentence_prev;
    private Telerik.WinControls.UI.CommandBarButton btn_sentence_next;
    private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator2;
    private Telerik.WinControls.UI.CommandBarButton btn_export;
    private Telerik.WinControls.UI.RadScrollablePanel radScrollablePanel1;
    private Telerik.WinControls.UI.CommandBarButton btn_page_add;
    private Telerik.WinControls.UI.CommandBarLabel commandBarLabel3;
    private Telerik.WinControls.UI.CommandBarButton btn_sentence_add;
    private Telerik.WinControls.UI.CommandBarLabel lbl_page_view;
    private Telerik.WinControls.UI.CommandBarLabel lbl_sentence_view;
  }
}