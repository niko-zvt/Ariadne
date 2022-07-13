// GeometrySupervisors.cpp : Defines the exported functions for the DLL application.
#include "pch.h"
#include "GeometrySupervisors.h"
#include <CGAL/Side_of_triangle_mesh.h>

int32_t __stdcall IsPointBelongToGrid(AriadnePoint3D point, AriadnePoint3D* elementPoints, int size, Notification notification)
{
    try 
    {
        std::string str = "NULL";

        // 1. Create target point
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

        // 2. Create mesh
        Surface_mesh element_mesh;
        Triangulation T(element_points.begin(), element_points.end());
        
        // 3. Locate point
        Locate_type lt;
        int li, lj;
        Cell_handle c = T.locate(target, lt, li, lj);

        // 4. Build result
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

        // 5. Export result
        notification(str.c_str());
        return 0;
    }
    catch (const std::exception& ex)
    {
        auto wt = ex.what();
    }
    return 1;
}

int32_t __stdcall SegmentsIntersection(AriadnePoint3D a1, AriadnePoint3D a2, AriadnePoint3D b1, AriadnePoint3D b2, Notification notification)
{
    try
    {
        // 1. Create target point
        std::string str = "{\"IntersectionType\":\"NULL\"}\n";

        double x = 0.0;
        double y = 0.0;
        double z = 0.0;
        double length = 0.0;

        // 2. Create segments
        auto line1 = Segment3D((Point3D(a1.x, a1.y, a1.z)), (Point3D(a2.x, a2.y, a2.z)));
        auto line2 = Segment3D((Point3D(b1.x, b1.y, b1.z)), (Point3D(b2.x, b2.y, b2.z)));

        // 3. Intersect segments
        auto result = CGAL::intersection(line1, line2);
        
        // 4. Build result
        if (result) 
        { 
            // IF SEGMENT
            const Kernel::Segment_3* s = boost::get<Kernel::Segment_3>(&*result);
            if (s)
            {
                // Start segment
                str = "{\"IntersectionType\":\"SEGMENT\"}\n";
                x = s->start().x();
                y = s->start().y();
                z = s->start().z();
                length = std::sqrt(x * x + y * y + z * z);
                str += "{\"Length\":" + std::to_string(length) +
                    ",\"X\":" + std::to_string(x) +
                    ",\"Y\":" + std::to_string(y) +
                    ",\"Z\":" + std::to_string(z) + "}\n";
                // End segment
                x = s->end().x();
                y = s->end().y();
                z = s->end().z();
                length = std::sqrt(x * x + y * y + z * z);
                str += "{\"Length\":" + std::to_string(length) +
                    ",\"X\":" + std::to_string(x) +
                    ",\"Y\":" + std::to_string(y) +
                    ",\"Z\":" + std::to_string(z) + "}\n";
            }

            // IF POINT
            const Kernel::Point_3* p = boost::get<Kernel::Point_3>(&*result);
            if (p)
            {
                str = "{\"IntersectionType\":\"POINT\"}\n";
                x = p->x();
                y = p->y();
                z = p->z();
                length = std::sqrt(x * x + y * y + z * z);
                str += "{\"Length\":" + std::to_string(length) +
                    ",\"X\":" + std::to_string(x) +
                    ",\"Y\":" + std::to_string(y) +
                    ",\"Z\":" + std::to_string(z) + "}\n";
            }
        }

        // 5. Export result
        notification(str.c_str());
        return 0;
    }
    catch (const std::exception& ex)
    {
        auto wt = ex.what();
    }
    return 1;
}