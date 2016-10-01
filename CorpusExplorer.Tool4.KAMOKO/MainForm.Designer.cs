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
      this.btn_course_saveas = new Telerik.WinControls.UI.CommandBarButton();
      this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
      this.btn_page_add = new Telerik.WinControls.UI.CommandBarButton();
      this.commandBarLabel1 = new Telerik.WinControls.UI.CommandBarLabel();
      this.txt_index_document = new Telerik.WinControls.UI.CommandBarTextBox();
      this.warn_page = new Telerik.WinControls.UI.CommandBarButton();
      this.btn_document_prev = new Telerik.WinControls.UI.CommandBarButton();
      this.lbl_page_view = new Telerik.WinControls.UI.CommandBarLabel();
      this.btn_document_next = new Telerik.WinControls.UI.CommandBarButton();
      this.commandBarLabel3 = new Telerik.WinControls.UI.CommandBarLabel();
      this.btn_sentence_add = new Telerik.WinControls.UI.CommandBarButton();
      this.commandBarLabel2 = new Telerik.WinControls.UI.CommandBarLabel();
      this.txt_index_sentence = new Telerik.WinControls.UI.CommandBarTextBox();
      this.warn_sentence = new Telerik.WinControls.UI.CommandBarButton();
      this.btn_sentence_prev = new Telerik.WinControls.UI.CommandBarButton();
      this.lbl_sentence_view = new Telerik.WinControls.UI.CommandBarLabel();
      this.btn_sentence_next = new Telerik.WinControls.UI.CommandBarButton();
      this.btn_sentence_source = new Telerik.WinControls.UI.CommandBarButton();
      this.commandBarSeparator2 = new Telerik.WinControls.UI.CommandBarSeparator();
      this.btn_export = new Telerik.WinControls.UI.CommandBarButton();
      this.commandBarSeparator3 = new Telerik.WinControls.UI.CommandBarSeparator();
      this.btn_errorConsole = new Telerik.WinControls.UI.CommandBarButton();
      this.radScrollablePanel1 = new Telerik.WinControls.UI.RadScrollablePanel();
      this.radCollapsiblePanel1 = new Telerik.WinControls.UI.RadCollapsiblePanel();
      this.tree_course = new Telerik.WinControls.UI.RadTreeView();
      this.lbl_combinations = new Telerik.WinControls.UI.RadLabel();
      ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).BeginInit();
      this.radScrollablePanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).BeginInit();
      this.radCollapsiblePanel1.PanelContainer.SuspendLayout();
      this.radCollapsiblePanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tree_course)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbl_combinations)).BeginInit();
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
      this.radCommandBar1.Size = new System.Drawing.Size(1010, 44);
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
            this.btn_course_saveas,
            this.commandBarSeparator1,
            this.btn_page_add,
            this.commandBarLabel1,
            this.txt_index_document,
            this.warn_page,
            this.btn_document_prev,
            this.lbl_page_view,
            this.btn_document_next,
            this.commandBarLabel3,
            this.btn_sentence_add,
            this.commandBarLabel2,
            this.txt_index_sentence,
            this.warn_sentence,
            this.btn_sentence_prev,
            this.lbl_sentence_view,
            this.btn_sentence_next,
            this.btn_sentence_source,
            this.commandBarSeparator2,
            this.btn_export,
            this.commandBarSeparator3,
            this.btn_errorConsole});
      this.commandBarStripElement1.Name = "commandBarStripElement1";
      // 
      // btn_course_new
      // 
      this.btn_course_new.DisplayName = "commandBarButton1";
      this.btn_course_new.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.document;
      this.btn_course_new.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_course_new.Name = "btn_course_new";
      this.btn_course_new.Text = "Neuer Kurs";
      this.btn_course_new.Click += new System.EventHandler(this.btn_course_new_Click);
      // 
      // btn_course_open
      // 
      this.btn_course_open.DisplayName = "commandBarButton2";
      this.btn_course_open.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.folder;
      this.btn_course_open.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_course_open.Name = "btn_course_open";
      this.btn_course_open.Text = "Kurs laden";
      this.btn_course_open.Click += new System.EventHandler(this.btn_course_open_Click);
      // 
      // btn_course_save
      // 
      this.btn_course_save.DisplayName = "commandBarButton3";
      this.btn_course_save.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.save;
      this.btn_course_save.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_course_save.Name = "btn_course_save";
      this.btn_course_save.Text = "Kurs speichern";
      this.btn_course_save.Click += new System.EventHandler(this.btn_course_save_Click);
      // 
      // btn_course_saveas
      // 
      this.btn_course_saveas.DisplayName = "commandBarButton1";
      this.btn_course_saveas.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.storage_floppy_save;
      this.btn_course_saveas.Name = "btn_course_saveas";
      this.btn_course_saveas.Text = "Kurs speichern unter...";
      this.btn_course_saveas.Click += new System.EventHandler(this.btn_course_saveas_Click);
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
      this.btn_page_add.DisplayName = "commandBarButton1";
      this.btn_page_add.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.add_button_blue;
      this.btn_page_add.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_page_add.Name = "btn_page_add";
      this.btn_page_add.Text = "Blatt hinzufügen";
      this.btn_page_add.Click += new System.EventHandler(this.btn_page_add_Click);
      // 
      // commandBarLabel1
      // 
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
      // warn_page
      // 
      this.warn_page.DisplayName = "commandBarButton1";
      this.warn_page.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.warning;
      this.warn_page.Name = "warn_page";
      this.warn_page.Text = "Blatt-Nr. bereits vergeben!";
      this.warn_page.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
      // 
      // btn_document_prev
      // 
      this.btn_document_prev.DisplayName = "commandBarButton1";
      this.btn_document_prev.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.button_blue_arrow_left;
      this.btn_document_prev.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_document_prev.Name = "btn_document_prev";
      this.btn_document_prev.Text = "Dokument zurück";
      this.btn_document_prev.Click += new System.EventHandler(this.btn_document_prev_Click);
      // 
      // lbl_page_view
      // 
      this.lbl_page_view.DisplayName = "commandBarLabel4";
      this.lbl_page_view.Name = "lbl_page_view";
      this.lbl_page_view.Text = "0 / 0";
      // 
      // btn_document_next
      // 
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
      // warn_sentence
      // 
      this.warn_sentence.DisplayName = "commandBarButton2";
      this.warn_sentence.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.warning;
      this.warn_sentence.Name = "warn_sentence";
      this.warn_sentence.Text = "Satz-Nr. bereits vergeben!";
      this.warn_sentence.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
      // 
      // btn_sentence_prev
      // 
      this.btn_sentence_prev.DisplayName = "commandBarButton4";
      this.btn_sentence_prev.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.button_green_arrow_left;
      this.btn_sentence_prev.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_sentence_prev.Name = "btn_sentence_prev";
      this.btn_sentence_prev.Text = "Satz zurück";
      this.btn_sentence_prev.Click += new System.EventHandler(this.btn_sentence_prev_Click);
      // 
      // lbl_sentence_view
      // 
      this.lbl_sentence_view.DisplayName = "commandBarLabel5";
      this.lbl_sentence_view.Name = "lbl_sentence_view";
      this.lbl_sentence_view.Text = "0 / 0";
      // 
      // btn_sentence_next
      // 
      this.btn_sentence_next.DisplayName = "commandBarButton5";
      this.btn_sentence_next.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.button_green_arrow_right;
      this.btn_sentence_next.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_sentence_next.Name = "btn_sentence_next";
      this.btn_sentence_next.Text = "Satz vor";
      this.btn_sentence_next.Click += new System.EventHandler(this.btn_sentence_next_Click);
      // 
      // btn_sentence_source
      // 
      this.btn_sentence_source.DisplayName = "commandBarButton1";
      this.btn_sentence_source.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.tag_green;
      this.btn_sentence_source.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.btn_sentence_source.Name = "btn_sentence_source";
      this.btn_sentence_source.Text = "Satzquelle vermerken";
      this.btn_sentence_source.Click += new System.EventHandler(this.btn_sentence_source_Click);
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
      this.btn_export.DisplayName = "commandBarButton1";
      this.btn_export.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.export_to_database;
      this.btn_export.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
      this.btn_export.Name = "btn_export";
      this.btn_export.Text = "Exportieren";
      this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
      // 
      // commandBarSeparator3
      // 
      this.commandBarSeparator3.AccessibleDescription = "commandBarSeparator3";
      this.commandBarSeparator3.AccessibleName = "commandBarSeparator3";
      this.commandBarSeparator3.DisplayName = "commandBarSeparator3";
      this.commandBarSeparator3.Name = "commandBarSeparator3";
      this.commandBarSeparator3.VisibleInOverflowMenu = false;
      // 
      // btn_errorConsole
      // 
      this.btn_errorConsole.DisplayName = "commandBarButton1";
      this.btn_errorConsole.Image = global::CorpusExplorer.Tool4.KAMOKO.Properties.Resources.worker;
      this.btn_errorConsole.Name = "btn_errorConsole";
      this.btn_errorConsole.Text = "Fehler anzeigen";
      this.btn_errorConsole.Click += new System.EventHandler(this.btn_errorConsole_Click);
      // 
      // radScrollablePanel1
      // 
      this.radScrollablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.radScrollablePanel1.Location = new System.Drawing.Point(0, 44);
      this.radScrollablePanel1.Name = "radScrollablePanel1";
      // 
      // radScrollablePanel1.PanelContainer
      // 
      this.radScrollablePanel1.PanelContainer.Size = new System.Drawing.Size(448, 407);
      this.radScrollablePanel1.Size = new System.Drawing.Size(450, 409);
      this.radScrollablePanel1.TabIndex = 1;
      this.radScrollablePanel1.Text = "radScrollablePanel1";
      // 
      // radCollapsiblePanel1
      // 
      this.radCollapsiblePanel1.Dock = System.Windows.Forms.DockStyle.Right;
      this.radCollapsiblePanel1.ExpandDirection = Telerik.WinControls.UI.RadDirection.Left;
      this.radCollapsiblePanel1.HeaderText = "Inspektor";
      this.radCollapsiblePanel1.Location = new System.Drawing.Point(450, 44);
      this.radCollapsiblePanel1.Name = "radCollapsiblePanel1";
      this.radCollapsiblePanel1.OwnerBoundsCache = new System.Drawing.Rectangle(860, 44, 150, 409);
      // 
      // radCollapsiblePanel1.PanelContainer
      // 
      this.radCollapsiblePanel1.PanelContainer.Controls.Add(this.tree_course);
      this.radCollapsiblePanel1.PanelContainer.Controls.Add(this.lbl_combinations);
      this.radCollapsiblePanel1.PanelContainer.Size = new System.Drawing.Size(521, 407);
      this.radCollapsiblePanel1.Size = new System.Drawing.Size(560, 409);
      this.radCollapsiblePanel1.TabIndex = 2;
      this.radCollapsiblePanel1.VerticalHeaderAlignment = Telerik.WinControls.UI.RadVerticalAlignment.Center;
      ((Telerik.WinControls.UI.RadCollapsiblePanelElement)(this.radCollapsiblePanel1.GetChildAt(0))).ExpandDirection = Telerik.WinControls.UI.RadDirection.Left;
      ((Telerik.WinControls.UI.CollapsiblePanelHeaderElement)(this.radCollapsiblePanel1.GetChildAt(0).GetChildAt(1))).HorizontalHeaderAlignment = Telerik.WinControls.UI.RadHorizontalAlignment.Left;
      ((Telerik.WinControls.UI.CollapsiblePanelHeaderElement)(this.radCollapsiblePanel1.GetChildAt(0).GetChildAt(1))).VerticalHeaderAlignment = Telerik.WinControls.UI.RadVerticalAlignment.Center;
      ((Telerik.WinControls.UI.CollapsiblePanelHeaderElement)(this.radCollapsiblePanel1.GetChildAt(0).GetChildAt(1))).Orientation = System.Windows.Forms.Orientation.Vertical;
      // 
      // tree_course
      // 
      this.tree_course.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tree_course.ItemHeight = 40;
      this.tree_course.Location = new System.Drawing.Point(0, 0);
      this.tree_course.Name = "tree_course";
      this.tree_course.Size = new System.Drawing.Size(521, 380);
      this.tree_course.TabIndex = 1;
      this.tree_course.Text = "radTreeView1";
      this.tree_course.TreeIndent = 40;
      this.tree_course.NodeMouseDoubleClick += new Telerik.WinControls.UI.RadTreeView.TreeViewEventHandler(this.tree_course_NodeMouseDoubleClick);
      // 
      // lbl_combinations
      // 
      this.lbl_combinations.AutoSize = false;
      this.lbl_combinations.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.lbl_combinations.Location = new System.Drawing.Point(0, 380);
      this.lbl_combinations.Name = "lbl_combinations";
      this.lbl_combinations.Size = new System.Drawing.Size(521, 27);
      this.lbl_combinations.TabIndex = 0;
      this.lbl_combinations.Text = "<html><p>Kombinationen: &nbsp;<strong>0</strong></html>";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1010, 453);
      this.Controls.Add(this.radScrollablePanel1);
      this.Controls.Add(this.radCollapsiblePanel1);
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
      this.Load += new System.EventHandler(this.MainForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).EndInit();
      this.radScrollablePanel1.ResumeLayout(false);
      this.radCollapsiblePanel1.PanelContainer.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).EndInit();
      this.radCollapsiblePanel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.tree_course)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbl_combinations)).EndInit();
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
    private Telerik.WinControls.UI.CommandBarButton btn_course_saveas;
    private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator3;
    private Telerik.WinControls.UI.CommandBarButton btn_errorConsole;
    private Telerik.WinControls.UI.CommandBarButton btn_sentence_source;
    private Telerik.WinControls.UI.RadCollapsiblePanel radCollapsiblePanel1;
    private Telerik.WinControls.UI.RadTreeView tree_course;
    private Telerik.WinControls.UI.RadLabel lbl_combinations;
    private Telerik.WinControls.UI.CommandBarButton warn_page;
    private Telerik.WinControls.UI.CommandBarButton warn_sentence;
  }
}