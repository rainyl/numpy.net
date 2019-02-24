﻿/*
 * BSD 3-Clause License
 *
 * Copyright (c) 2018-2019
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice,
 *    this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its
 *    contributors may be used to endorse or promote products derived from
 *    this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using NumpyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NPY_INTP_64
using npy_intp = System.Int64;
#else
using npy_intp = System.Int32;
#endif

namespace NumpyDotNet
{
    public static partial class np
    {

        private static ndarray asarray(ndarray m)
        {
            return m;
        }

        public static ndarray asanyarray(object a, dtype dtype = null, NPY_ORDER order = NPY_ORDER.NPY_ANYORDER)
        {
            //  Convert the input to a masked array, conserving subclasses.

            //  If `a` is a subclass of `MaskedArray`, its class is conserved.
            //  No copy is performed if the input is already an `ndarray`.

            //  Parameters
            //  ----------
            //  a : array_like
            //      Input data, in any form that can be converted to an array.
            //  dtype : dtype, optional
            //      By default, the data-type is inferred from the input data.
            //  order : {'C', 'F'}, optional
            //      Whether to use row-major('C') or column-major('FORTRAN') memory
            //    representation.Default is 'C'.
            //
            //  Returns
            //  -------
            //
            // out : MaskedArray
            //    MaskedArray interpretation of `a`.
            //
            //
            //See Also
            //  --------
            //
            //asarray : Similar to `asanyarray`, but does not conserve subclass.
            //
            //Examples
            //  --------
            //  >>> x = np.arange(10.).reshape(2, 5)
            //  >>> x
            //
            //array([[0., 1., 2., 3., 4.],
            //
            //       [5., 6., 7., 8., 9.]])
            //  >>> np.ma.asanyarray(x)
            //  masked_array(data =
            //   [[0.  1.  2.  3.  4.]
            //   [5.  6.  7.  8.  9.]],
            //               mask =
            //   False,
            //         fill_value = 1e+20)
            //  >>> type(np.ma.asanyarray(x))
            //  <class 'numpy.ma.core.MaskedArray'>

            //if (a is MaskedArray && (dtype == null || dtype == a.Dtype))
            //{
            //    return a;
            //}
            //return masked_array(a, dtype: dtype, copy: false, keep_mask: true, sub_ok: true);

            if (dtype != null)
            {
                return np.array(a, dtype, copy: false, order: order, subok: true);
            }

            if (a is ndarray)
            {
                return a as ndarray;
            }
            if (a.GetType().IsArray)
            {
                System.Array ssrc = a as System.Array;
                NPY_TYPES type_num;
                switch (ssrc.Rank)
                {
                    case 1:
                        type_num = Get_NPYType(ssrc.GetValue(0));
                        break;
                    case 2:
                        type_num = Get_NPYType(ssrc.GetValue(0,0));
                        break;
                    case 3:
                        type_num = Get_NPYType(ssrc.GetValue(0, 0, 0));
                        break;
                    case 4:
                        type_num = Get_NPYType(ssrc.GetValue(0, 0, 0, 0));
                        break;
                    case 5:
                        type_num = Get_NPYType(ssrc.GetValue(0, 0, 0, 0, 0));
                        break;
                    case 6:
                        type_num = Get_NPYType(ssrc.GetValue(0, 0, 0, 0, 0, 0));
                        break;
                    case 7:
                        type_num = Get_NPYType(ssrc.GetValue(0, 0, 0, 0, 0, 0, 0));
                        break;
                    case 8:
                        type_num = Get_NPYType(ssrc.GetValue(0, 0, 0, 0, 0, 0, 0));
                        break;
                    default:
                        throw new Exception("Number of dimensions is not supported");
                }

                return ndArrayFromMD(ssrc, type_num, ssrc.Rank);
            }

            if (IsNumericType(a))
            {
                return np.array(GetSingleElementArray(a), null);
            }

            throw new Exception("Unable to convert object to ndarray");
        }

        private static ndarray ndArrayFromMD(Array ssrc, NPY_TYPES type_num, int ndim)
        {
            npy_intp []newshape = new npy_intp[ndim];
            for (int i = 0; i < ndim; i++)
            {
                newshape[i] = ssrc.GetLength(i);
            }

            return np.array(new VoidPtr(ArrayFromMD(ssrc, type_num), type_num), null).reshape(newshape);
        }

        private static System.Array ArrayFromMD(Array ssrc, NPY_TYPES type_num)
        {
            switch (type_num)
            {
                case NPY_TYPES.NPY_BOOL:
                    return ssrc.Cast<bool>().ToArray();
                case NPY_TYPES.NPY_BYTE:
                    return ssrc.Cast<sbyte>().ToArray();
                case NPY_TYPES.NPY_UBYTE:
                    return ssrc.Cast<byte>().ToArray();
                case NPY_TYPES.NPY_INT16:
                    return ssrc.Cast<Int16>().ToArray();
                case NPY_TYPES.NPY_UINT16:
                    return ssrc.Cast<UInt16>().ToArray();
                case NPY_TYPES.NPY_INT32:
                    return ssrc.Cast<Int32>().ToArray();
                case NPY_TYPES.NPY_UINT32:
                    return ssrc.Cast<UInt32>().ToArray();
                case NPY_TYPES.NPY_INT64:
                    return ssrc.Cast<Int64>().ToArray();
                case NPY_TYPES.NPY_UINT64:
                    return ssrc.Cast<UInt64>().ToArray();
                case NPY_TYPES.NPY_FLOAT:
                    return ssrc.Cast<float>().ToArray();
                case NPY_TYPES.NPY_DOUBLE:
                    return ssrc.Cast<double>().ToArray();
                case NPY_TYPES.NPY_DECIMAL:
                    return ssrc.Cast<decimal>().ToArray();
            }

            throw new Exception("Unexpected NPY_TYPES");

        }

        public static bool IsNumericType(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static VoidPtr GetSingleElementArray(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Boolean:
                    return new VoidPtr(new bool[] { (bool)o });
                case TypeCode.Byte:
                    return new VoidPtr(new byte[] { (byte)o });
                case TypeCode.SByte:
                    return new VoidPtr(new sbyte[] { (sbyte)o });
                case TypeCode.UInt16:
                    return new VoidPtr(new UInt16[] { (UInt16)o });
                case TypeCode.UInt32:
                    return new VoidPtr(new UInt32[] { (UInt32)o });
                case TypeCode.UInt64:
                    return new VoidPtr(new UInt64[] { (UInt64)o });
                case TypeCode.Int16:
                    return new VoidPtr(new Int16[] { (Int16)o });
                case TypeCode.Int32:
                    return new VoidPtr(new Int32[] { (Int32)o });
                case TypeCode.Int64:
                   return new VoidPtr(new Int64[] { (Int64)o });
                case TypeCode.Decimal:
                    return new VoidPtr(new Decimal[] { (Decimal)o });
                case TypeCode.Double:
                    return new VoidPtr(new Double[] { (Double)o });
                case TypeCode.Single:
                    return new VoidPtr(new Single[] { (Single)o });
                default:
                    throw new Exception("Unable to convert numeric type");
            }
        }

     
        private static bool hasattr(ndarray m, string v)
        {
            return true;
        }



        private static npy_intp[] arange(int start, int end)
        {
            npy_intp[] a = new npy_intp[end - start];

            int index = 0;
            for (int i = start; i < end; i++)
            {
                a[index] = i;
                index++;
            }

            return a;
        }



    }
}
