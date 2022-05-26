// OOBB.cpp : Defines the exported functions for the DLL application.

#include "pch.h"
#include "OOBB.h"

#include <Windows.h>
#include <stdio.h>
#include <string>
#include <vector>

// Manual test of FLOAT marshalling
int32_t __stdcall GetOptimalOrientedBoundingBox(AriadnePoint3D* points, int size, Notification notification)
{
    std::vector<Point3D> element_points;
    for (int32_t i = 0; i < size; i++)
    {
        auto x = points[i].x;
        auto y = points[i].y;
        auto z = points[i].z;
        auto point = Point3D(x, y, z);
        element_points.push_back(point);
    }

    // Compute convex hull of element
    Surface_mesh element_mesh;
    CGAL::convex_hull_3(element_points.begin(), element_points.end(), element_mesh);
    if (element_mesh.is_empty())
        return 1;
    // Compute the extreme points of the mesh, and then a tightly fitted oriented bounding box
    const int32_t boxSize = 8;
    std::array<Point3D, boxSize> obb_points;
    CGAL::oriented_bounding_box(element_mesh, obb_points, CGAL::parameters::use_convex_hull(true));

    std::string str = "I'm JSON!\n";
    
    for (int32_t i = 0; i < boxSize; i++)
    {
        auto x = obb_points[i].x();
        auto y = obb_points[i].y();
        auto z = obb_points[i].z();
        str += "x = " + std::to_string(x) + ", " +
                "y = " + std::to_string(y) + ", " +
                "z = " + std::to_string(z) + ";\n";
    }

    notification(str.c_str());
    return 0;
}