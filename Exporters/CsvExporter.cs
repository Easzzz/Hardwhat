using System;
using System.Text;
using Hardwhat.Models;

namespace Hardwhat.Exporters
{
    public class CsvExporter
    {
        public string Export(HardwareReport r)
        {
            var sb = new StringBuilder();

            // 表头：固定 4 列
            sb.AppendLine("Section");

            // 1. OS
            AppendRow(sb, "OS", "", "Manufacturer", r.OsManufacturer);
            AppendRow(sb, "OS", "", "Caption", r.OsCaption);
            AppendRow(sb, "OS", "", "Version", r.OsVersion);
            AppendRow(sb, "OS", "", "Architecture", r.OsArchitecture);
            AppendRow(sb, "OS", "", "BuildNumber", r.OsBuildNumber);
            AppendRow(sb, "", "", "", "");

            // 2. CPU
            AppendRow(sb, "CPU", "", "Model", r.CpuModel);
            AppendRow(sb, "CPU", "", "Cores", r.CpuCores);
            AppendRow(sb, "CPU", "", "LogicalProcessors", r.CpuLogicalProcessors);
            AppendRow(sb, "CPU", "", "MaxClockSpeed", r.CpuMaxClockSpeed);
            AppendRow(sb, "CPU", "", "Socket", r.CpuSocket);
            AppendRow(sb, "CPU", "", "Manufacturer", r.CpuManufacturer);
            AppendRow(sb, "", "", "", "");

            // 3. GPU
            for (int i = 0; i < r.GpuNames.Count; i++)
            {
                AppendRow(sb, "GPU", $"#{i}", "Name", r.GpuNames[i]);
                AppendRow(sb, "GPU", $"#{i}", "Memory", r.GpuMemories[i]);
                AppendRow(sb, "GPU", $"#{i}", "Driver", r.GpuDrivers[i]);
                AppendRow(sb, "GPU", $"#{i}", "Manufacturer", r.GpuManufacturers[i]);
                AppendRow(sb, "", "", "", "");
            }

            // 4. MEM
            for (int i = 0; i < r.MemCapacities.Count; i++)
            {
                AppendRow(sb, "MEM", $"#{i}", "Capacity", r.MemCapacities[i]);
                AppendRow(sb, "MEM", $"#{i}", "Manufacturer", r.MemManufacturers[i]);
                AppendRow(sb, "MEM", $"#{i}", "Speed", r.MemSpeeds[i]);
                AppendRow(sb, "", "", "", "");
            }

            // 5. DISK
            for (int i = 0; i < r.DiskCapacities.Count; i++)
            {
                AppendRow(sb, "DISK", $"#{i}", "Model", r.DiskModels[i]);
                AppendRow(sb, "DISK", $"#{i}", "Capacity", r.DiskCapacities[i]);
                AppendRow(sb, "DISK", $"#{i}", "Type", r.DiskTypes[i]);
                AppendRow(sb, "DISK", $"#{i}", "Interface", r.DiskInterfaces[i]);
                AppendRow(sb, "", "", "", "");
            }

            // 6. MOTHERBOARD
            AppendRow(sb, "MOTHERBOARD", "", "Manufacturer", r.MbManufacturer);
            AppendRow(sb, "MOTHERBOARD", "", "Product", r.MbProduct);
            AppendRow(sb, "MOTHERBOARD", "", "Version", r.MbVersion);
            AppendRow(sb, "MOTHERBOARD", "", "SerialNumber", r.MbSerialNumber);
            AppendRow(sb, "", "", "", "");

            // 7. NETWORK
            for (int i = 0; i < r.NetNames.Count; i++)
            {
                AppendRow(sb, "NETWORK", $"#{i}", "Name", r.NetNames[i]);
                AppendRow(sb, "NETWORK", $"#{i}", "Manufacturer", r.NetManufacturers[i]);
                AppendRow(sb, "NETWORK", $"#{i}", "MacAddress", r.NetMacAddresses[i]);
                AppendRow(sb, "NETWORK", $"#{i}", "Type", r.NetTypes[i]);
                AppendRow(sb, "NETWORK", $"#{i}", "Speed", r.NetSpeeds[i]);
                AppendRow(sb, "", "", "", "");
            }

            // 8. AUDIO
            for (int i = 0; i < r.AudioNames.Count; i++)
            {
                AppendRow(sb, "AUDIO", $"#{i}", "Name", r.AudioNames[i]);
                AppendRow(sb, "AUDIO", $"#{i}", "Manufacturer", r.AudioManufacturers[i]);
                AppendRow(sb, "", "", "", "");
            }

            return sb.ToString();
        }

        // 写入一行 CSV
        private void AppendRow(StringBuilder sb, string section, string index, string field, string value)
        {
            sb.AppendLine($"{Esc(section)},{Esc(index)},{Esc(field)},{Esc(value)}");
        }

        // CSV 转义规则：
        // 1. 空值输出 N/A，和 TableMarkdownExporter 保持一致
        // 2. 含有逗号、双引号、换行的字段，整体用双引号包起来
        // 3. 字段里的双引号变成两个双引号
        private string Esc(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";

            if (s.Contains(",") || s.Contains("\"") || s.Contains("\n") || s.Contains("\r"))
            {
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            }
            return s;
        }
    }
}