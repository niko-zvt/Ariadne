// AABB.cpp : Defines the exported functions for the DLL application.
#include "pch.h"
#include "GeometrySupervisors.h"
#include <CGAL/Side_of_triangle_mesh.h>

// Manual test of FLOAT marshalling
int32_t __stdcall IsPointBelongToElement(AriadnePoint3D point, AriadnePoint3D* elementPoints, int size, Notification notification)
{
    try 
    {
        std::string str = "NULL";
        auto target = Point3D(point.x, point.y, point.z);
        std::vector<Point3D> element_points;
        for (int32_t i = 0; i < size; i++)
        {
            auto x = elementPoints[i].x;
            auto y = elementPoints[i].y;
            auto z = elementPoints[i].z;
            auto point = Point3D(x, y, z);
            element_points.push_back(point);
        }

        Surface_mesh element_mesh;

        Triangulation T(element_points.begin(), element_points.end());
        Locate_type lt;
        int li, lj;
        Cell_handle c = T.locate(target, lt, li, lj);

        if (lt == Triangulation::VERTEX)
        {
            str = "VERTEX";
        }
        else if (lt == Triangulation::EDGE)
        {
            str = "EDGE";
        }
        else if (lt == Triangulation::FACET)
        {
            str = "FACET";
        }
        else if (lt == Triangulation::CELL)
        {
            str = "CELL";
        }
        else if (lt == Triangulation::OUTSIDE_CONVEX_HULL)
        {
            str = "OUTSIDE_CONVEX_HULL";
        }
        else if (lt == Triangulation::OUTSIDE_AFFINE_HULL)
        {
            str = "OUTSIDE_AFFINE_HULL";
        }
        else { /* DO NOTHING */ }

        notification(str.c_str());
        return 0;
    }
    catch (const std::exception& ex)
    {
        auto wt = ex.what();
    }
}