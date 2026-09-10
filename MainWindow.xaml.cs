using Hardwhat.Exporters;
using Hardwhat.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace Hardwhat
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _hardwareReport = new HardwareReport();
            string userName = string.Empty;
            userName = Environment.UserName;
            Welcome.Content = "Welcome! " + userName;
        }

        private HardwareReport _hardwareReport;

        private async void ToSearch_Click(object sender, RoutedEventArgs e)
        {
            ToSearch.Visibility = Visibility.Collapsed;
            SidebarBorder.Visibility = Visibility.Visible;
            DividingLine.Visibility = Visibility.Visible;
            MainScroll.Visibility = Visibility.Visible;
            Welcome.Visibility = Visibility.Collapsed;
            Export.Visibility = Visibility.Visible;
            await LoadAllHardwareAsync();
        }

        private async Task LoadAllHardwareAsync()
        {
            var tasks = new List<Task>
            {
                Task.Run(() => LoadOsInfo()),
                Task.Run(() => LoadCpuInfo()),
                Task.Run(() => LoadGpuInfo()),
                Task.Run(() => LoadMemInfo()),
                Task.Run(() => LoadDiskInfo()),
                Task.Run(() => LoadMainboardInfo()),
                Task.Run(() => LoadNetworkInfo()),
                Task.Run(() => LoadAudioInfo()),
            };
            await Task.WhenAll(tasks); // 并行查询
        }

        private void ScrollToElement(FrameworkElement target)
        {
            // 将目标元素坐标转换为相对于 MainScroll 的坐标
            GeneralTransform transform = target.TransformToVisual(MainScroll);
            Point position = transform.Transform(new Point(0, 0));

            // 获取当前的垂直偏移，加上目标位置，减去 20px 留白
            double targetOffset = MainScroll.VerticalOffset + position.Y - 20;

            // 确保不超出滚动范围
            targetOffset = Math.Max(0, Math.Min(targetOffset, MainScroll.ScrollableHeight));

            MainScroll.ScrollToVerticalOffset(targetOffset);
        }

        // 各按钮的 Click 事件
        private void BtnOS_Click(object sender, RoutedEventArgs e) => ScrollToElement(LblOS);
        private void BtnCPU_Click(object sender, RoutedEventArgs e) => ScrollToElement(LblCPU);
        private void BtnGPU_Click(object sender, RoutedEventArgs e) => ScrollToElement(LblGPU);
        private void BtnMEM_Click(object sender, RoutedEventArgs e) => ScrollToElement(LblMEM);
        private void BtnDISK_Click(object sender, RoutedEventArgs e) => ScrollToElement(LblDISK);
        private void BtnMB_Click(object sender, RoutedEventArgs e) => ScrollToElement(LblMB);
        private void BtnNET_Click(object sender, RoutedEventArgs e) => ScrollToElement(LblNET);
        private void BtnAUDIO_Click(object sender, RoutedEventArgs e) => ScrollToElement(LblAUDIO);

        private void LoadOsInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string caption = obj["Caption"]?.ToString() ?? "N/A";
                        string version = obj["Version"]?.ToString() ?? "N/A";
                        string architecture = obj["OSArchitecture"]?.ToString() ?? "N/A";
                        string buildNumber = obj["BuildNumber"]?.ToString() ?? "N/A";

                        // Manufacturer 从 Win32_ComputerSystem 获取
                        string manufacturer = "N/A";
                        try
                        {
                            using (var csSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
                            {
                                foreach (ManagementObject csObj in csSearcher.Get())
                                {
                                    manufacturer = csObj["Manufacturer"]?.ToString() ?? "N/A";
                                    break;
                                }
                            }
                        }
                        catch { }

                        _hardwareReport.OsManufacturer = manufacturer;
                        _hardwareReport.OsCaption = caption;
                        _hardwareReport.OsVersion = version;
                        _hardwareReport.OsArchitecture = architecture;
                        _hardwareReport.OsBuildNumber = buildNumber;

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            TxtOsManufacturer.Text = manufacturer;
                            TxtOsCaption.Text = caption;
                            TxtOsVersion.Text = version;
                            TxtOsArchitecture.Text = architecture;
                            TxtOsBuildNumber.Text = buildNumber;
                        }));

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TxtOsCaption.Text = "获取失败: " + ex.Message;
                }));
            }
        }

        private void LoadCpuInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string CpuModel = obj["Name"]?.ToString() ?? "N/A";
                        string cores = obj["NumberOfCores"]?.ToString() ?? "N/A";
                        string threads = obj["NumberOfLogicalProcessors"]?.ToString() ?? "N/A";
                        uint maxClock = (uint)(obj["MaxClockSpeed"] ?? 0);
                        string clock = $"{maxClock / 1000.0:F2} GHz";
                        string socket = obj["SocketDesignation"]?.ToString() ?? "N/A";
                        string manufacturer = obj["Manufacturer"]?.ToString() ?? "N/A";

                        _hardwareReport.CpuModel = CpuModel;
                        _hardwareReport.CpuCores = cores;
                        _hardwareReport.CpuLogicalProcessors = threads;
                        _hardwareReport.CpuMaxClockSpeed = clock;
                        _hardwareReport.CpuSocket = socket;
                        _hardwareReport.CpuManufacturer = manufacturer;

                        // 更新 UI（使用 BeginInvoke 避免卡顿）
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            TxtCpuModel.Text = CpuModel;
                            TxtCpuCores.Text = cores;
                            TxtCpuThreads.Text = threads;
                            TxtCpuClock.Text = clock;
                            TxtCpuSocket.Text = socket;
                            TxtCpuManufacturer.Text = manufacturer;
                        }));

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TxtCpuModel.Text = "获取失败: " + ex.Message;
                }));
            }
        }

        private void LoadGpuInfo()
        {

            _hardwareReport.GpuNames.Clear();
            _hardwareReport.GpuMemories.Clear();
            _hardwareReport.GpuDrivers.Clear();
            _hardwareReport.GpuManufacturers.Clear();

            var gpus = new List<(string Name, string Memory, string Driver, string Manufacturer)>();
            try
            {
                using (ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
                {

                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string manufacturer = "N/A";
                        try
                        {
                            manufacturer = obj["AdapterCompatibility"]?.ToString() ?? "N/A";
                        }
                        catch (ManagementException) { }

                        // 过滤非 N/A/I 厂商
                        // string manuUpper = manufacturer.ToUpper();
                        // if (!(manuUpper.Contains("NVIDIA") || manuUpper.Contains("AMD") || manuUpper.Contains("INTEL")))
                        //     continue;

                        string name = obj["Name"]?.ToString() ?? "N/A";
                        string ram = "N/A";
                        try
                        {
                            object ramObj = obj["AdapterRAM"];
                            if (ramObj != null)
                            {
                                ulong ramBytes = Convert.ToUInt64(ramObj);
                                ram = $"{ramBytes / 1073741824.0:F2} GB";
                            }
                        }
                        catch { }

                        string driver = obj["DriverVersion"]?.ToString() ?? "N/A";

                        // 拼接
                        //if (allName.Length > 0) allName += "\n";
                        //allName += name;

                        //if (allRam.Length > 0) allRam += "\n";
                        //allRam += ram;

                        //if (allDriver.Length > 0) allDriver += "\n";
                        //allDriver += driver;

                        //if (allManufacturer.Length > 0) allManufacturer += "\n";
                        //allManufacturer += manufacturer;

                        gpus.Add((name, ram, driver, manufacturer));
                    }

                    // 循环外，从头遍历 gpus 拼字符串
                    string allName = "";
                    string allRam = "";
                    string allDriver = "";
                    string allManufacturer = "";

                    foreach (var gpu in gpus)
                    {
                        if (allName.Length > 0) allName += "\n";
                        allName += gpu.Name;

                        if (allRam.Length > 0) allRam += "\n";
                        allRam += gpu.Memory;

                        if (allDriver.Length > 0) allDriver += "\n";
                        allDriver += gpu.Driver;

                        if (allManufacturer.Length > 0) allManufacturer += "\n";
                        allManufacturer += gpu.Manufacturer;

                        _hardwareReport.GpuNames.Add(gpu.Name);
                        _hardwareReport.GpuMemories.Add(gpu.Memory);
                        _hardwareReport.GpuDrivers.Add(gpu.Driver);
                        _hardwareReport.GpuManufacturers.Add(gpu.Manufacturer);
                    }

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TxtGpuName.Text = allName;
                        TxtGpuRam.Text = allRam;
                        TxtGpuDriver.Text = allDriver;
                        TxtGpuManufacturer.Text = allManufacturer;
                    }));
                }
            }

            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TxtGpuName.Text = "获取失败: " + ex.Message;
                }));
            }
        }

        private void LoadMemInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory"))
                {
                    // 存储所有内存条信息
                    string allManufacturer = "";
                    string allCapacity = "";
                    string allSpeed = "";

                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string manufacturer = obj["Manufacturer"]?.ToString() ?? "N/A";
                        ulong capacity = (ulong)(obj["Capacity"] ?? 0);
                        string capacityStr = (capacity / (1024 * 1024 * 1024)).ToString() + " GB";
                        string speed = (obj["Speed"]?.ToString() ?? "N/A") + " MHz";


                        // 拼接多根内存条信息（用换行分隔）
                        if (allManufacturer.Length > 0) allManufacturer += "\n";
                        allManufacturer += manufacturer;

                        if (allCapacity.Length > 0) allCapacity += "\n";
                        allCapacity += capacityStr;

                        if (allSpeed.Length > 0) allSpeed += "\n";
                        allSpeed += speed;

                        _hardwareReport.MemCapacities.Add(capacityStr);
                        _hardwareReport.MemManufacturers.Add(manufacturer);
                        _hardwareReport.MemSpeeds.Add(speed);
                    }

                    // 一次性更新 UI
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TxtMemManufacturer.Text = allManufacturer;
                        TxtMemCapacity.Text = allCapacity;
                        TxtMemSpeed.Text = allSpeed;
                    }));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TxtMemManufacturer.Text = "获取失败: " + ex.Message;
                }));
            }
        }

        private void LoadDiskInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"))
                {
                    string allModel = "";
                    string allCapacity = "";
                    string allType = "";
                    string allInterface = "";

                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string model = obj["Model"]?.ToString()?.Trim() ?? "N/A";

                        string capacity = "N/A";
                        try
                        {
                            ulong size = (ulong)(obj["Size"] ?? 0);
                            if (size > 0)
                                capacity = $"{size / 1073741824.0:F2} GB";
                        }
                        catch { }

                        // 介质类型
                        string type = obj["MediaType"]?.ToString() ?? "N/A";
                        // 接口类型
                        string interfaceType = obj["InterfaceType"]?.ToString() ?? "N/A";

                        _hardwareReport.DiskModels.Add(model);
                        _hardwareReport.DiskCapacities.Add(capacity);
                        _hardwareReport.DiskTypes.Add(type);
                        _hardwareReport.DiskInterfaces.Add(interfaceType);

                        // 拼接
                        if (allModel.Length > 0) allModel += "\n";
                        allModel += model;

                        if (allCapacity.Length > 0) allCapacity += "\n";
                        allCapacity += capacity;

                        if (allType.Length > 0) allType += "\n";
                        allType += type;

                        if (allInterface.Length > 0) allInterface += "\n";
                        allInterface += interfaceType;
                    }

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TxtDiskModel.Text = allModel;
                        TxtDiskCapacity.Text = allCapacity;
                        TxtDiskType.Text = allType;
                        TxtDiskInterface.Text = allInterface;
                    }));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TxtDiskModel.Text = "获取失败: " + ex.Message;
                }));
            }
        }
        
        private void LoadMainboardInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard"))
                {
                    string allManufacturer = "";
                    string allProduct = "";
                    string allVersion = "";
                    string allSerial = "";

                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string manufacturer = obj["Manufacturer"]?.ToString()?.Trim() ?? "N/A";
                        string product = obj["Product"]?.ToString()?.Trim() ?? "N/A";
                        string version = obj["Version"]?.ToString()?.Trim() ?? "N/A";
                        string serial = obj["SerialNumber"]?.ToString()?.Trim() ?? "N/A";

                        // 无效序列号处理
                        if (string.IsNullOrWhiteSpace(serial) || serial.Trim('0') == "")
                            serial = "N/A";

                        _hardwareReport.MbManufacturer = manufacturer;
                        _hardwareReport.MbProduct = product;
                        _hardwareReport.MbVersion = version;
                        _hardwareReport.MbSerialNumber = serial;


                        // 拼接
                        if (allManufacturer.Length > 0) allManufacturer += "\n";
                        allManufacturer += manufacturer;

                        if (allProduct.Length > 0) allProduct += "\n";
                        allProduct += product;

                        if (allVersion.Length > 0) allVersion += "\n";
                        allVersion += version;

                        if (allSerial.Length > 0) allSerial += "\n";
                        allSerial += serial;
                    }


                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TxtMbManufacturer.Text = allManufacturer;
                        TxtMbProduct.Text = allProduct;
                        TxtMbVersion.Text = allVersion;
                        TxtMbSerial.Text = allSerial;
                    }));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TxtMbManufacturer.Text = "获取失败: " + ex.Message;
                }));
            }
        }

        private void LoadNetworkInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter"))
                {
                    string allName = "";
                    string allManufacturer = "";
                    string allMAC = "";
                    string allSpeed = "";
                    string allType = "";

                    foreach (ManagementObject obj in searcher.Get())
                    {
                        // 只显示已启用的网卡
                        bool enabled = false;
                        try { enabled = Convert.ToBoolean(obj["NetEnabled"] ?? false); }
                        catch { continue; }
                        if (!enabled)
                            continue;

                        string name = obj["Name"]?.ToString() ?? "N/A";
                        string manufacturer = obj["Manufacturer"]?.ToString() ?? "N/A";
                        string mac = obj["MACAddress"]?.ToString() ?? "N/A";

                        string speed = "N/A";
                        try
                        {
                            ulong speedBps = (ulong)(obj["Speed"] ?? 0);
                            if (speedBps >= 1000000000)
                                speed = $"{speedBps / 1000000000.0:F1} Gbps";
                            else if (speedBps > 0)
                                speed = $"{speedBps / 1000000.0:F0} Mbps";
                        }
                        catch { }

                        string adapterType = obj["AdapterType"]?.ToString() ?? "N/A";

                        _hardwareReport.NetNames.Add(name);
                        _hardwareReport.NetManufacturers.Add(manufacturer);
                        _hardwareReport.NetMacAddresses.Add(mac);
                        _hardwareReport.NetSpeeds.Add(speed);
                        _hardwareReport.NetTypes.Add(adapterType);


                        if (allName.Length > 0) allName += "\n";
                        allName += name;

                        if (allManufacturer.Length > 0) allManufacturer += "\n";
                        allManufacturer += manufacturer;

                        if (allMAC.Length > 0) allMAC += "\n";
                        allMAC += mac;

                        if (allSpeed.Length > 0) allSpeed += "\n";
                        allSpeed += speed;

                        if (allType.Length > 0) allType += "\n";
                        allType += adapterType;
                    }

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TxtNetName.Text = allName;
                        TxtNetManufacturer.Text = allManufacturer;
                        TxtNetMAC.Text = allMAC;
                        TxtNetSpeed.Text = allSpeed;
                        TxtNetType.Text = allType;
                    }));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TxtNetName.Text = "获取失败: " + ex.Message;
                }));
            }
        }

        private void LoadAudioInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_SoundDevice"))
                {
                    string allName = "";
                    string allManufacturer = "";

                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string name = obj["Name"]?.ToString() ?? "N/A";
                        string manufacturer = obj["Manufacturer"]?.ToString() ?? "N/A";

                        _hardwareReport.AudioNames.Add(name);
                        _hardwareReport.AudioManufacturers.Add(manufacturer);


                        if (allName.Length > 0) allName += "\n";
                        allName += name;

                        if (allManufacturer.Length > 0) allManufacturer += "\n";
                        allManufacturer += manufacturer;
                    }

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TxtAudioName.Text = allName;
                        TxtAudioManufacturer.Text = allManufacturer;
                    }));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TxtAudioName.Text = "获取失败: " + ex.Message;
                }));
            }
        }



        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var exporter = new HeadingMarkdownExporter();
            string content = exporter.Export(_hardwareReport);
            var dialog = new SaveFileDialog()
            {
                Filter = "Markdown File (*.md)|*.md|All Files (*.*)|*.*",
                FileName = $"Hardwhat_Report_{DateTime.Now:yyyyMMdd_HHmmss}.md"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, content);
                MessageBox.Show($"Exported successfully at: {dialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Export failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}