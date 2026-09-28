using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hardwhat.Models;

namespace Hardwhat.Exporters
{

    public class HtmlExporter
    {

        public string Export(HardwareReport r)
        {

            var sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='en-US'>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset='UTF-8' />");
            sb.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1' />");
            sb.AppendLine("<title>Hardware Information Detection Report</title>");
            sb.AppendLine(Styles);
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("");
            sb.AppendLine("<div class='app'>");

            sb.AppendLine(SidebarHtml);
            sb.AppendLine("<div class='divider' aria-hidden='true'></div>");
            sb.AppendLine("<main class='main'>");

            AppendOsSection(sb, r);
            AppendCpuSection(sb, r);
            AppendGpuSection(sb, r);
            AppendMemSection(sb, r);
            AppendDiskSection(sb, r);
            AppendMbSection(sb, r);
            AppendNetSection(sb, r);
            AppendAudioSection(sb, r);

            sb.AppendLine("</main>");
            sb.AppendLine("</div>");    // <div class='app'> 的结束
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }



        private static void Title(StringBuilder sb, string id, string text)
        {
            sb.Append("  <h2 class='section-title' id='")
              .Append(id).Append("'>")
              .Append(HtmlEscape(text))
              .AppendLine("</h2>");
        }

        private static void Pair(StringBuilder sb, string key, string value)
        {
            sb.Append("    <span class='k'>").Append(HtmlEscape(key)).AppendLine("</span>");
            sb.Append("    <span class='v").Append("'>").Append(HtmlEscape(value)).AppendLine("</span>");
        }

        private static void BeginCard(StringBuilder sb)
        {
            sb.AppendLine("  <div class='card'>");
        }

        private static void EndCard(StringBuilder sb)
        {
            sb.AppendLine("  </div>");
        }

        private static void AppendOsSection(StringBuilder sb, HardwareReport r)
        {
            Title(sb, "os", "Operating System");

            BeginCard(sb);
            Pair(sb, "Manufacturer", r.OsManufacturer);
            Pair(sb, "Caption", r.OsCaption);
            Pair(sb, "Version", r.OsVersion);
            Pair(sb, "Architecture", r.OsArchitecture);
            Pair(sb, "Build Number", r.OsBuildNumber);
            EndCard(sb);
        }

        private static void AppendCpuSection(StringBuilder sb, HardwareReport r)
        {
            Title(sb, "cpu", "CPU");

            BeginCard(sb);
            Pair(sb, "Model", r.CpuModel);
            Pair(sb, "Cores", r.CpuCores);
            Pair(sb, "Logical Processors", r.CpuLogicalProcessors);
            Pair(sb, "Max Clock Speed", r.CpuMaxClockSpeed);
            Pair(sb, "Socket", r.CpuSocket);
            Pair(sb, "Manufacturer", r.CpuManufacturer);
            EndCard(sb);
        }

        private static void AppendGpuSection(StringBuilder sb, HardwareReport r)
        {
            Title(sb, "gpu", "GPU");

            int n = MaxCount(r.GpuNames, r.GpuMemories, r.GpuDrivers, r.GpuManufacturers);
            for (int i = 0; i < n; i++)
            {
                BeginCard(sb);
                Pair(sb, "#", $"GPU #{i}");
                Pair(sb, "Name", At(r.GpuNames, i));
                Pair(sb, "Memory", At(r.GpuMemories, i));
                Pair(sb, "Driver", At(r.GpuDrivers, i));
                Pair(sb, "Manufacturer", At(r.GpuManufacturers, i));
                EndCard(sb);
            }
        }

        private static void AppendMemSection(StringBuilder sb, HardwareReport r)
        {
            Title(sb, "mem", "Memory");

            BeginCard(sb);
            Pair(sb, "Total Capacity", TotalCapacity(r.MemCapacities));
            Pair(sb, "Configured Speed", MinSpeed(r.MemSpeeds));
            EndCard(sb);

            int n = r.MemManufacturers.Count;
            for (int i = 0; i < n; i++)
            {
                BeginCard(sb);
                Pair(sb, "#", $"Slot #{i}");
                Pair(sb, "Manufacturer", At(r.MemManufacturers, i));
                Pair(sb, "Capacity", At(r.MemCapacities, i));
                Pair(sb, "MemSpeed", At(r.MemSpeeds, i));
                EndCard(sb);
            }
        }

        private static void AppendDiskSection(StringBuilder sb, HardwareReport r)
        {
            Title(sb, "disk", "Disk");

            int n = MaxCount(r.DiskModels, r.DiskCapacities, r.DiskTypes, r.DiskInterfaces);
            for (int i = 0; i < n; i++)
            {
                BeginCard(sb);
                Pair(sb, "#", $"Disk #{i}");
                Pair(sb, "Model", At(r.DiskModels, i));
                Pair(sb, "Capacity", At(r.DiskCapacities, i));
                Pair(sb, "Type", At(r.DiskTypes, i));
                Pair(sb, "Interface", At(r.DiskInterfaces, i));
                EndCard(sb);
            }
        }

        private static void AppendMbSection(StringBuilder sb, HardwareReport r)
        {
            Title(sb, "mb", "Mainboard");

            BeginCard(sb);
            Pair(sb, "Manufacturer", r.MbManufacturer);
            Pair(sb, "Product", r.MbProduct);
            Pair(sb, "Version", r.MbVersion);
            Pair(sb, "Serial Number", r.MbSerialNumber);
            EndCard(sb);
        }

        private static void AppendNetSection(StringBuilder sb, HardwareReport r)
        {
            Title(sb, "net", "Network");

            int n = MaxCount(r.NetNames, r.NetManufacturers, r.NetMacAddresses,
                             r.NetSpeeds, r.NetTypes);
            for (int i = 0; i < n; i++)
            {
                BeginCard(sb);
                Pair(sb, "#", $"Network Adapter #{i}");
                Pair(sb, "Name", At(r.NetNames, i));
                Pair(sb, "Manufacturer", At(r.NetManufacturers, i));
                Pair(sb, "MAC Address", At(r.NetMacAddresses, i));
                Pair(sb, "Speed", At(r.NetSpeeds, i));
                Pair(sb, "Type", At(r.NetTypes, i));
                EndCard(sb);
            }
        }

        private static void AppendAudioSection(StringBuilder sb, HardwareReport r)
        {
            Title(sb, "audio", "Audio");

            int n = MaxCount(r.AudioNames, r.AudioManufacturers);
            for (int i = 0; i < n; i++)
            {
                BeginCard(sb);
                Pair(sb, "#", $"Audio Adapter #{i}");
                Pair(sb, "Name", At(r.AudioNames, i));
                Pair(sb, "Manufacturer", At(r.AudioManufacturers, i));
                EndCard(sb);
            }
        }



        private static string At(List<string> list, int i)
        {
            if (list == null || i < 0 || i >= list.Count) return "";
            return list[i] ?? "";
        }


        private static int MaxCount(params List<string>[] lists)
        {
            int max = 0;
            if (lists == null) return 0;
            foreach (var l in lists)
                if (l != null && l.Count > max) max = l.Count;
            return max;
        }

        private static string MinSpeed(List<string> list)
        {
            if (list == null || list.Count == 0) return "";

            int min = int.MaxValue;
            string result = "";

            foreach (string speed in list)
            {
                if (string.IsNullOrEmpty(speed)) continue;

                string[] parts = speed.Split(' ');
                if (parts.Length < 2) continue;

                int v;
                if (!int.TryParse(parts[0], out v)) continue;

                if (v < min)
                {
                    min = v;
                    result = speed;
                }
            }

            return result;
        }

        // 把 "8 GB" 之类的容量字符串求和，输出如 "32 GB (4 × 8 GB)"
        private static string TotalCapacity(List<string> capacities)
        {
            if (capacities == null || capacities.Count == 0) return "";

            double sum = 0;
            string unit = null;
            bool allOk = true;

            foreach (var c in capacities)
            {
                if (string.IsNullOrEmpty(c)) { allOk = false; break; }
                var parts = c.Split(' ');
                double v;
                if (parts.Length < 2 || !double.TryParse(parts[0], out v))
                {
                    allOk = false; break;
                }
                string u = parts[1].ToUpperInvariant();
                if (unit == null) unit = parts[1];
                else if (!string.Equals(unit, parts[1], StringComparison.OrdinalIgnoreCase))
                {
                    allOk = false; break;
                }
                sum += v;
            }

            if (!allOk) return string.Join(" + ", capacities);

            return string.Format("{0:0.##} {1} ({2} × {3} {1})",
                sum, unit, capacities.Count, capacities[0].Split(' ')[0]);
        }


        // 处理转义
        private static string HtmlEscape(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            var sb = new StringBuilder(s.Length + 8); // 预留 8 字节
            foreach (char c in s)
            {
                switch (c)
                {
                    case '&': sb.Append("&amp;"); break;
                    case '<': sb.Append("&lt;"); break;
                    case '>': sb.Append("&gt;"); break;
                    case '"': sb.Append("&quot;"); break;
                    case '\'': sb.Append("&#39;"); break;
                    default: sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        // @ 用单引号不用转义  
        private const string Styles = @"
<style>
    :root
    {
        --bg:#0F1117; --sidebar-top:#181C26; --sidebar-bottom:#0F1117;
        --card:#181C26; --card-border:#2E3548;
        --hover:#282F40; --hover-border:#3E4862;
        --pressed:#333843; --pressed-border:#495165;
        --text:#E2E6ED; --label:#8A90A2;
        --icon:#8A90A2; --icon-hover:#E2E6ED; --icon-pressed:#FFFFFF;
        --scroll-track:#181C26; --scroll-thumb:#282F40; --scroll-thumb-bd:#8A90A2;
    }

    *, *::before, *::after { box-sizing: border-box; }

    html, body { height: 100%; margin: 0; }

    body
    {
        background: var(--bg); color: var(--text);
        font-family: 'Microsoft Sans Serif','Segoe UI','PingFang SC','Microsoft YaHei',system-ui,sans-serif;
        font-size: 14px; line-height: 1.45;
        -webkit-font-smoothing: antialiased; overflow: hidden;
    }

    .app
    {
        display: grid; grid-template-columns: 50px 4px 1fr; height: 100vh; overflow: hidden;
    }

    .sidebar
    {
        display: flex; flex-direction: column; align-items: center; padding-top: 12px;
        background: linear-gradient(180deg, var(--sidebar-top) 20.4%, var(--sidebar-bottom) 100%);
    }

    .icon-btn
    {
    width: 38px; height: 38px; margin: 3px 0 0 2px; padding: 0;
    border: 0; border-radius: 6px; background: transparent; color: var(--icon);
    cursor: pointer; display: inline-flex; align-items: center; justify-content: center;
    text-decoration: none; transition: background .15s ease, color .15s ease;
    -webkit-tap-highlight-color: transparent;
    }

    .icon-btn svg { width: 22px; height: 22px; display: block; }
    .icon-btn:hover  { background: var(--hover);   color: var(--icon-hover); }
    .icon-btn:active { background: var(--pressed); color: var(--icon-pressed); }
    .icon-btn:focus-visible { outline: 1px solid var(--hover-border); outline-offset: 1px; }

    .divider { background: linear-gradient(180deg, var(--hover) 20.4%, var(--sidebar-bottom) 100%); }

    .main { overflow-y: auto; overflow-x: hidden; padding: 4px 0 40px; scroll-behavior: smooth; }
    .main::-webkit-scrollbar { width: 8px; }
    .main::-webkit-scrollbar-track { background: var(--scroll-track); border-radius: 5px; }
    .main::-webkit-scrollbar-thumb { background: var(--scroll-thumb); border: 1px solid var(--scroll-thumb-bd); border-radius: 5px; }
    .main { scrollbar-width: auto; scrollbar-color: var(--scroll-thumb) var(--scroll-track); }

    .section-title
    {
        margin: 15px 0 10px 35px;
        font-family: Consolas,'Cascadia Mono',ui-monospace,monospace;
        font-size: 28px; font-weight: 700; color: var(--text);
        letter-spacing: .01em; scroll-margin-top: 10px;
    }

    .card
    {
        display: grid; grid-template-columns: 180px 1fr;
        column-gap: 16px; row-gap: 10px;
        margin: 0 50px 20px 30px; padding: 20px 25px;
        background: var(--card); border: 1px solid var(--card-border); border-radius: 16px;
    }
    .card .k { color: var(--label); font-size: 14px; min-width: 0; }
    .card .v { color: var(--text);  font-size: 14px; min-width: 0;
             word-break: break-word; overflow-wrap: anywhere; }


    @media (max-width: 720px)
    {
        .app { grid-template-columns: 44px 3px 1fr; }
        .icon-btn { width: 34px; height: 34px; margin-left: 1px; }
        .icon-btn svg { width: 19px; height: 19px; }
        .section-title { margin-left: 18px; font-size: 22px; }
        .card { grid-template-columns: 1fr; row-gap: 2px; margin: 0 16px 16px 16px; padding: 16px 18px; }
        .card .k { margin-top: 8px; font-size: 12px; letter-spacing: .02em; }
        .card .v { font-size: 13.5px; }
    }

</style>
";

        private const string SidebarHtml = @"
<aside class='sidebar' aria-label='Hardware categories'>
  <a class='icon-btn' href='#os' title='System' aria-label='System'>
    <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.2'
         stroke-linecap='round' stroke-linejoin='round'>
      <path d='M12 17V21 M14.305 7.53L15.228 7.148 M15.228 4.852L14.305 4.469
               M16.852 3.228L16.469 2.304 M16.852 8.772L16.469 9.695
               M19.148 3.228L19.531 2.304 M19.53 9.696L19.148 8.772
               M20.772 4.852L21.696 4.469 M20.772 7.148L21.696 7.531
               M22 13V15A2 2 0 0 1 20 17H4A2 2 0 0 1 2 15V5A2 2 0 0 1 4 3H11
               M8 21H16 M15 6A3 3 0 1 1 21 6A3 3 0 1 1 15 6'/>
    </svg>
  </a>
  <a class='icon-btn' href='#cpu' title='CPU' aria-label='CPU'>
    <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.2'
         stroke-linecap='round' stroke-linejoin='round'>
      <path d='M12 20V22 M12 2V4 M17 20V22 M17 2V4 M2 12H4 M2 17H4 M2 7H4
               M20 12H22 M20 17H22 M20 7H22 M7 20V22 M7 2V4
               M6 4H18A2 2 0 0 1 20 6V18A2 2 0 0 1 18 20H6A2 2 0 0 1 4 18V6A2 2 0 0 1 6 4Z
               M9 8H15A1 1 0 0 1 16 9V15A1 1 0 0 1 15 16H9A1 1 0 0 1 8 15V9A1 1 0 0 1 9 8Z'/>
    </svg>
  </a>
  <a class='icon-btn' href='#gpu' title='GPU' aria-label='GPU'>
    <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.2'
         stroke-linecap='round' stroke-linejoin='round'>
      <path d='M2 17H20A2 2 0 0 0 22 15V7A2 2 0 0 0 20 5H2 M2 21V3
               M7 17V20A1 1 0 0 0 8 21H13A1 1 0 0 0 14 20V17
               M14 11A2 2 0 1 1 18 11A2 2 0 1 1 14 11
               M6 11A2 2 0 1 1 10 11A2 2 0 1 1 6 11'/>
    </svg>
  </a>
  <a class='icon-btn' href='#mem' title='Memory' aria-label='Memory'>
    <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.2'
         stroke-linecap='round' stroke-linejoin='round'>
      <path d='M12 12V10 M12 18V16 M16 12V10 M16 18V16 M2 11H3.5
               M20 18V16 M20.5 11H22 M4 18V16 M8 12V10 M8 18V16
               M4 6H20A2 2 0 0 1 22 8V14A2 2 0 0 1 20 16H4A2 2 0 0 1 2 14V8A2 2 0 0 1 4 6Z'/>
    </svg>
  </a>
  <a class='icon-btn' href='#disk' title='Disk' aria-label='Disk'>
    <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.2'
         stroke-linecap='round' stroke-linejoin='round'>
      <path d='M6 16H6.01 M10 16H10.01
               M5.45 5.11L2.212 11.577A2 2 0 0 0 2 12.473V18A2 2 0 0 0 4 20H20
               A2 2 0 0 0 22 18V12.473A2 2 0 0 0 21.788 11.577L18.55 5.11
               A2 2 0 0 0 16.76 4H7.24A2 2 0 0 0 5.45 5.11Z
               M2.054 12.013H21.946'/>
    </svg>
  </a>
  <a class='icon-btn' href='#mb' title='Mainboard' aria-label='Mainboard'>
    <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.2'
         stroke-linecap='round' stroke-linejoin='round'>
      <path d='M5 3H19A2 2 0 0 1 21 5V19A2 2 0 0 1 19 21H5A2 2 0 0 1 3 19V5A2 2 0 0 1 5 3Z
               M11 9H15A2 2 0 0 0 17 7V3
               M7 9A2 2 0 1 1 11 9A2 2 0 1 1 7 9
               M7 21V17A2 2 0 0 1 9 15H13
               M13 15A2 2 0 1 1 17 15A2 2 0 1 1 13 15'/>
    </svg>
  </a>
  <a class='icon-btn' href='#net' title='Network' aria-label='Network'>
    <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.2'
         stroke-linecap='round' stroke-linejoin='round'>
      <path d='M17 16H22V22H17V16Z M3 16H8V22H3V16Z M10 2H15V8H10V2Z
               M5 16V13A1 1 0 0 1 6 12H18A1 1 0 0 1 19 13V16 M12 12V8'/>
    </svg>
  </a>
  <a class='icon-btn' href='#audio' title='Audio' aria-label='Audio'>
    <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.2'
         stroke-linecap='round' stroke-linejoin='round'>
      <path d='M11 4.702A0.705 0.705 0 0 0 9.797 4.204L6.413 7.587
               A1.4 1.4 0 0 1 5.416 8H3A1 1 0 0 0 2 9V15A1 1 0 0 0 3 16H5.416
               A1.4 1.4 0 0 1 6.413 16.413L9.796 19.797A0.705 0.705 0 0 0 11 19.298V4.702Z
               M16 9A5 5 0 0 1 16 15 M19.364 18.364A9 9 0 0 0 19.364 5.636'/>
    </svg>
  </a>
</aside>
";
    }
}