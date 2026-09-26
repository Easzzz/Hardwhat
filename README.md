# Hardwhat
An WPF (.NET Framework) app for checking hardware information
---
### English | [简体中文](docs/README-zh.md)

Built on .NET Framework and uses **WMI** (Windows Management Instrumentation) to query hardware information.
This project is under GPL-3.0 **(Except sidebar and window icons)**.

![Start Page](docs/startpage.png)

**⌃ Start Page**

![Querying Page](docs/queryingpage.png)

**⌃ Query result view**

With a clean and intuitive UI, you can easily explore your:

`🪟 OS` `⚡ CPU` `🎮 GPU` `🧠 Memory` `💾 Disk` `🖥️ Motherboard` `🌐 Network` `🔊 Audio`

**Main Logic is in `MainWindow.xaml.cs` and Design is in `MainWindow.xaml`**

## Hardwhat 2.0 New Feature: Export the query result in 3 ways
Click the `Export icon`![Export icon](https://cdn.jsdelivr.net/npm/lucide-static@latest/icons/file-up.svg), you can choose as a Table-style or Heading-style `.md` or `.csv`

![Export dialog](docs/exportdialog.png)
**⌃ Export dialog**


![The exported Table-style Markdown](docs/tablemd.png)
**⌃ Table-style Markdown**

![The exported Heading-style Markdown](docs/headingmd.png)
**⌃ Heading-style Markdown**

![The exported CSV](docs/csv.png)
**⌃ CSV**


All icons are derived from [Lucide](https://lucide.dev) and converted to XAML Path, licensed under the [ISC License](docs/LICENSE-Lucide).

> 💡 *This is my second GitHub repository. Any ideas or suggestions are welcome in [Issues](https://github.com/Easzzz/Hardwhat/issues).*

> ⭐ *If you find this project useful, a star would be appreciated~*

![License](https://img.shields.io/badge/License-GPLv3-blue) ![.NET Framework|79](https://img.shields.io/badge/.NET-4.8-512BD4?logo=dotnet) ![C#](https://img.shields.io/badge/C%23-100%25-239120?logo=csharp) ![WPF](https://img.shields.io/badge/UI-WPF-0078D6?logo=.net)