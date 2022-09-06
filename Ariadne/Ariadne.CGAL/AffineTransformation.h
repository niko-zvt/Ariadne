// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

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
/// Transform point
/// </summary>
/// <param name="point">Point</param>
/// <param name="source">Source CS</param>
/// <param name="target">Target CS</param>
/// <param name="notification">Transform point as JSON string</param>
/// <returns>
/// - true in the case, when the result is valid
/// </returns>
extern "C" int32_t ARIADNE_CGAL_API __stdcall TransformPoint(AriadneVector3D point, AriadneLCS source, AriadneLCS target, Notification notification);