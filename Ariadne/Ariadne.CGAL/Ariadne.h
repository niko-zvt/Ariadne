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
#include <CGAL/intersections.h>


typedef void(__stdcall* Notification)(const char*);

// Ariadne.Kernel Vector3D
typedef struct _AriadneVector3D
{
    float x;
    float y;
    float z;
} AriadneVector3D;

// Ariadne.Kernel LCS
typedef struct _AriadneLCS
{
    AriadneVector3D origin;
    AriadneVector3D xAxis;
    AriadneVector3D yAxis;
    AriadneVector3D zAxis;
} AriadneLCS;

typedef CGAL::Exact_predicates_inexact_constructions_kernel     Kernel;
typedef Kernel::Point_3                                         Point3D;
typedef Kernel::Line_3                                          Line3D;
typedef Kernel::Segment_3                                       Segment3D;
typedef Kernel::Intersect_3                                     Intersect3D;
typedef CGAL::Polyhedron_3<Kernel>                              Polyhedron3D;
typedef CGAL::Surface_mesh<Point3D>                             Surface_mesh;
typedef CGAL::Triangulation_3<Kernel>                           Triangulation;
typedef Triangulation::Cell_handle                              Cell_handle;
typedef Triangulation::Vertex_handle                            Vertex_handle;
typedef Triangulation::Locate_type                              Locate_type;
typedef CGAL::Aff_transformation_3<Kernel>                      Transformation3D;