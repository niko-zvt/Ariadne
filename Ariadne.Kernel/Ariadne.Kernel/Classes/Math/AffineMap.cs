using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ariadne.Kernel.Math
{
    abstract class AffineMap
    {
    }

    class AffineMap3D : AffineMap
    {
        private Matrix3x3 _R;
        private Vector3D _T;
        private Vector3D _S;
        private float _I;

        public AffineMap3D()
        {
            _R = new Matrix3x3(1, 1, 1);
            _T = new Vector3D();
            _S = new Vector3D();
            _I = 1.0f;
        }

        public AffineMap3D(Matrix3x3 R, Vector3D T)
        {
            _R = R;
            _T = T;
            _S = new Vector3D();
            _I = 1.0f;
        }

        public AffineMap3D(Vector3D i, Vector3D j, Vector3D k, Vector3D T)
        {
            _R = new Matrix3x3(i, j, k);
            _T = T;
            _S = new Vector3D();
            _I = 1.0f;
        }
    }
}
