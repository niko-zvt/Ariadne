#pragma once

#include "pch.h"
#include <Windows.h>
#include <stdio.h>
#include <string>
#include <vector>
#include <cstdint>

#include <CGAL/Exact_predicates_inexact_constructions_kernel.h>
#include <CGAL/Point_set_3.h>
#include <CGAL/bounding_box.h>
#include <CGAL/Surface_mesh.h>
#include <CGAL/Polyhedron_3.h>
#include <CGAL/convex_hull_3.h>
#include <CGAL/optimal_bounding_box.h>
#include <CGAL/Delaunay_triangulation_3.h>
#include <CGAL/Triangulation_3.h>


typedef void(__stdcall* Notification)(const char*);

// Ariadne.Kernel Point3D
typedef struct _AriadnePoint3D
{
    float x;
    float y;
    float z;
} AriadnePoint3D;

typedef CGAL::Exact_predicates_inexact_constructions_kernel     Kernel;
typedef Kernel::Point_3                                         Point3D;
typedef CGAL::Polyhedron_3<Kernel>                              Polyhedron3D;
typedef CGAL::Surface_mesh<Point3D>                             Surface_mesh;
typedef CGAL::Triangulation_3<Kernel>                           Triangulation;
typedef Triangulation::Cell_handle                              Cell_handle;
typedef Triangulation::Vertex_handle                            Vertex_handle;
typedef Triangulation::Locate_type                              Locate_type;