// OOBB.cpp : Defines the exported functions for the DLL application.
//
#include "pch.h"
#include "OOBB.h"
#include <Windows.h>
#include <stdio.h>

int32_t __stdcall GetOptimalOrientedBoundingBox(int32_t start, int32_t count, Notification notification)
{
    if (notification == nullptr)
    {
        return 0;
    }
    int32_t result = 0;
    for (int32_t i = 0; i < count; ++i)
    {
        char buffer[64];
        result += sprintf_s(buffer, "OOBB - Notification %d from C++", i + start);
        notification(buffer);
        Sleep(500);
    }
    return result;
}
