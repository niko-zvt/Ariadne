// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

namespace Ariadne.Kernel.Math
{
    public interface IMappable
    {
        public AffineMap3D GetMapToGlobalCS();

        public AffineMap3D GetMapToLocalCS();

        //public object GetMapTo(CoordinateSystem coordinateSystem);
    }
}
