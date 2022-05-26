// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the CGAL_OOBB_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// CGAL_OOBB_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#pragma once
#ifdef CGAL_OOBB_EXPORTS
#define CGAL_OOBB_API __declspec(dllexport)
#else
#define CGAL_OOBB_API __declspec(dllimport)
#endif

#include <cstdint>
#include <string>

#include <CGAL/Exact_predicates_inexact_constructions_kernel.h>
#include <CGAL/Point_set_3.h>
#include <CGAL/Surface_mesh.h>
#include <CGAL/Polyhedron_3.h>
#include <CGAL/convex_hull_3.h>
#include <CGAL/optimal_bounding_box.h>
#include <CGAL/Delaunay_triangulation_3.h>

typedef CGAL::Exact_predicates_inexact_constructions_kernel    Kernel;
typedef Kernel::Point_3                                        Point3D;
typedef CGAL::Polyhedron_3<Kernel>                             Polyhedron3D;
typedef CGAL::Surface_mesh<Point3D>                            Surface_mesh;
typedef CGAL::Delaunay_triangulation_3<Kernel>                 Delaunay;
typedef Delaunay::Vertex_handle                                Vertex_handle;

typedef void(__stdcall* Notification)(const char*);

// Ariadne.Kernel Point3D
typedef struct _AriadnePoint3D
{
    float x;
    float y;
    float z;
} AriadnePoint3D;

extern "C" int32_t CGAL_OOBB_API __stdcall GetOptimalOrientedBoundingBox(AriadnePoint3D * points, int size, Notification notification);
