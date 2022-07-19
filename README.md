# <u>XSConsole</u>

XenServer remote console based on XCP code ported to Windows, Linux and MacOS using .NET

This application is derived from XCP or XenCenter. The purpose of making this application is to provide a Xen API CLI based tool that is multiplatform.

Target OS:

- Windows x86
- Windows x64
- Linux x64
- Linux MUSL x64 (e.g Alpine Linux)
- OSX x64

The prerequisite framework is embedded inside the application so that you can move it to anywhere without any dependency.

Requirements:
**Free disk space > 100MB**

## Quick Guide

```
xe -s server_path -u user_name -pw password command-list
```

Alternatively for Linux/MacOS with system variables:
```
export XE_HOST=xenserver_host
export XE_USERNAME=username
export XE_PASSWORD=password
xe command-list
```
For Windows
```
set XE_HOST=xenserver_host
set XE_USERNAME=username
set XE_PASSWORD=password
xe command-list
```


For command-list, please refer to : https://linuxconfig.org/xe-full-command-list-reference-with-description-for-xenserver

## Build

Install .NET 6.0


```bash
git clone https://github.com/fandigunawan/XSConsole.git
cd XSConsole
```

Windows x64

```
dotnet publish -c Release -f net6.0 --self-contained -o pub\Win64 -r win-x64 .
```

Windows x86

```
dotnet publish -c Release -f net6.0 --self-contained -o pub\Win32 -r win-x86 .
```

Linux x64

```
dotnet publish -c Release -f net6.0 --self-contained -o pub/Linux64 -r linux-x64 .
```

Linux MUSL x64

```
dotnet publish -c Release -f net6.0 --self-contained -o pub/LinuxMin64 -r linux-musl-x64 .
```

MacOS X

```
dotnet publish -c Release -f net6.0 --self-contained -o pub/OSX64 -r osx-x64 .
```

## License
Please see xen-lic.txt