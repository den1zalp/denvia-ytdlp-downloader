# Denvia yt-dlp Downloader

Denvia yt-dlp Downloader is a simple Windows desktop GUI for yt-dlp, built with C# WinForms.

It allows users to analyze video links, preview video information, select quality options, and download videos through a clean graphical interface.

## Features

- Analyze single video links before downloading
- Show video title, duration and thumbnail
- Dynamic quality selection for single videos
- Download videos as MP4
- Download audio as MP3
- Live progress bar
- Live yt-dlp logs
- Cancel active downloads
- Modern dark UI
- Playlist and channel download mode
- Optional archive support to skip already downloaded videos
- Portable external tools support

## Download

Go to the **Releases** section and download the latest Windows package from **Assets**:

`DenviaDownloader-v1.0.0-win-x64.zip`

Do not download the automatically generated `Source code` files unless you want to build the project manually.

## Usage

### Single Video Mode

Use this mode when you want to download one video.

1. Make sure `Playlist / Channel mode` is unchecked.
2. Paste a video URL.
3. Click `Analyze`.
4. Wait for the title, thumbnail and quality options to load.
5. Select the preferred quality.
6. Click `Download`.

In single video mode, the app analyzes the link first and loads the available quality options for that specific video.

### Playlist / Channel Mode

Use this mode when you want to download multiple videos from a playlist or channel.

1. Check `Playlist / Channel mode`.
2. Paste a playlist, channel or supported collection URL.
3. Set `Max videos`.
4. Choose the preferred quality.
5. Keep `Skip already downloaded` enabled if you want to avoid duplicate downloads.
6. Click `Download`.

In Playlist / Channel mode, the app skips the analyze step and sends the link directly to yt-dlp.

## Playlist / Channel Options

### Max videos

`Max videos` limits how many videos will be downloaded from the playlist or channel.

For example:

```text
Max videos: 3
```

This means only the first 3 videos will be downloaded.

This is useful to avoid accidentally downloading an entire channel.

### Skip already downloaded

When `Skip already downloaded` is enabled, the app creates a `download-archive.txt` file in the output folder.

This archive file allows yt-dlp to skip videos that were already downloaded before.

This is especially useful for channel downloads, because you can run the downloader again later without downloading the same videos twice.

## Output Structure

### Single Video Mode

Single videos are saved directly into the selected output folder:

```text
Output/
└─ Video title.mp4
```

### Playlist / Channel Mode

Playlist and channel downloads are organized by uploader:

```text
Output/
└─ Channel Name/
   ├─ 001 - Video title.mp4
   ├─ 002 - Video title.mp4
   └─ 003 - Video title.mp4
```

If `Skip already downloaded` is enabled, an archive file may also be created:

```text
Output/
└─ download-archive.txt
```

## Folder Structure

The application expects the `Tools` folder to be in the same directory as `YTDlpGui.exe`.

```text
DenviaDownloader/
├─ YTDlpGui.exe
└─ Tools/
   ├─ yt-dlp.exe
   ├─ ffmpeg.exe
   ├─ ffprobe.exe
   └─ deno.exe
```

Do not move `YTDlpGui.exe` away from the `Tools` folder.

## External Tools

This application uses the following external tools:

- yt-dlp
- ffmpeg
- ffprobe
- deno

These tools are not included in the source code repository. They are included only in the release package.

## Build From Source

Requirements:

- Windows
- Visual Studio
- .NET 10 SDK
- Windows Forms support

Clone the repository:

```powershell
git clone https://github.com/den1zalp/denvia-ytdlp-downloader.git
cd denvia-ytdlp-downloader
```

Build the project:

```powershell
dotnet build
```

Publish a Windows x64 build:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## Disclaimer

Denvia yt-dlp Downloader is a graphical interface for yt-dlp.

Users are responsible for respecting copyright laws, platform terms of service, and content owners' rights. This project does not encourage downloading copyrighted content without permission.

## Credits

Created by **den1zalp**

GitHub: https://github.com/den1zalp
