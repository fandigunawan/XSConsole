dotnet restore
dotnet publish -c Release -f netcoreapp3.1 --self-contained -o pub/Win64 -r win-x64 .
dotnet publish -c Release -f netcoreapp3.1 --self-contained -o pub/Win32 -r win-x86 .
dotnet publish -c Release -f netcoreapp3.1 --self-contained -o pub/Linux64 -r linux-x64 .
dotnet publish -c Release -f netcoreapp3.1 --self-contained -o pub/LinuxMin64 -r linux-musl-x64 .
dotnet publish -c Release -f netcoreapp3.1 --self-contained -o pub/OSX64 -r osx-x64 .
copy xen-lic.txt pub/Win64/
copy xen-lic.txt pub/Win32/
copy xen-lic.txt pub/Linux64/
copy xen-lic.txt pub/LinuxMin64/
copy xen-lic.txt pub/OSX64/
cd pub/
zip -r XSConsole-Win64.zip Win64/
zip -r XSConsole-Win32.zip Win32/
zip -r XSConsole-Linux64.zip Linux64/
zip -r XSConsole-LinuxMin.zip LinuxMin64/
zip -r XSConsole-OSX64.zip OSX64/