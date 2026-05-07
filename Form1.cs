using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YTDlpGui
{
    public partial class Form1 : Form
    {
        private Process? currentProcess;
        private bool isCancelling;
        private int easterEggClickCount = 0;

        private bool isVideoAnalyzed = false;
        private string analyzedUrl = string.Empty;

        private sealed class QualityOption
        {
            public string DisplayText { get; }
            public int? MaxHeight { get; }
            public bool AudioOnly { get; }

            public QualityOption(string displayText, int? maxHeight = null, bool audioOnly = false)
            {
                DisplayText = displayText;
                MaxHeight = maxHeight;
                AudioOnly = audioOnly;
            }

            public override string ToString()
            {
                return DisplayText;
            }
        }

        public Form1()
        {
            InitializeComponent();

            txtFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            progressDownload.Value = 0;
            lblProgress.Text = "Ready";

            btnCancel.Enabled = false;
            btnDownload.Enabled = false;

            picThumbnail.SizeMode = PictureBoxSizeMode.Zoom;
            lblVideoTitle.Text = "No video loaded";
            lblVideoInfo.Text = string.Empty;

            chkUseArchive.Checked = true;
            chkUseArchive.Enabled = false;
            numMaxVideos.Value = 25;
            numMaxVideos.Enabled = false;

            LoadLockedQualityPlaceholder();

            txtUrl.TextChanged += txtUrl_TextChanged;

            ApplyModernTheme();
            ApplyRoundedCorners();
        }

        private void txtUrl_TextChanged(object? sender, EventArgs e)
        {
            if (chkPlaylistMode.Checked)
            {
                btnDownload.Enabled = !string.IsNullOrWhiteSpace(txtUrl.Text);
                return;
            }

            if (!isVideoAnalyzed)
            {
                return;
            }

            string currentUrl = txtUrl.Text.Trim();

            if (!string.Equals(currentUrl, analyzedUrl, StringComparison.OrdinalIgnoreCase))
            {
                ResetAnalyzedVideoState();
                lblProgress.Text = "Analyze required";
            }
        }

        private void chkPlaylistMode_CheckedChanged(object? sender, EventArgs e)
        {
            if (chkPlaylistMode.Checked)
            {
                isVideoAnalyzed = false;
                analyzedUrl = string.Empty;

                picThumbnail.Image?.Dispose();
                picThumbnail.Image = null;

                lblVideoTitle.Text = "Playlist / Channel mode";
                lblVideoInfo.Text = "Analyze is skipped. The link will be downloaded directly.";

                LoadPlaylistQualityOptions();

                btnAnalyze.Enabled = false;
                btnDownload.Enabled = !string.IsNullOrWhiteSpace(txtUrl.Text);

                chkUseArchive.Enabled = true;
                numMaxVideos.Enabled = true;

                lblProgress.Text = "Playlist / Channel mode ready";
            }
            else
            {
                ResetAnalyzedVideoState();

                btnAnalyze.Enabled = true;
                btnDownload.Enabled = false;

                chkUseArchive.Enabled = false;
                numMaxVideos.Enabled = false;

                lblProgress.Text = "Analyze required";
            }
        }

        private async void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (chkPlaylistMode.Checked)
            {
                MessageBox.Show("Analyze is disabled in Playlist / Channel mode. You can download directly.");
                return;
            }

            string url = txtUrl.Text.Trim();

            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("You have not entered a link.");
                return;
            }

            string appFolder = AppContext.BaseDirectory;
            string toolsFolder = Path.Combine(appFolder, "Tools");
            string ytDlpPath = Path.Combine(toolsFolder, "yt-dlp.exe");
            string denoPath = Path.Combine(toolsFolder, "deno.exe");

            if (!File.Exists(ytDlpPath))
            {
                MessageBox.Show("yt-dlp.exe could not be found:\n" + ytDlpPath);
                return;
            }

            if (!File.Exists(denoPath))
            {
                MessageBox.Show("deno.exe could not be found:\n" + denoPath);
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ytDlpPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            startInfo.ArgumentList.Add("-J");
            startInfo.ArgumentList.Add("--no-playlist");

            startInfo.ArgumentList.Add("--ffmpeg-location");
            startInfo.ArgumentList.Add(toolsFolder);

            startInfo.ArgumentList.Add("--js-runtimes");
            startInfo.ArgumentList.Add("deno:" + denoPath);

            startInfo.ArgumentList.Add(url);

            txtLog.Clear();
            lblProgress.Text = "Analyzing...";
            AppendLog("Analyzing video information...");

            ResetAnalyzedVideoState();

            btnAnalyze.Enabled = false;
            btnAnalyze.Text = "Analyzing...";
            btnDownload.Enabled = false;

            string jsonOutput = string.Empty;
            string errorOutput = string.Empty;
            int exitCode = -1;

            await Task.Run(() =>
            {
                using Process process = new Process();
                process.StartInfo = startInfo;

                process.Start();

                jsonOutput = process.StandardOutput.ReadToEnd();
                errorOutput = process.StandardError.ReadToEnd();

                process.WaitForExit();
                exitCode = process.ExitCode;
            });

            btnAnalyze.Enabled = true;
            btnAnalyze.Text = "Analyze";
            btnDownload.Enabled = isVideoAnalyzed;

            if (exitCode != 0)
            {
                lblProgress.Text = "Analyze failed";

                AppendLog("STDERR:");
                AppendLog(errorOutput);

                AppendLog("STDOUT:");
                AppendLog(jsonOutput);

                MessageBox.Show("Could not analyze the video. Check the logs.");
                return;
            }

            try
            {
                await ApplyVideoInfoFromJsonAsync(jsonOutput);

                isVideoAnalyzed = true;
                analyzedUrl = url;

                cmbQuality.Enabled = true;
                btnDownload.Enabled = true;

                lblProgress.Text = "Video loaded";
                AppendLog("Video information loaded.");
            }
            catch (Exception ex)
            {
                ResetAnalyzedVideoState();

                lblProgress.Text = "Analyze failed";
                AppendLog("Could not parse video information: " + ex.Message);
                MessageBox.Show("Could not parse video information. Check the logs.");
            }
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();

            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("You have not entered a link.");
                return;
            }

            if (!chkPlaylistMode.Checked)
            {
                if (!isVideoAnalyzed || !string.Equals(url, analyzedUrl, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Please analyze the video first.");
                    return;
                }
            }

            string appFolder = AppContext.BaseDirectory;
            string toolsFolder = Path.Combine(appFolder, "Tools");
            string ytDlpPath = Path.Combine(toolsFolder, "yt-dlp.exe");
            string denoPath = Path.Combine(toolsFolder, "deno.exe");

            if (!File.Exists(ytDlpPath))
            {
                MessageBox.Show("yt-dlp.exe could not be found:\n" + ytDlpPath);
                return;
            }

            if (!File.Exists(denoPath))
            {
                MessageBox.Show("deno.exe could not be found:\n" + denoPath);
                return;
            }

            string selectedFolder = txtFolder.Text.Trim();

            if (!Directory.Exists(selectedFolder))
            {
                MessageBox.Show("The selected folder could not be found.");
                return;
            }

            string outputTemplate = chkPlaylistMode.Checked
                ? Path.Combine(selectedFolder, "%(uploader)s", "%(playlist_index)s - %(title)s.%(ext)s")
                : Path.Combine(selectedFolder, "%(title)s.%(ext)s");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ytDlpPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            startInfo.ArgumentList.Add("--newline");

            if (chkPlaylistMode.Checked)
            {
                startInfo.ArgumentList.Add("--yes-playlist");

                int maxVideos = (int)numMaxVideos.Value;

                if (maxVideos > 0)
                {
                    startInfo.ArgumentList.Add("--playlist-end");
                    startInfo.ArgumentList.Add(maxVideos.ToString(CultureInfo.InvariantCulture));
                }

                if (chkUseArchive.Checked)
                {
                    string archivePath = Path.Combine(selectedFolder, "download-archive.txt");

                    startInfo.ArgumentList.Add("--download-archive");
                    startInfo.ArgumentList.Add(archivePath);
                }
            }
            else
            {
                startInfo.ArgumentList.Add("--no-playlist");
            }

            AddQualityArguments(startInfo);

            startInfo.ArgumentList.Add("--ffmpeg-location");
            startInfo.ArgumentList.Add(toolsFolder);

            startInfo.ArgumentList.Add("--js-runtimes");
            startInfo.ArgumentList.Add("deno:" + denoPath);

            startInfo.ArgumentList.Add("-o");
            startInfo.ArgumentList.Add(outputTemplate);

            startInfo.ArgumentList.Add(url);

            txtLog.Clear();
            progressDownload.Value = 0;
            lblProgress.Text = "Starting...";
            AppendLog("Download started...");

            if (chkPlaylistMode.Checked)
            {
                AppendLog("Playlist / Channel mode is enabled.");
                AppendLog("Max videos: " + numMaxVideos.Value.ToString(CultureInfo.InvariantCulture));

                if (chkUseArchive.Checked)
                {
                    AppendLog("Archive mode: enabled. Already downloaded items will be skipped.");
                }
            }

            isCancelling = false;
            SetDownloadUiState(isDownloading: true);

            int exitCode = -1;

            await Task.Run(() =>
            {
                using Process process = new Process();
                currentProcess = process;
                process.StartInfo = startInfo;

                process.OutputDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrWhiteSpace(args.Data))
                    {
                        AppendLog(args.Data);
                        UpdateProgressFromLine(args.Data);
                    }
                };

                process.ErrorDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrWhiteSpace(args.Data))
                    {
                        AppendLog(args.Data);
                        UpdateProgressFromLine(args.Data);
                    }
                };

                try
                {
                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    exitCode = process.ExitCode;
                }
                finally
                {
                    currentProcess = null;
                }
            });

            SetDownloadUiState(isDownloading: false);

            if (isCancelling)
            {
                lblProgress.Text = "Cancelled";
                AppendLog("Download cancelled.");
                return;
            }

            if (exitCode == 0)
            {
                progressDownload.Value = 100;
                lblProgress.Text = "Done";
                AppendLog("Download completed.");
                MessageBox.Show("Download has been completed.");
            }
            else
            {
                lblProgress.Text = "Error";
                AppendLog("Process exited with error code: " + exitCode);
                MessageBox.Show("There was an error while downloading. Check the logs.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (currentProcess == null || currentProcess.HasExited)
            {
                return;
            }

            try
            {
                isCancelling = true;
                lblProgress.Text = "Cancelling...";
                AppendLog("Cancellation request sent...");

                currentProcess.Kill(entireProcessTree: true);
            }
            catch (Exception ex)
            {
                AppendLog("Error while cancelling: " + ex.Message);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.Description = "Choose a destination where the file will be saved.";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = dialog.SelectedPath;
            }
        }

        private void lnkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/den1zalp",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open GitHub link:\n" + ex.Message);
            }
        }

        private void btnEasterEgg_Click(object sender, EventArgs e)
        {
            easterEggClickCount++;

            if (easterEggClickCount >= 5)
            {
                easterEggClickCount = 0;
                MessageBox.Show("Silvia is a cook!!", "Easter Egg");
            }
        }

        private void AddQualityArguments(ProcessStartInfo startInfo)
        {
            QualityOption selectedOption = cmbQuality.SelectedItem as QualityOption
                ?? new QualityOption("Best MP4");

            if (selectedOption.AudioOnly)
            {
                startInfo.ArgumentList.Add("-x");
                startInfo.ArgumentList.Add("--audio-format");
                startInfo.ArgumentList.Add("mp3");
                return;
            }

            string formatSelector;

            if (selectedOption.MaxHeight.HasValue)
            {
                int height = selectedOption.MaxHeight.Value;
                formatSelector = $"bv*[height<={height}][ext=mp4]+ba[ext=m4a]/b[height<={height}][ext=mp4]/bv*[height<={height}]+ba/b[height<={height}]";
            }
            else
            {
                formatSelector = "bv*[ext=mp4]+ba[ext=m4a]/b[ext=mp4]/bv*+ba/b";
            }

            startInfo.ArgumentList.Add("-f");
            startInfo.ArgumentList.Add(formatSelector);

            startInfo.ArgumentList.Add("--merge-output-format");
            startInfo.ArgumentList.Add("mp4");
        }

        private async Task ApplyVideoInfoFromJsonAsync(string jsonOutput)
        {
            using JsonDocument document = JsonDocument.Parse(jsonOutput);
            JsonElement root = document.RootElement;

            string title = GetJsonString(root, "title", "Unknown title");
            string extractor = GetJsonString(root, "extractor_key", "Unknown site");
            List<string> thumbnailUrls = GetThumbnailCandidates(root);
            double durationSeconds = GetJsonDouble(root, "duration", 0);

            lblVideoTitle.Text = title;
            lblVideoInfo.Text = BuildVideoInfoText(extractor, durationSeconds);

            PopulateQualityOptionsFromJson(root);
            await LoadThumbnailAsync(thumbnailUrls);
        }

        private void PopulateQualityOptionsFromJson(JsonElement root)
        {
            List<int> heights = new List<int>();

            if (root.TryGetProperty("formats", out JsonElement formats) && formats.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement format in formats.EnumerateArray())
                {
                    if (!FormatHasVideo(format))
                    {
                        continue;
                    }

                    int height = GetJsonInt(format, "height", 0);

                    if (height <= 0)
                    {
                        continue;
                    }

                    if (!heights.Contains(height))
                    {
                        heights.Add(height);
                    }
                }
            }

            heights.Sort((a, b) => b.CompareTo(a));

            cmbQuality.Items.Clear();
            cmbQuality.Items.Add(new QualityOption("Best MP4"));

            foreach (int height in heights)
            {
                cmbQuality.Items.Add(new QualityOption($"{height}p MP4", height));
            }

            cmbQuality.Items.Add(new QualityOption("Audio Only MP3", audioOnly: true));
            cmbQuality.SelectedIndex = 0;
        }

        private bool FormatHasVideo(JsonElement format)
        {
            string vcodec = GetJsonString(format, "vcodec", string.Empty);
            return !string.Equals(vcodec, "none", StringComparison.OrdinalIgnoreCase);
        }

        private void LoadLockedQualityPlaceholder()
        {
            cmbQuality.Items.Clear();
            cmbQuality.Items.Add("Analyze a video first");
            cmbQuality.SelectedIndex = 0;
            cmbQuality.Enabled = false;
        }

        private void LoadPlaylistQualityOptions()
        {
            cmbQuality.Items.Clear();
            cmbQuality.Items.Add(new QualityOption("Best MP4"));
            cmbQuality.Items.Add(new QualityOption("1080p or below MP4", 1080));
            cmbQuality.Items.Add(new QualityOption("720p or below MP4", 720));
            cmbQuality.Items.Add(new QualityOption("480p or below MP4", 480));
            cmbQuality.Items.Add(new QualityOption("Audio Only MP3", audioOnly: true));
            cmbQuality.SelectedIndex = 0;
            cmbQuality.Enabled = true;
        }

        private void ResetAnalyzedVideoState()
        {
            isVideoAnalyzed = false;
            analyzedUrl = string.Empty;

            btnDownload.Enabled = false;
            LoadLockedQualityPlaceholder();

            lblVideoTitle.Text = "No video loaded";
            lblVideoInfo.Text = string.Empty;

            picThumbnail.Image?.Dispose();
            picThumbnail.Image = null;
        }

        private async Task LoadThumbnailAsync(List<string> thumbnailUrls)
        {
            picThumbnail.Image?.Dispose();
            picThumbnail.Image = null;

            if (thumbnailUrls.Count == 0)
            {
                return;
            }

            foreach (string thumbnailUrl in thumbnailUrls)
            {
                try
                {
                    byte[] imageBytes = await DownloadThumbnailBytesAsync(thumbnailUrl);

                    using MemoryStream stream = new MemoryStream(imageBytes);
                    using Image image = Image.FromStream(stream, false, true);

                    picThumbnail.Image?.Dispose();
                    picThumbnail.Image = new Bitmap(image);

                    return;
                }
                catch (Exception ex)
                {
                    AppendLog("Thumbnail candidate failed: " + ex.Message);
                }
            }

            AppendLog("No usable thumbnail could be loaded.");
        }

        private async Task<byte[]> DownloadThumbnailBytesAsync(string thumbnailUrl)
        {
            using HttpClientHandler handler = new HttpClientHandler
            {
                AutomaticDecompression =
                    DecompressionMethods.GZip |
                    DecompressionMethods.Deflate |
                    DecompressionMethods.Brotli
            };

            using HttpClient client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(15)
            };

            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, thumbnailUrl);

            request.Headers.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:126.0) Gecko/20100101 Firefox/126.0"
            );

            request.Headers.Accept.ParseAdd(
                "image/jpeg,image/png,image/apng,image/svg+xml,image/*;q=0.8,*/*;q=0.5"
            );

            request.Headers.AcceptLanguage.ParseAdd("en-US,en;q=0.9,tr;q=0.8");

            request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "image");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "no-cors");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "cross-site");

            if (thumbnailUrl.Contains("redd.it", StringComparison.OrdinalIgnoreCase) ||
                thumbnailUrl.Contains("redditmedia.com", StringComparison.OrdinalIgnoreCase))
            {
                request.Headers.Referrer = new Uri("https://www.reddit.com/");
            }

            using HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string? mediaType = response.Content.Headers.ContentType?.MediaType;

            if (!string.IsNullOrWhiteSpace(mediaType) &&
                !mediaType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Thumbnail response was not an image: " + mediaType);
            }

            return await response.Content.ReadAsByteArrayAsync();
        }

        private List<string> GetThumbnailCandidates(JsonElement root)
        {
            List<string> candidates = new List<string>();

            AddThumbnailCandidate(candidates, GetJsonString(root, "thumbnail", string.Empty));

            if (root.TryGetProperty("thumbnails", out JsonElement thumbnails) &&
                thumbnails.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement thumbnail in thumbnails.EnumerateArray())
                {
                    AddThumbnailCandidate(candidates, GetJsonString(thumbnail, "url", string.Empty));
                }
            }

            if (root.TryGetProperty("preview", out JsonElement preview) &&
                preview.ValueKind == JsonValueKind.Object &&
                preview.TryGetProperty("images", out JsonElement images) &&
                images.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement previewImage in images.EnumerateArray())
                {
                    if (previewImage.TryGetProperty("source", out JsonElement source))
                    {
                        AddThumbnailCandidate(candidates, GetJsonString(source, "url", string.Empty));
                    }
                }
            }

            return candidates;
        }

        private void AddThumbnailCandidate(List<string> candidates, string thumbnailUrl)
        {
            if (string.IsNullOrWhiteSpace(thumbnailUrl))
            {
                return;
            }

            thumbnailUrl = WebUtility.HtmlDecode(thumbnailUrl).Trim();

            if (thumbnailUrl == "default" ||
                thumbnailUrl == "self" ||
                thumbnailUrl == "nsfw" ||
                thumbnailUrl == "spoiler" ||
                thumbnailUrl == "image")
            {
                return;
            }

            if (!Uri.IsWellFormedUriString(thumbnailUrl, UriKind.Absolute))
            {
                return;
            }

            if (!candidates.Contains(thumbnailUrl))
            {
                candidates.Add(thumbnailUrl);
            }
        }

        private string BuildVideoInfoText(string extractor, double durationSeconds)
        {
            string durationText = durationSeconds > 0
                ? FormatDuration(durationSeconds)
                : "Unknown duration";

            return extractor + " • " + durationText;
        }

        private string FormatDuration(double totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);

            if (time.TotalHours >= 1)
            {
                return time.ToString(@"h\:mm\:ss");
            }

            return time.ToString(@"m\:ss");
        }

        private string GetJsonString(JsonElement element, string propertyName, string fallback)
        {
            if (element.TryGetProperty(propertyName, out JsonElement property) && property.ValueKind == JsonValueKind.String)
            {
                return property.GetString() ?? fallback;
            }

            return fallback;
        }

        private int GetJsonInt(JsonElement element, string propertyName, int fallback)
        {
            if (element.TryGetProperty(propertyName, out JsonElement property) && property.ValueKind == JsonValueKind.Number)
            {
                return property.GetInt32();
            }

            return fallback;
        }

        private double GetJsonDouble(JsonElement element, string propertyName, double fallback)
        {
            if (element.TryGetProperty(propertyName, out JsonElement property) && property.ValueKind == JsonValueKind.Number)
            {
                return property.GetDouble();
            }

            return fallback;
        }

        private void AppendLog(string text)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => AppendLog(text)));
                return;
            }

            txtLog.AppendText(text + Environment.NewLine);
        }

        private void UpdateProgressFromLine(string line)
        {
            Match match = Regex.Match(line, @"\[download\]\s+(\d+(?:\.\d+)?)%");

            if (!match.Success)
            {
                return;
            }

            if (double.TryParse(
                match.Groups[1].Value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out double percent))
            {
                int value = Math.Max(0, Math.Min(100, (int)Math.Round(percent)));

                if (progressDownload.InvokeRequired)
                {
                    progressDownload.Invoke(new Action(() =>
                    {
                        progressDownload.Value = value;
                        lblProgress.Text = $"Downloading... {value}%";
                    }));

                    return;
                }

                progressDownload.Value = value;
                lblProgress.Text = $"Downloading... {value}%";
            }
        }

        private void SetDownloadUiState(bool isDownloading)
        {
            btnDownload.Enabled = !isDownloading && (isVideoAnalyzed || chkPlaylistMode.Checked);
            btnCancel.Enabled = isDownloading;
            btnAnalyze.Enabled = !isDownloading && !chkPlaylistMode.Checked;
            btnBrowse.Enabled = !isDownloading;
            txtUrl.Enabled = !isDownloading;
            txtFolder.Enabled = !isDownloading;
            cmbQuality.Enabled = !isDownloading && (isVideoAnalyzed || chkPlaylistMode.Checked);

            chkPlaylistMode.Enabled = !isDownloading;
            chkUseArchive.Enabled = !isDownloading && chkPlaylistMode.Checked;
            numMaxVideos.Enabled = !isDownloading && chkPlaylistMode.Checked;

            btnDownload.Text = isDownloading ? "Downloading..." : "Download";
        }

        private void ApplyModernTheme()
        {
            Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            BackColor = Color.FromArgb(24, 26, 32);
            ForeColor = Color.FromArgb(230, 230, 230);

            Text = "yt-dlp Downloader by den1zalp";
            StartPosition = FormStartPosition.CenterScreen;

            StylePanel(pnlHeader);
            StylePanel(pnlInput);
            StylePanel(pnlPreview);
            StylePanel(pnlSettings);
            StylePanel(pnlLogs);

            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);

            lblSubtitle.ForeColor = Color.FromArgb(160, 170, 185);

            lnkGithub.LinkColor = Color.FromArgb(140, 200, 255);
            lnkGithub.ActiveLinkColor = Color.White;
            lnkGithub.VisitedLinkColor = Color.FromArgb(140, 200, 255);
            lnkGithub.BackColor = Color.Transparent;

            StyleInvisibleButton(btnEasterEgg, Color.FromArgb(30, 33, 42));

            label1.ForeColor = Color.FromArgb(210, 210, 210);
            label2.ForeColor = Color.FromArgb(210, 210, 210);
            label3.ForeColor = Color.FromArgb(210, 210, 210);
            label4.ForeColor = Color.FromArgb(210, 210, 210);

            StyleCheckBox(chkPlaylistMode);
            StyleCheckBox(chkUseArchive);

            numMaxVideos.BackColor = Color.FromArgb(35, 38, 47);
            numMaxVideos.ForeColor = Color.White;

            lblVideoTitle.ForeColor = Color.White;
            lblVideoTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            lblVideoInfo.ForeColor = Color.FromArgb(160, 170, 185);
            lblProgress.ForeColor = Color.FromArgb(140, 200, 255);

            StyleTextBox(txtUrl);
            StyleTextBox(txtFolder);

            txtLog.BackColor = Color.FromArgb(17, 18, 22);
            txtLog.ForeColor = Color.FromArgb(210, 210, 210);
            txtLog.BorderStyle = BorderStyle.FixedSingle;
            txtLog.Font = new Font("Consolas", 9F, FontStyle.Regular);

            cmbQuality.BackColor = Color.FromArgb(35, 38, 47);
            cmbQuality.ForeColor = Color.White;
            cmbQuality.FlatStyle = FlatStyle.Flat;

            StyleButton(btnAnalyze, Color.FromArgb(88, 101, 242));
            StyleButton(btnBrowse, Color.FromArgb(60, 64, 78));
            StyleButton(btnDownload, Color.FromArgb(46, 204, 113));
            StyleButton(btnCancel, Color.FromArgb(231, 76, 60));

            picThumbnail.BackColor = Color.FromArgb(35, 38, 47);
            picThumbnail.BorderStyle = BorderStyle.FixedSingle;

            progressDownload.Style = ProgressBarStyle.Continuous;
        }

        private void StylePanel(Panel panel)
        {
            panel.BackColor = Color.FromArgb(30, 33, 42);
        }

        private void StyleButton(Button button, Color backColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
        }

        private void StyleCheckBox(CheckBox checkBox)
        {
            checkBox.BackColor = Color.Transparent;
            checkBox.ForeColor = Color.FromArgb(210, 210, 210);
            checkBox.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
        }

        private void StyleInvisibleButton(Button button, Color backColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = backColor;
            button.FlatAppearance.MouseOverBackColor = backColor;
            button.BackColor = backColor;
            button.ForeColor = backColor;
            button.Text = string.Empty;
            button.TabStop = false;
            button.Cursor = Cursors.Default;
        }

        private void StyleTextBox(TextBox textBox)
        {
            textBox.BackColor = Color.FromArgb(35, 38, 47);
            textBox.ForeColor = Color.White;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        }

        private void ApplyRoundedCorners()
        {
            RoundControl(pnlHeader, 12);
            RoundControl(pnlInput, 12);
            RoundControl(pnlPreview, 12);
            RoundControl(pnlSettings, 12);
            RoundControl(pnlLogs, 12);

            RoundControl(btnAnalyze, 8);
            RoundControl(btnBrowse, 8);
            RoundControl(btnDownload, 10);
            RoundControl(btnCancel, 10);

            RoundControl(txtUrl, 6);
            RoundControl(txtFolder, 6);
            RoundControl(cmbQuality, 6);
            RoundControl(picThumbnail, 8);
            RoundControl(txtLog, 8);
        }

        private void RoundControl(Control control, int radius)
        {
            Rectangle bounds = new Rectangle(0, 0, control.Width, control.Height);

            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                return;
            }

            int diameter = radius * 2;

            GraphicsPath path = new GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            control.Region = new Region(path);
        }
    }
}
