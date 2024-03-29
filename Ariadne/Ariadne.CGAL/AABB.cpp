// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

// AABB.cpp : Defines the exported functions for the DLL application.
#include "pch.h"
#include "AABB.h"

int32_t __stdcall GetAxisAlignedBoundingBox(AriadneVector3D* points, int size, Notification notification)
{
    try
    {
        // 1. Create points
        std::vector<Point3D> element_points;
        for (int32_t i = 0; i < size; i++)
        {
            auto x = points[i].x;
            auto y = points[i].y;
            auto z = points[i].z;
            auto point = Point3D(x, y, z);
            element_points.push_back(point);
        }

        // 2. Create axis-aligned bounding box of 3D points
        Kernel::Iso_cuboid_3 c3 = CGAL::bounding_box(element_points.begin(), element_points.end());
        double xMin = c3.bbox().xmin();
        double yMin = c3.bbox().ymin();
        double zMin = c3.bbox().zmin();
        double minLength = std::sqrt(xMin * xMin + yMin * yMin + zMin * zMin);

        double xMax = c3.bbox().xmax();
        double yMax = c3.bbox().ymax();
        double zMax = c3.bbox().zmax();
        double maxLength = std::sqrt(xMax * xMax + yMax * yMax + zMax * zMax);

        // 3. Build result
        std::string str = "";

        str += "{\"Length\":" + std::to_string(minLength) +
            ",\"X\":" + std::to_string(xMin) +
            ",\"Y\":" + std::to_string(yMin) +
            ",\"Z\":" + std::to_string(zMin) + "}\n";

        str += "{\"Length\":" + std::to_string(maxLength) +
            ",\"X\":" + std::to_string(xMax) +
            ",\"Y\":" + std::to_string(yMax) +
            ",\"Z\":" + std::to_string(zMax) + "}";

        // 4. Export result
        notification(str.c_str());
        return 0;
    }
    catch (const std::exception& ex)
    {
        auto wt = ex.what();
    }
    return 1;
}