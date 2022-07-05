# Build info
This section provides information on how to build a solution and configure projects for different platforms.

## **Windows (x64)**
___
### **1. Install vcpkg**
[Installing `vcpkg`](https://vcpkg.io/en/getting-started.html) is a two-step process: first, clone the repo, then run the bootstrapping script to produce the vcpkg binary. The repo can be cloned anywhere, and will include the vcpkg binary after bootstrapping as well as any libraries that are installed from the command line. It is recommended to clone vcpkg as a submodule for CMake projects, but to install it globally for MSBuild projects. If installing globally, we recommend a short install path like: `C:\vcpkg` or `C:\dev\vcpkg`, since otherwise you may run into path issues for some port build systems.

#### 1.1. Clone the vcpkg repo
    git clone https://github.com/Microsoft/vcpkg.git

#### 1.2. Run the bootstrap script to build vcpkg
    .\vcpkg\bootstrap-vcpkg.bat

### **2. Config environment**

Add environment variable [`{VCPKG_ROOT}`](https://vcpkg.readthedocs.io/en/latest/users/config-environment/#:~:text=command%2Dspecific%20help.-,VCPKG_ROOT,-This%20environment%20variable).

This environment variable can be set to a directory to use as the root of the vcpkg instance. Note that mixing vcpkg repo versions and executable versions can cause issues.

_GUI Method:_

"Settings" -> "About" -> "Advanced system settings" link -> "Advanced" tab -> "Environment Variables" button -> "New" button

_Command Prompt Method:_

    setx VCPKG_ROOT “C:\vcpkg”

_PowerShell Method:_

    $env:VCPKG_ROOT = 'C:\vcpkg'

### **3. Install libraries for your platform**

#### 3.1. [Install CGAL with the Vcpkg Library Manager](https://doc.cgal.org/latest/Manual/windows.html)

Because of a bug with gmp in vcpkg for windows, you need to install yasm-tool in 32 bits to be able to correctly build gmp 64bits, needed for cgal:

    ./vcpkg.exe install yasm-tool:x86-windows

You are now ready to install CGAL:

    ./vcpkg.exe install cgal:x64-windows

Some CGAL packages also have additional dependencies.
Sometimes you may need to install additional packages, such as [`gmp`](https://vcpkg.info/port/gmp) or [`eigen3`](https://vcpkg.info/port/eigen3). Use the suffix `{package_name}:x64-windows` to build for a specific platform.

### **Errors**

If there are errors, see the log. The most common mistakes:
* CGAL header files were not found at compile time.
* Additional library files were not found during linking.

To fix it, check that the path is specified correctly in the `{VCPKG_ROOT}` variable.