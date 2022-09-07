# Solution info

## Solution structure
The Ariadne solution consists of a set of subsystems represented by several projects.
Further, the projects are listed according to the order of their assembly with indication of cross-project dependencies.

|#| Project | Description | Dependencies | Third-party Dependencies |
|-| ------- | ----------- | ---------- | ----------- |
|1| `Ariadne.CGAL` | The project that hides the low-level implementation of geometric and mathematical functions based on the C++ CGAL library.|  | [![CGAL](https://img.shields.io/badge/CGAL-v5.5-informational)](https://www.cgal.org/) | 
|2| `Ariadne.Kernel` | The project that implements the main core of the program (the data layer) is the source of the main classes of the application. It also implements hiding part of the FEM functionality of the third-party FeResPost library. | `CGAL` | [![FeResPost](https://img.shields.io/badge/FeResPost-v.5.0.3-informational)](http://www.ferespost.eu/) |
|3| **`Ariadne.Console`** | The project implements the command line interface (CLI) for the Ariadne kernel. **Now it's a start-up project.** | `Kernel` |  |

Below are the service subprojects for debugging.

|#| Project | Description | Dependencies | Third-party Dependencies |
|-| ------- | ----------- | ---------- | ----------- |
|*| `Ariadne.Ruby` | A set of application utilities provided by the FeResPost project in the Ruby programming language. |  | [![FeResPos](https://img.shields.io/badge/FeResPost-v.5.0.3-informational)](http://www.ferespost.eu/) |