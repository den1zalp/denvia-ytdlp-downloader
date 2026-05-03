namespace YTDlpGui
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            btnEasterEgg = new Button();
            lnkGithub = new LinkLabel();
            lblSubtitle = new Label();
            lblAppTitle = new Label();
            pnlInput = new Panel();
            btnAnalyze = new Button();
            txtUrl = new TextBox();
            label1 = new Label();
            pnlPreview = new Panel();
            lblVideoInfo = new Label();
            lblVideoTitle = new Label();
            picThumbnail = new PictureBox();
            pnlSettings = new Panel();
            chkPlaylistMode = new CheckBox();
            chkUseArchive = new CheckBox();
            numMaxVideos = new NumericUpDown();
            label4 = new Label();
            btnCancel = new Button();
            btnDownload = new Button();
            btnBrowse = new Button();
            txtFolder = new TextBox();
            label2 = new Label();
            cmbQuality = new ComboBox();
            label3 = new Label();
            pnlLogs = new Panel();
            txtLog = new TextBox();
            lblProgress = new Label();
            progressDownload = new ProgressBar();
            pnlHeader.SuspendLayout();
            pnlInput.SuspendLayout();
            pnlPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picThumbnail).BeginInit();
            pnlSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxVideos).BeginInit();
            pnlLogs.SuspendLayout();
            SuspendLayout();

            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(30, 33, 42);
            pnlHeader.Controls.Add(btnEasterEgg);
            pnlHeader.Controls.Add(lnkGithub);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(lblAppTitle);
            pnlHeader.Location = new Point(20, 18);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(860, 82);
            pnlHeader.TabIndex = 0;

            // 
            // btnEasterEgg
            // 
            btnEasterEgg.BackColor = Color.FromArgb(30, 33, 42);
            btnEasterEgg.FlatAppearance.BorderSize = 0;
            btnEasterEgg.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 33, 42);
            btnEasterEgg.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 33, 42);
            btnEasterEgg.FlatStyle = FlatStyle.Flat;
            btnEasterEgg.ForeColor = Color.FromArgb(30, 33, 42);
            btnEasterEgg.Location = new Point(820, 10);
            btnEasterEgg.Name = "btnEasterEgg";
            btnEasterEgg.Size = new Size(28, 28);
            btnEasterEgg.TabIndex = 3;
            btnEasterEgg.TabStop = false;
            btnEasterEgg.Text = "";
            btnEasterEgg.UseVisualStyleBackColor = false;
            btnEasterEgg.Click += btnEasterEgg_Click;

            // 
            // lblAppTitle
            // 
            lblAppTitle.AutoSize = true;
            lblAppTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.Location = new Point(22, 14);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Size = new Size(356, 32);
            lblAppTitle.TabIndex = 0;
            lblAppTitle.Text = "yt-dlp Downloader by den1zalp";

            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.FromArgb(160, 170, 185);
            lblSubtitle.Location = new Point(25, 50);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(244, 15);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Fast, portable video downloader interface";

            // 
            // lnkGithub
            // 
            lnkGithub.ActiveLinkColor = Color.White;
            lnkGithub.AutoSize = true;
            lnkGithub.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lnkGithub.LinkColor = Color.FromArgb(140, 200, 255);
            lnkGithub.Location = new Point(706, 50);
            lnkGithub.Name = "lnkGithub";
            lnkGithub.Size = new Size(126, 15);
            lnkGithub.TabIndex = 2;
            lnkGithub.TabStop = true;
            lnkGithub.Text = "github.com/den1zalp";
            lnkGithub.VisitedLinkColor = Color.FromArgb(140, 200, 255);
            lnkGithub.LinkClicked += lnkGithub_LinkClicked;

            // 
            // pnlInput
            // 
            pnlInput.BackColor = Color.FromArgb(30, 33, 42);
            pnlInput.Controls.Add(btnAnalyze);
            pnlInput.Controls.Add(txtUrl);
            pnlInput.Controls.Add(label1);
            pnlInput.Location = new Point(20, 116);
            pnlInput.Name = "pnlInput";
            pnlInput.Size = new Size(860, 78);
            pnlInput.TabIndex = 1;

            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(210, 210, 210);
            label1.Location = new Point(22, 17);
            label1.Name = "label1";
            label1.Size = new Size(61, 15);
            label1.TabIndex = 0;
            label1.Text = "Video URL";

            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(22, 39);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(680, 23);
            txtUrl.TabIndex = 1;

            // 
            // btnAnalyze
            // 
            btnAnalyze.Location = new Point(720, 36);
            btnAnalyze.Name = "btnAnalyze";
            btnAnalyze.Size = new Size(115, 29);
            btnAnalyze.TabIndex = 2;
            btnAnalyze.Text = "Analyze";
            btnAnalyze.UseVisualStyleBackColor = true;
            btnAnalyze.Click += btnAnalyze_Click;

            // 
            // pnlPreview
            // 
            pnlPreview.BackColor = Color.FromArgb(30, 33, 42);
            pnlPreview.Controls.Add(lblVideoInfo);
            pnlPreview.Controls.Add(lblVideoTitle);
            pnlPreview.Controls.Add(picThumbnail);
            pnlPreview.Location = new Point(20, 210);
            pnlPreview.Name = "pnlPreview";
            pnlPreview.Size = new Size(420, 250);
            pnlPreview.TabIndex = 2;

            // 
            // picThumbnail
            // 
            picThumbnail.BackColor = Color.FromArgb(35, 38, 47);
            picThumbnail.Location = new Point(22, 22);
            picThumbnail.Name = "picThumbnail";
            picThumbnail.Size = new Size(180, 105);
            picThumbnail.SizeMode = PictureBoxSizeMode.Zoom;
            picThumbnail.TabIndex = 0;
            picThumbnail.TabStop = false;

            // 
            // lblVideoTitle
            // 
            lblVideoTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblVideoTitle.ForeColor = Color.White;
            lblVideoTitle.Location = new Point(22, 150);
            lblVideoTitle.Name = "lblVideoTitle";
            lblVideoTitle.Size = new Size(375, 45);
            lblVideoTitle.TabIndex = 1;
            lblVideoTitle.Text = "No video loaded";

            // 
            // lblVideoInfo
            // 
            lblVideoInfo.AutoSize = true;
            lblVideoInfo.ForeColor = Color.FromArgb(160, 170, 185);
            lblVideoInfo.Location = new Point(22, 205);
            lblVideoInfo.Name = "lblVideoInfo";
            lblVideoInfo.Size = new Size(0, 15);
            lblVideoInfo.TabIndex = 2;

            // 
            // pnlSettings
            // 
            pnlSettings.BackColor = Color.FromArgb(30, 33, 42);
            pnlSettings.Controls.Add(chkPlaylistMode);
            pnlSettings.Controls.Add(chkUseArchive);
            pnlSettings.Controls.Add(numMaxVideos);
            pnlSettings.Controls.Add(label4);
            pnlSettings.Controls.Add(btnCancel);
            pnlSettings.Controls.Add(btnDownload);
            pnlSettings.Controls.Add(btnBrowse);
            pnlSettings.Controls.Add(txtFolder);
            pnlSettings.Controls.Add(label2);
            pnlSettings.Controls.Add(cmbQuality);
            pnlSettings.Controls.Add(label3);
            pnlSettings.Location = new Point(460, 210);
            pnlSettings.Name = "pnlSettings";
            pnlSettings.Size = new Size(420, 250);
            pnlSettings.TabIndex = 3;

            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.FromArgb(210, 210, 210);
            label3.Location = new Point(22, 22);
            label3.Name = "label3";
            label3.Size = new Size(45, 15);
            label3.TabIndex = 0;
            label3.Text = "Quality";

            // 
            // cmbQuality
            // 
            cmbQuality.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbQuality.FormattingEnabled = true;
            cmbQuality.Location = new Point(22, 45);
            cmbQuality.Name = "cmbQuality";
            cmbQuality.Size = new Size(180, 23);
            cmbQuality.TabIndex = 1;

            // 
            // chkPlaylistMode
            // 
            chkPlaylistMode.AutoSize = true;
            chkPlaylistMode.ForeColor = Color.FromArgb(210, 210, 210);
            chkPlaylistMode.Location = new Point(220, 47);
            chkPlaylistMode.Name = "chkPlaylistMode";
            chkPlaylistMode.Size = new Size(151, 19);
            chkPlaylistMode.TabIndex = 2;
            chkPlaylistMode.Text = "Playlist / Channel mode";
            chkPlaylistMode.UseVisualStyleBackColor = true;
            chkPlaylistMode.CheckedChanged += chkPlaylistMode_CheckedChanged;

            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.FromArgb(210, 210, 210);
            label4.Location = new Point(22, 82);
            label4.Name = "label4";
            label4.Size = new Size(65, 15);
            label4.TabIndex = 3;
            label4.Text = "Max videos";

            // 
            // numMaxVideos
            // 
            numMaxVideos.Location = new Point(105, 80);
            numMaxVideos.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numMaxVideos.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxVideos.Name = "numMaxVideos";
            numMaxVideos.Size = new Size(97, 23);
            numMaxVideos.TabIndex = 4;
            numMaxVideos.Value = new decimal(new int[] { 25, 0, 0, 0 });

            // 
            // chkUseArchive
            // 
            chkUseArchive.AutoSize = true;
            chkUseArchive.Checked = true;
            chkUseArchive.CheckState = CheckState.Checked;
            chkUseArchive.ForeColor = Color.FromArgb(210, 210, 210);
            chkUseArchive.Location = new Point(220, 82);
            chkUseArchive.Name = "chkUseArchive";
            chkUseArchive.Size = new Size(160, 19);
            chkUseArchive.TabIndex = 5;
            chkUseArchive.Text = "Skip already downloaded";
            chkUseArchive.UseVisualStyleBackColor = true;

            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.FromArgb(210, 210, 210);
            label2.Location = new Point(22, 118);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 6;
            label2.Text = "Output";

            // 
            // txtFolder
            // 
            txtFolder.Location = new Point(22, 141);
            txtFolder.Name = "txtFolder";
            txtFolder.Size = new Size(280, 23);
            txtFolder.TabIndex = 7;

            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(315, 138);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(82, 29);
            btnBrowse.TabIndex = 8;
            btnBrowse.Text = "Choose";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;

            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(22, 195);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(130, 32);
            btnDownload.TabIndex = 9;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;

            // 
            // btnCancel
            // 
            btnCancel.Enabled = false;
            btnCancel.Location = new Point(165, 195);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(110, 32);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;

            // 
            // pnlLogs
            // 
            pnlLogs.BackColor = Color.FromArgb(30, 33, 42);
            pnlLogs.Controls.Add(txtLog);
            pnlLogs.Controls.Add(lblProgress);
            pnlLogs.Controls.Add(progressDownload);
            pnlLogs.Location = new Point(20, 476);
            pnlLogs.Name = "pnlLogs";
            pnlLogs.Size = new Size(860, 230);
            pnlLogs.TabIndex = 4;

            // 
            // progressDownload
            // 
            progressDownload.Location = new Point(22, 22);
            progressDownload.Name = "progressDownload";
            progressDownload.Size = new Size(810, 20);
            progressDownload.TabIndex = 0;

            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.ForeColor = Color.FromArgb(140, 200, 255);
            lblProgress.Location = new Point(22, 52);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(39, 15);
            lblProgress.TabIndex = 1;
            lblProgress.Text = "Ready";

            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(17, 18, 22);
            txtLog.ForeColor = Color.FromArgb(210, 210, 210);
            txtLog.Location = new Point(22, 78);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(810, 130);
            txtLog.TabIndex = 2;

            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 26, 32);
            ClientSize = new Size(900, 730);
            Controls.Add(pnlLogs);
            Controls.Add(pnlSettings);
            Controls.Add(pnlPreview);
            Controls.Add(pnlInput);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "yt-dlp Downloader by den1zalp";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlInput.ResumeLayout(false);
            pnlInput.PerformLayout();
            pnlPreview.ResumeLayout(false);
            pnlPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picThumbnail).EndInit();
            pnlSettings.ResumeLayout(false);
            pnlSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxVideos).EndInit();
            pnlLogs.ResumeLayout(false);
            pnlLogs.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Button btnEasterEgg;
        private Label lblSubtitle;
        private Label lblAppTitle;
        private LinkLabel lnkGithub;
        private Panel pnlInput;
        private Label label1;
        private TextBox txtUrl;
        private Button btnAnalyze;
        private Panel pnlPreview;
        private PictureBox picThumbnail;
        private Label lblVideoTitle;
        private Label lblVideoInfo;
        private Panel pnlSettings;
        private CheckBox chkPlaylistMode;
        private CheckBox chkUseArchive;
        private NumericUpDown numMaxVideos;
        private Label label4;
        private Label label3;
        private ComboBox cmbQuality;
        private Label label2;
        private TextBox txtFolder;
        private Button btnBrowse;
        private Button btnDownload;
        private Button btnCancel;
        private Panel pnlLogs;
        private ProgressBar progressDownload;
        private Label lblProgress;
        private TextBox txtLog;
    }
}