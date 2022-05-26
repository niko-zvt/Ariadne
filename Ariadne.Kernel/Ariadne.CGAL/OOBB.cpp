// OOBB.cpp : Defines the exported functions for the DLL application.

#include "pch.h"
#include "OOBB.h"

#include <Windows.h>
#include <stdio.h>
#include <string>

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
    //Surface_mesh element_mesh;
    //CGAL::convex_hull_3(element_points.begin(), element_points.end(), element_mesh);
    //if (element_mesh.is_empty())
    //    return 1;
    //// Compute the extreme points of the mesh, and then a tightly fitted oriented bounding box
    //const int32_t boxSize = 8;
    //std::array<Point3D, boxSize> obb_points;
    //CGAL::oriented_bounding_box(element_mesh, obb_points, CGAL::parameters::use_convex_hull(true));
    //auto box = new AriadnePoint3D[boxSize];
    //for (int32_t i = 0; i < boxSize; i++)
    //{
    //    auto x = obb_points[i].x();
    //    auto y = obb_points[i].y();
    //    auto z = obb_points[i].z();
    //    box[i] = { float(x), float(y), float(z) };
    //}

    auto str = GetJSONByPointVector(element_points);
    notification(str.c_str());
    return 0;
}

std::string GetJSONByPointVector(const std::vector<Point3D>& points)
{
    return "I'm JSON! My size " + std::to_string(points.size());
}