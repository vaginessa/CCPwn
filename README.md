# CC Checker
- Lightweight
- Stable
- Linux and Windows support
- MultiThreaded

# Setup
- Ubuntu Net 6 (https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu)
- Centos Net 6 (https://docs.microsoft.com/en-us/dotnet/core/install/linux-centos)
- Debian Net 6 (https://docs.microsoft.com/en-us/dotnet/core/install/linux-debian)
- Building Linux (`dotnet publish -r linux-x64`)
- Building Windows (just build as release)
- In source code go to Extensions.cs and in CheckCard were it says `//ADD PROXY` add a usa proxy

# Usage
- (Linux) `screen dotnet PWMGen.dll 1000 3 <bin_code>` (No <> needed its just a place holder)
