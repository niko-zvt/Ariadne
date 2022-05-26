// OOBB.cpp : Defines the exported functions for the DLL application.

#include "pch.h"
#include "OOBB.h"
#include <Windows.h>
#include <stdio.h>

// Manual test of FLOAT marshalling
int32_t __stdcall GetOptimalOrientedBoundingBox(AriadnePoint3D* points, int size)
{
    float res = 0.0f;

    for (int32_t i = 0; i < size; i++) {
        res += points[i].x + points[i].y + points[i].z;
    }

    return int32_t(res);
}
