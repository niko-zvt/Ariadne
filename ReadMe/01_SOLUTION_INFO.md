# Solution info

## Solution structure
The Ariadne solution consists of a set of subsystems represented by several projects.
Further, the projects are listed according to the order of their assembly with indication of cross-project dependencies.

|#| Project | Description | Dependencies | Third-party |
|-| ------- | ----------- | ---------- | ----------- |
|1| `Ariadne.CGAL` | The project that hides the low-level implementation of geometric and mathematical functions based on the C++ CGAL library.| `-` | `CGAL` | 
|2| `Ariadne.Kernel` | The project that implements the main core of the program (the data layer) is the source of the main classes of the application. It also implements hiding part of the FEM functionality of the third-party FeResPost library. | `Ariadne.CGAL` | `FeResPost` |
|3| **`Ariadne.Console`** | The project implements the command line interface (CLI) for the Ariadne kernel. **Now it's a start-up project.** | `Ariadne.Kernel` | `-` |