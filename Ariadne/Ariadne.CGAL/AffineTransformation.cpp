// GeometrySupervisors.cpp : Defines the exported functions for the DLL application.
#include "pch.h"
#include "AffineTransformation.h"

int32_t __stdcall TransformPoint(AriadneVector3D pointInSource, AriadneLCS source, AriadneLCS target, Notification notification)
{
    try
    {
        std::string str = "NULL";

        // 1. Create point
        auto lp = Point3D(pointInSource.x, pointInSource.y, pointInSource.z);
        
        // 2. Create affine map from source CS to global CS
        auto sourceMap = Transformation3D(source.xAxis.x, source.xAxis.y, source.xAxis.z, source.origin.x,
                                          source.yAxis.x, source.yAxis.y, source.yAxis.z, source.origin.y,
                                          source.zAxis.x, source.zAxis.y, source.zAxis.z, source.origin.z);

        auto mapToGlobalCS = sourceMap.inverse();

        // 3. Transform point to global CS
        auto gp = lp.transform(mapToGlobalCS);

        // 4. Create affine map from global CS to target CS
        auto targetMap = Transformation3D(target.xAxis.x, target.xAxis.y, target.xAxis.z, target.origin.x,
                                          target.yAxis.x, target.yAxis.y, target.yAxis.z, target.origin.y,
                                          target.zAxis.x, target.zAxis.y, target.zAxis.z, target.origin.z);
        
        // 5. Transform point
        auto resultPoint = gp.transform(targetMap);

        // 6. Export result
        auto x = resultPoint.x();
        auto y = resultPoint.y();
        auto z = resultPoint.z();
        auto length = std::sqrt(x * x + y * y + z * z);
        str = "{\"Length\":" + std::to_string(length) +
            ",\"X\":" + std::to_string(x) +
            ",\"Y\":" + std::to_string(y) +
            ",\"Z\":" + std::to_string(z) + "}\n";

        notification(str.c_str());
        return 0;
    }
    catch (const std::exception& ex)
    {
        auto wt = ex.what();
    }
    return 1;
}