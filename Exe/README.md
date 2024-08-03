reference env issues
A. code: https://blueskyson.github.io/2022/05/01/c-sharp-uart/
B. fail to create a Console App template solution: https://blog.csdn.net/m0_50214533/article/details/126262094
C. system.io.port plug-in: https://blog.csdn.net/Dr_Haven/article/details/116530895
D. COM-network: https://moon-half.info/p/3491
E. Com0com: tera term failed to detect Com0com virtual serial port
F. Virtual Serial Port Kit: tera term succeeds in detecting virtual serial port
                            https://www.fabulatech.com/virtual-serial-port-kit-download.html
G. PlatformNotSupported_IOPorts: 
    1. command prompt
    2. navigate to project directory where .csproj file is located
    3. run the command: dotnet add package RJCP.SerialPortStream
    4. command will download the RJCP.SerialPortStream package and update your project’s .csproj file to include a reference to this package

H. 'using System' is grayed out: .csproj file is not included in the project