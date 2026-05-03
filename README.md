# Denvia yt-dlp Downloader

Denvia yt-dlp Downloader is a simple Windows desktop GUI for yt-dlp, built with C# WinForms.

It allows users to analyze a video link, preview the title, duration and thumbnail, select an available quality, and download the content through a clean graphical interface.

## Features

- Analyze video links before downloading
- Show video title, duration and thumbnail
- Dynamic quality selection based on available formats
- Download video as MP4
- Download audio as MP3
- Live progress bar
- Live yt-dlp logs
- Cancel active downloads
- Modern dark UI
- Portable external tools support

## Download

Go to the **Releases** section and download the latest Windows release package from **Assets**.

Do not download the automatically generated `Source code` files unless you want to build the project yourself.

## Usage

1. Download the latest release package.
2. Extract the archive.
3. Run `YTDlpGui.exe`.
4. Paste a video URL.
5. Click `Analyze`.
6. Select an available quality.
7. Click `Download`.


<img width="902" height="693" alt="image" src="https://github.com/user-attachments/assets/40a258c9-f366-44b3-ac74-76708456a323" />



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
