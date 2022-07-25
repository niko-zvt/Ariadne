// GeometrySupervisors.cpp : Defines the exported functions for the DLL application.
#include "pch.h"
#include "AffineTransformation.h"

int32_t __stdcall TransformPoint(AriadneVector3D point, AriadneLCS source, AriadneLCS target, Notification notification)
{
    try
    {
        std::string str = "NULL";

        // 1. Create point
        auto p = Point3D(point.x, point.y, point.z);
        
        // TODO: TRANSFORM

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