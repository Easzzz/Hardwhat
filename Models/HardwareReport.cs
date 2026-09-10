using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardwhat.Models
{
    public class HardwareReport
    {

        //---Os---

        public string OsManufacturer { get; set; }
        public string OsCaption { get; set; }
        public string OsVersion { get; set; }
        public string OsArchitecture { get; set; }
        public string OsBuildNumber { get; set; }

        //---Cpu---

        public string CpuModel { get; set; }
        public string CpuCores { get; set; }
        public string CpuLogicalProcessors { get; set; }
        public string CpuMaxClockSpeed { get; set; }
        public string CpuSocket { get; set; }
        public string CpuManufacturer { get; set; }

        //---Gpu---

        //public string GpuName { get; set; }
        //public string GpuMemory { get; set; }
        //public string GpuDriver { get; set; }
        //public string GpuManufacturer { get; set; }

        //---Gpu---
        public List<string> GpuNames { get; set; } = new List<string>();
        public List<string> GpuMemories { get; set; } = new List<string>();
        public List<string> GpuDrivers { get; set; } = new List<string>();
        public List<string> GpuManufacturers { get; set; } = new List<string>();


        //---Mem---

        //public string MemManufacturer { get; set; }
        //public string MemCapacity { get; set; }
        //public string MemSpeed { get; set; }

        public List<string> MemManufacturers { get; set; } = new List<string>();
        public List<string> MemCapacities { get; set; } = new List<string>();
        public List<string> MemSpeeds { get; set; } = new List<string>();

        //---Disk---

        //public string DiskModel { get; set; }
        //public string DiskCapacity { get; set; }
        //public string DiskType { get; set; }
        //public string DiskInterface { get; set; }

        public List<string> DiskModels { get; set; } = new List<string>();
        public List<string> DiskCapacities { get; set; } = new List<string>();
        public List<string> DiskTypes { get; set; } = new List<string>();
        public List<string> DiskInterfaces { get; set; } = new List<string>();

        //---Mb---

        public string MbManufacturer { get; set; }
        public string MbProduct { get; set; }
        public string MbVersion { get; set; }
        public string MbSerialNumber { get; set; }

        //---Net---

        public List<string> NetNames { get; set; } = new List<string>();
        public List<string> NetManufacturers { get; set; } = new List<string>();
        public List<string> NetMacAddresses { get; set; } = new List<string>();
        public List<string> NetSpeeds { get; set; } = new List<string>();
        public List<string> NetTypes { get; set; } = new List<string>();


        //---Audio---

        public List<string> AudioNames { get; set; } = new List<string>();
        public List<string> AudioManufacturers { get; set; } = new List<string>();


    }
}
