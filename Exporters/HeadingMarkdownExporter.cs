using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardwhat.Models;

namespace Hardwhat.Exporters
{
    public class HeadingMarkdownExporter
    {
        private List<(string Content, MdFormat Format)> _segments;

        public HeadingMarkdownExporter()
        {
            _segments = new List<(string, MdFormat)>();
        }

        public enum MdFormat { Plain, H1, H2, Bold, NewLine, HorizontalRule}
        private void AddAsH1(string s)
        {
            _segments.Add((s, MdFormat.H1));
        }
        private void AddAsH2(string s)
        {
            _segments.Add((s, MdFormat.H2));
        }
        private void AddAsBold(string s)
        {
            _segments.Add((s, MdFormat.Bold));
        }
        //Bold 不会自动换行

        private void AddAsPlain(string s)
        {
            _segments.Add((s, MdFormat.Plain));
        }

        private void AddAsNewLine()
        {
            _segments.Add(("", MdFormat.NewLine));
        }
        private void AddAsHorizontalRule()
        {
            _segments.Add(("", MdFormat.HorizontalRule));
        }

        public string Export(HardwareReport r)
        {
            AddAsH1("Hardware Information Detection Report");
            AddAsBold($"Generated Time: {DateTime.Now:G}");
            AddAsNewLine();
            AddAsNewLine();
            AddAsHorizontalRule();

            AddAsH2("1.OS");
            AddAsBold("Manufacturer:"); AddAsPlain($" {r.OsManufacturer}");
            AddAsBold("Caption:"); AddAsPlain($" {r.OsCaption}");
            AddAsBold("Version:"); AddAsPlain($" {r.OsVersion}");
            AddAsBold("Architecture:"); AddAsPlain($" {r.OsArchitecture}");
            AddAsBold("BuildNumber:"); AddAsPlain($" {r.OsBuildNumber}");
            AddAsNewLine();
            AddAsHorizontalRule();

            AddAsH2("2.CPU");
            AddAsBold("Model:"); AddAsPlain($" {r.CpuModel}");
            AddAsBold("Cores:"); AddAsPlain($" {r.CpuCores}");
            AddAsBold("Logical Processors:"); AddAsPlain($" {r.CpuLogicalProcessors}");
            AddAsBold("Max Clock Speed:"); AddAsPlain($" {r.CpuMaxClockSpeed}");
            AddAsBold("Socket:"); AddAsPlain($" {r.CpuSocket}");
            AddAsBold("Manufacturer:"); AddAsPlain($" {r.CpuManufacturer}");
            AddAsNewLine();
            AddAsHorizontalRule();

            AddAsH2("3.GPU");
            if (r.GpuNames.Count == 0) AddAsPlain("N/A");
            else
            {
                for (int i = 0; i < r.GpuNames.Count; i++)
                {
                    AddAsBold($"GPU #{i}");
                    AddAsNewLine();
                    AddAsBold("Name:"); AddAsPlain($" {r.GpuNames[i]}");
                    AddAsBold("Memory:"); AddAsPlain($" {r.GpuMemories[i]}");
                    AddAsBold("Driver:"); AddAsPlain($" {r.GpuDrivers[i]}");
                    AddAsBold("Manufacturer:"); AddAsPlain($" {r.GpuManufacturers[i]}");
                    AddAsNewLine();
                }
            }
            AddAsHorizontalRule();

            AddAsH2("4.MEM");
            if (r.MemCapacities.Count == 0) AddAsPlain("N/A");
            else
            {
                for (int i = 0; i < r.MemCapacities.Count; i++)
                {
                    AddAsBold($"Slot #{i}");
                    AddAsNewLine();
                    AddAsBold("Capacity:"); AddAsPlain($" {r.MemCapacities[i]}");
                    AddAsBold("Manufacturer:"); AddAsPlain($" {r.MemManufacturers[i]}");
                    AddAsBold("Speed:"); AddAsPlain($" {r.MemSpeeds[i]}");
                    AddAsNewLine();
                }
            }
            AddAsHorizontalRule();

            AddAsH2("5.DISK");
            if (r.DiskCapacities.Count == 0) AddAsPlain("N/A");
            else
            {
                for (int i = 0; i < r.DiskCapacities.Count; i++)
                {
                    AddAsBold($"Disk #{i}");
                    AddAsNewLine();
                    AddAsBold("Model:"); AddAsPlain($" {r.DiskModels[i]}");
                    AddAsBold("Capacity:"); AddAsPlain($" {r.DiskCapacities[i]}");
                    AddAsBold("Type:"); AddAsPlain($" {r.DiskTypes[i]}");
                    AddAsBold("Interface:"); AddAsPlain($" {r.DiskInterfaces[i]}");
                    AddAsNewLine();
                }
            }
            AddAsHorizontalRule();

            AddAsH2("6.MOTHERBOARD");
            AddAsBold("Manufacturer:"); AddAsPlain($" {r.MbManufacturer}");
            AddAsBold("Product:"); AddAsPlain($" {r.MbProduct}");
            AddAsBold("Version:"); AddAsPlain($" {r.MbVersion}");
            AddAsBold("SerialNumber:"); AddAsPlain($" {r.MbSerialNumber}");
            AddAsNewLine();
            AddAsHorizontalRule();

            AddAsH2("7.NETWORK");
            if (r.NetNames.Count == 0) AddAsPlain("N/A");
            else
            {
                for (int i = 0; i < r.NetNames.Count; i++)
                {
                    AddAsBold($"Network Adapter #{i}");
                    AddAsNewLine();
                    AddAsBold("Name:"); AddAsPlain($" {r.NetNames[i]}");
                    AddAsBold("Manufacturer:"); AddAsPlain($" {r.NetManufacturers[i]}");
                    AddAsBold("MacAddress:"); AddAsPlain($" {r.NetMacAddresses[i]}");
                    AddAsBold("Type:"); AddAsPlain($" {r.NetTypes[i]}");
                    AddAsBold("Speed:"); AddAsPlain($" {r.NetSpeeds[i]}");
                    AddAsNewLine();
                }
            }
            AddAsHorizontalRule();

            AddAsH2("8.AUDIO");
            if (r.AudioNames.Count == 0) AddAsPlain("N/A");
            else
            {
                for (int i = 0; i < r.AudioNames.Count; i++)
                {
                    AddAsBold($"Audio Adapter #{i}");
                    AddAsNewLine();
                    AddAsBold("Name:"); AddAsPlain($" {r.AudioNames[i]}");
                    AddAsBold("Manufacturer:"); AddAsPlain($" {r.AudioManufacturers[i]}");
                    AddAsNewLine();
                }
            }
            AddAsHorizontalRule();
            AddAsNewLine();
            AddAsBold("This report was automatically generated by Hardwhat.");
            AddAsNewLine();
            AddAsPlain("[Hardwhat on GitHub](https://github.com/Easzzz/Hardwhat)");

            return Render();
        }
        private string Render()
        {
            var sb = new StringBuilder();
            foreach (var (content, format) in _segments)
            {
                switch (format)
                {
                    case MdFormat.H1:
                        sb.AppendLine($"# {content}");
                        break;

                    case MdFormat.H2:
                        sb.AppendLine($"## {content}");
                        break;

                    case MdFormat.Bold:
                        sb.Append($"**{content}**");
                        break;

                    case MdFormat.NewLine:
                        sb.Append("\n");
                        break;

                    case MdFormat.HorizontalRule:
                        sb.AppendLine("---");
                        break;

                    case MdFormat.Plain:
                    default:
                        sb.AppendLine(content);
                        break;

                }
            }
            return sb.ToString();
        }

    }
}
