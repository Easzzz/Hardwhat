using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardwhat.Models;

namespace Hardwhat.Exporters
{
    public class TableMarkdownExporter
    {
        public string Export(HardwareReport r)
        {
            var sb = new StringBuilder();

            sb.AppendLine("# Hardware Information Detection Report");
            sb.AppendLine($"**Generated Time: {DateTime.Now:G}**");
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // 1. OS
            sb.AppendLine("## 1.OS");
            sb.AppendLine();
            sb.AppendLine("| Manufacturer | Caption | Version | Architecture | BuildNumber |");
            sb.AppendLine("| --- | --- | --- | --- | --- |");
            sb.AppendLine($"| {Esc(r.OsManufacturer)} | {Esc(r.OsCaption)} | {Esc(r.OsVersion)} | {Esc(r.OsArchitecture)} | {Esc(r.OsBuildNumber)} |");
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // 2. CPU
            sb.AppendLine("## 2.CPU");
            sb.AppendLine();
            sb.AppendLine("| Model | Cores | Logical Processors | Max Clock Speed | Socket | Manufacturer |");
            sb.AppendLine("| --- | ---: | ---: | --- | --- | --- |");
            sb.AppendLine($"| {Esc(r.CpuModel)} | {Esc(r.CpuCores)} | {Esc(r.CpuLogicalProcessors)} | {Esc(r.CpuMaxClockSpeed)} | {Esc(r.CpuSocket)} | {Esc(r.CpuManufacturer)} |");
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // 3. GPU
            sb.AppendLine("## 3.GPU");
            sb.AppendLine();
            if (r.GpuNames.Count == 0)
            {
                sb.AppendLine("N/A");
            }
            else
            {
                sb.AppendLine("| # | Name | Memory | Driver | Manufacturer |");
                sb.AppendLine("| --- | --- | --- | --- | --- |");
                for (int i = 0; i < r.GpuNames.Count; i++)
                {
                    sb.AppendLine($"| GPU #{i} | {Esc(r.GpuNames[i])} | {Esc(r.GpuMemories[i])} | {Esc(r.GpuDrivers[i])} | {Esc(r.GpuManufacturers[i])} |");
                }
            }
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // 4. MEM
            sb.AppendLine("## 4.MEM");
            sb.AppendLine();
            if (r.MemCapacities.Count == 0)
            {
                sb.AppendLine("N/A");
            }
            else
            {
                sb.AppendLine("| Slot | Capacity | Manufacturer | Speed |");
                sb.AppendLine("| --- | ---: | --- | ---: |");
                for (int i = 0; i < r.MemCapacities.Count; i++)
                {
                    sb.AppendLine($"| Slot #{i} | {Esc(r.MemCapacities[i])} | {Esc(r.MemManufacturers[i])} | {Esc(r.MemSpeeds[i])} |");
                }
            }
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // 5. DISK
            sb.AppendLine("## 5.DISK");
            sb.AppendLine();
            if (r.DiskCapacities.Count == 0)
            {
                sb.AppendLine("N/A");
            }
            else
            {
                sb.AppendLine("| Disk | Model | Capacity | Type | Interface |");
                sb.AppendLine("| --- | --- | ---: | --- | --- |");
                for (int i = 0; i < r.DiskCapacities.Count; i++)
                {
                    sb.AppendLine($"| Disk #{i} | {Esc(r.DiskModels[i])} | {Esc(r.DiskCapacities[i])} | {Esc(r.DiskTypes[i])} | {Esc(r.DiskInterfaces[i])} |");
                }
            }
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // 6. MOTHERBOARD
            sb.AppendLine("## 6.MOTHERBOARD");
            sb.AppendLine();
            sb.AppendLine("| Manufacturer | Product | Version | SerialNumber |");
            sb.AppendLine("| --- | --- | --- | --- |");
            sb.AppendLine($"| {Esc(r.MbManufacturer)} | {Esc(r.MbProduct)} | {Esc(r.MbVersion)} | {Esc(r.MbSerialNumber)} |");
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // 7. NETWORK
            sb.AppendLine("## 7.NETWORK");
            sb.AppendLine();
            if (r.NetNames.Count == 0)
            {
                sb.AppendLine("N/A");
            }
            else
            {
                sb.AppendLine("| # | Name | Manufacturer | MacAddress | Type | Speed |");
                sb.AppendLine("| --- | --- | --- | --- | --- | ---: |");
                for (int i = 0; i < r.NetNames.Count; i++)
                {
                    sb.AppendLine($"| Network Adapter #{i} | {Esc(r.NetNames[i])} | {Esc(r.NetManufacturers[i])} | {Esc(r.NetMacAddresses[i])} | {Esc(r.NetTypes[i])} | {Esc(r.NetSpeeds[i])} |");
                }
            }
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // 8. AUDIO
            sb.AppendLine("## 8.AUDIO");
            sb.AppendLine();
            if (r.AudioNames.Count == 0)
            {
                sb.AppendLine("N/A");
            }
            else
            {
                sb.AppendLine("| # | Name | Manufacturer |");
                sb.AppendLine("| --- | --- | --- |");
                for (int i = 0; i < r.AudioNames.Count; i++)
                {
                    sb.AppendLine($"| Audio Adapter #{i} | {Esc(r.AudioNames[i])} | {Esc(r.AudioManufacturers[i])} |");
                }
            }
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();
            sb.AppendLine("**This report was automatically generated by Hardwhat.**");
            sb.AppendLine();
            sb.AppendLine("[Hardwhat on GitHub](https://github.com/Easzzz/Hardwhat)");

            return sb.ToString();
        }

        //竖线 | 转义为 \|
        //换行替换为 <br>
        private string Esc(string s)
        {
            if (string.IsNullOrEmpty(s)) return "N/A";
            return s.Replace("\\", "\\\\")
                    .Replace("|", "\\|")
                    .Replace("\r\n", "<br>")
                    .Replace("\n", "<br>")
                    .Replace("\r", "<br>");
        }
    }
}