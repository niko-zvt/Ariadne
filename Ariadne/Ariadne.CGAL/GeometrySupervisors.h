// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the ARIADNE_CGAL_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// ARIADNE_CGAL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#pragma once
#ifdef ARIADNE_CGAL_EXPORTS
#define ARIADNE_CGAL_API __declspec(dllexport)
#else
#define ARIADNE_CGAL_API __declspec(dllimport)
#endif

#include "Ariadne.h"

/// <summary>
/// The method determines whether a point belongs to a grid created on the basis of a point cloud.
/// </summary>
/// <param name="point">Point</param>
/// <param name="meshPoints">Point cloud of mesh</param>
/// <param name="size">Size of point cloud</param>
/// <param name="notification">Belonging of a point as a JSON string</param>
/// <returns>
/// <para> - true in the case, when the result is valid.</para>
/// JSON contain:<br/>
///  - VERTEX    - if the point lies at the vertex of the grid.<br/>
///  - EDGE      - if the point lies at the edge of the grid.<br/>
///  - FACET     - if the point lies at the facet of the grid.<br/>
///  - CELL      - if the point lies inside the cell.<br/>
///  - OUTSIDE_CONVEX_HULL - if the point lies at the outside convex hull.<br/>
///  - OUTSIDE_AFFINE_HULL - if the point lies at the outside affine hull.<br/>
/// </returns>
extern "C" int32_t ARIADNE_CGAL_API __stdcall IsPointBelongToGrid(AriadneVector3D point, AriadneVector3D * elementPoints, int size, Notification notification);

/// <summary>
/// The method determines the intersection of two segments defined by the start and end points.
/// </summary>
/// <param name="A1">Start point of first segment.</param>
/// <param name="A2">End point of first segment.</param>
/// <param name="B1">Start point of second segment.</param>
/// <param name="B2">End point of second segment.</param>
/// <param name="notification">Intersection type as JSON string.</param>
/// <returns>
/// <para> - true in the case, when the result is valid.</para>
/// JSON contain:<br/>
///  - NULL    - if there are no intersection points.<br/>
///  - POINT   - if the intersection is a point.<br/>
///  - SEGMENT - if the intersection is a segment.<br/>
/// </returns>
extern "C" int32_t ARIADNE_CGAL_API __stdcall SegmentsIntersection(AriadneVector3D a1, AriadneVector3D a2, AriadneVector3D b1, AriadneVector3D b2, Notification notification);

/// <summary>
/// The method determines the intersection of two lines defined by the start and end points.
/// </summary>
/// <param name="A1">Start point of first line.</param>
/// <param name="A2">End point of first line.</param>
/// <param name="B1">Start point of second line.</param>
/// <param name="B2">End point of second line.</param>
/// <param name="notification">Intersection type as JSON string.</param>
/// <returns>
/// <para> - true in the case, when the result is valid.</para>
/// JSON contain:<br/>
///  - NULL    - if there are no intersection points.<br/>
///  - POINT   - if the intersection is a point.<br/>
///  - LINE	   - if the intersection is a line.<br/>
/// </returns>
extern "C" int32_t ARIADNE_CGAL_API __stdcall LinesIntersection(AriadneVector3D a1, AriadneVector3D a2, AriadneVector3D b1, AriadneVector3D b2, Notification notification);