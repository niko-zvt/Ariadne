// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the CGAL_OOBB_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// CGAL_OOBB_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef CGAL_OOBB_EXPORTS
#define CGAL_OOBB_API __declspec(dllexport)
#else
#define CGAL_OOBB_API __declspec(dllimport)
#endif

#include <cstdint>

typedef void(__stdcall* Notification)(const char*);

extern "C" int32_t CGAL_OOBB_API __stdcall GetOptimalOrientedBoundingBox(int32_t start, int32_t count, Notification notification);
