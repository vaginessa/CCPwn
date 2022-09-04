# CC Checker + Gen
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
- (Linux) `screen dotnet PWMGen.dll <threads> <delay> <bin>` (No <> needed its just a place holder)

# FYI
- Saves in Valid.txt
