// AABB.cpp : Defines the exported functions for the DLL application.
#include "pch.h"
#include "GeometrySupervisors.h"
#include <CGAL/Side_of_triangle_mesh.h>

// Manual test of FLOAT marshalling
int32_t __stdcall IsPointBelongToElement(AriadnePoint3D point, AriadnePoint3D* elementPoints, int size, Notification notification)
{
    std::string str = "NULL";
    std::vector<Point3D> element_points;
    for (int32_t i = 0; i < size; i++)
    {
        auto x = elementPoints[i].x;
        auto y = elementPoints[i].y;
        auto z = elementPoints[i].z;
        auto point = Point3D(x, y, z);
        element_points.push_back(point);
    }

    // Compute convex hull of element
    Surface_mesh element_mesh;
    CGAL::convex_hull_3(element_points.begin(), element_points.end(), element_mesh);
    if (element_mesh.is_empty() && !element_mesh.is_valid())
        return 1;

    CGAL::Side_of_triangle_mesh<Surface_mesh, Kernel> mesh(element_mesh);
    CGAL::Bounded_side res = mesh(Point3D(point.x, point.y, point.z));
    if (res == CGAL::ON_UNBOUNDED_SIDE) { str = "ON_UNBOUNDED_SIDE"; }
    if (res == CGAL::ON_BOUNDED_SIDE) { str = "ON_BOUNDED_SIDE"; }
    if (res == CGAL::ON_BOUNDARY) { str = "ON_BOUNDARY"; }

    notification(str.c_str());
    return 0;
}