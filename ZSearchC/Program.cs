using System.Text;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Diagnostics;
using System;
using System.Security;
using System.ComponentModel;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using System.Security.Policy;

Console.WriteLine("Setting up DLLs...");
Console.WriteLine("DotNetZip.DLL");
Console.WriteLine("ZSearch.DLL");
Console.WriteLine("WinRT.Runtime.DLL");
Console.WriteLine("Microsoft.Windows.SDK.NET.DLL");

Console.WriteLine("Launching ZSearch...");
Process.Start("ZSearch.exe");

while (true)
{
    
}