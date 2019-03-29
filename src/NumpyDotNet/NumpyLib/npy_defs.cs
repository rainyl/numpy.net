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

#define NPY_LITTLE_ENDIAN
//#define NPY_BIG_ENDIAN

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NPY_INTP_64
using npy_intp = System.Int64;
#else
using npy_intp = System.Int32;
#endif

namespace NumpyLib
{
  
    public class NpyArray_Dims
    {
        public npy_intp[] ptr;
        public int len;
    };
    public struct npy_datetimestruct
    {
        public int year;
        public int month, day, hour, min, sec, us, ps, as1;
    };
    public struct npy_timedeltastruct
    {
        public long day;
        public int sec, us, ps, as1;
    };

    public enum NPY_TYPECHAR
    {
        NPY_BOOLLTR = '?',
        NPY_BYTELTR = 'b',
        NPY_UBYTELTR = 'B',
        NPY_SHORTLTR = 'h',
        NPY_USHORTLTR = 'H',
        NPY_INTLTR = 'i',
        NPY_UINTLTR = 'I',
        NPY_LONGLTR = 'l',
        NPY_ULONGLTR = 'L',
        NPY_LONGLONGLTR = 'q',
        NPY_ULONGLONGLTR = 'Q',
        NPY_FLOATLTR = 'f',
        NPY_DOUBLELTR = 'd',
        NPY_LONGDOUBLELTR = 'g',
        NPY_CFLOATLTR = 'F',
        NPY_CDOUBLELTR = 'D',
        NPY_CLONGDOUBLELTR = 'G',
        NPY_OBJECTLTR = 'O',
        NPY_STRINGLTR = 'S',
        NPY_STRINGLTR2 = 'a',
        NPY_UNICODELTR = 'U',
        NPY_VOIDLTR = 'V',
        NPY_DATETIMELTR = 'M',
        NPY_TIMEDELTALTR = 'm',
        NPY_CHARLTR = 'c',

        /*
         * No Descriptor, just a define -- this let's
         * Python users specify an array of integers
         * large enough to hold a pointer on the
         * platform
         */
        NPY_INTPLTR = 'p',
        NPY_UINTPLTR = 'P',

        NPY_GENBOOLLTR = 'b',
        NPY_SIGNEDLTR = 'i',
        NPY_UNSIGNEDLTR = 'u',
        NPY_FLOATINGLTR = 'f',
        NPY_COMPLEXLTR = 'c'
    };

    public enum NPY_TYPES : int
    {
        NPY_NOTSET = -1,
        NPY_BOOL = 0,
        NPY_BYTE,
        NPY_UBYTE,
        NPY_SHORT,
        NPY_USHORT,
        NPY_INT,
        NPY_UINT,
        NPY_LONG,
        NPY_ULONG,
        NPY_LONGLONG,
        NPY_ULONGLONG,
        NPY_FLOAT,
        NPY_DOUBLE,
        NPY_DECIMAL,
        NPY_LONGDOUBLE,
        NPY_CFLOAT,
        NPY_CDOUBLE,
        NPY_CLONGDOUBLE,
        NPY_DATETIME,
        NPY_TIMEDELTA,
        NPY_OBJECT,
        NPY_STRING,
        NPY_UNICODE,
        NPY_VOID,
        NPY_NTYPES,
        NPY_NOTYPE,
        NPY_CHAR,               /* special flag */
        NPY_USERDEF = 256,      /* leave room for characters */

        NPY_INT8 = NPY_BYTE,
        NPY_INT16 = NPY_SHORT,
        NPY_INT32 = NPY_INT,
        NPY_INT64 = NPY_LONG,
        NPY_INT128 = NPY_LONGLONG,
        NPY_UINT8 = NPY_UBYTE,
        NPY_UINT16 = NPY_USHORT,
        NPY_UINT32 = NPY_UINT,
        NPY_UINT64 = NPY_ULONG,

#if NPY_INTP_64
        NPY_INTP = NPY_LONG,
#else
        NPY_INTP = NPY_INT,
#endif
        //NPY_UINT128 = NPY_ULONGLONG,

        //NPY_FLOAT32 = NPY_FLOAT,
        //NPY_FLOAT64 = NPY_DOUBLE,
        //NPY_FLOAT80 = NPY_DOUBLE,
        //NPY_FLOAT96 = NPY_LONGDOUBLE,
        //NPY_FLOAT128 = NPY_LONGDOUBLE,

        //NPY_COMPLEX64 = NPY_CFLOAT,
        //NPY_COMPLEX128 = NPY_CDOUBLE,
        //NPY_COMPLEX160 = NPY_CLONGDOUBLE,
        //NPY_COMPLEX192 = NPY_CLONGDOUBLE,
        //NPY_COMPLEX256 = NPY_CLONGDOUBLE,

    };

    /* How many floating point types are there */
    internal class npy_defs
    {
        // VALID indicates a currently-allocated object, INVALID means object has been deallocated.
        internal const UInt32 NPY_VALID_MAGIC = 1234567;
        internal const UInt32 NPY_INVALID_MAGIC = 0xdeadbeef;

        internal const int NPY_MAXDIMS = 32;
        internal const int NPY_MAXARGS = 32;
        internal const int NPY_MAX_PIVOT_STACK = 50;

        internal const int NPY_MAX_INTP = 2147483647;
        internal const int NPY_MIN_INTP = (-2147483647 - 1);

        internal const int NPY_BUFSIZE = 10000;

        internal const int NPY_NUM_FLOATTYPE = 3;
        internal const int NPY_NSORTS = (int)(NPY_SORTKIND.NPY_MERGESORT + 1);
        internal const int NPY_NSEARCHSIDES = (int)(NPY_SEARCHSIDE.NPY_SEARCHRIGHT + 1);
        internal const int NPY_NSCALARKINDS = (int)(NPY_SCALARKIND.NPY_OBJECT_SCALAR + 1);

            

        internal const string NPY_STR_Y = "Y";
        internal const string NPY_STR_M = "M";
        internal const string NPY_STR_W = "W";
        internal const string NPY_STR_B = "B";
        internal const string NPY_STR_D = "D";
        internal const string NPY_STR_h = "h";
        internal const string NPY_STR_m = "m";
        internal const string NPY_STR_s = "s";
        internal const string NPY_STR_ms = "ms";
        internal const string NPY_STR_us = "us";
        internal const string NPY_STR_ns = "ns";
        internal const string NPY_STR_ps = "ps";
        internal const string NPY_STR_fs = "fs";
        internal const string NPY_STR_as = "as";
    }

    public enum NPY_SORTKIND : int
    {
        NPY_QUICKSORT = 0,
        NPY_HEAPSORT = 1,
        NPY_MERGESORT = 2
    };

    public enum NPY_SEARCHSIDE : int
    {
        NPY_SEARCHLEFT = 0,
        NPY_SEARCHRIGHT = 1
    };

    public enum NPY_SCALARKIND : int
    {
        NPY_NOSCALAR = -1,
        NPY_BOOL_SCALAR,
        NPY_INTPOS_SCALAR,
        NPY_INTNEG_SCALAR,
        NPY_FLOAT_SCALAR,
        NPY_COMPLEX_SCALAR,
        NPY_OBJECT_SCALAR
    };


    public enum NPY_CLIPMODE : int
    {
        NPY_CLIP = 0,
        NPY_WRAP = 1,
        NPY_RAISE = 2
    };


    public enum NPY_CASTING : int
    {
        /* Only allow identical types */
        NPY_NO_CASTING = 0,
        /* Allow identical and byte swapped types */
        NPY_EQUIV_CASTING = 1,
        /* Only allow safe casts */
        NPY_SAFE_CASTING = 2,
        /* Allow safe casts or casts within the same kind */
        NPY_SAME_KIND_CASTING = 3,
        /* Allow any casts */
        NPY_UNSAFE_CASTING = 4
    };

    public enum NPY_CONVOLE_MODE : int
    {
        NPY_CONVOLVE_VALID = 0,
        NPY_CONVOLVE_SAME = 1,
        NPY_CONVOLVE_FULL = 2,
    };

    public enum NPY_SELECTKIND : int
    {
        NPY_INTROSELECT = 0,
    }


    public enum NPY_DATETIMEUNIT : int
    {
        NPY_FR_Y,
        NPY_FR_M,
        NPY_FR_W,
        NPY_FR_B,
        NPY_FR_D,
        NPY_FR_h,
        NPY_FR_m,
        NPY_FR_s,
        NPY_FR_ms,
        NPY_FR_us,
        NPY_FR_ns,
        NPY_FR_ps,
        NPY_FR_fs,
        NPY_FR_as,

        NPY_DATETIME_NUMUNITS = NPY_FR_as + 1,
        NPY_DATETIME_DEFAULTUNIT = NPY_FR_us,
    };

    internal partial class numpyinternal
    {


        internal static npy_intp ConvertNpyIntpValue(object val)
        {
#if NPY_INTP_64
            return Convert.ToInt64(val);
#else
            return Convert.ToInt32(val);
#endif
        }

        internal static bool Validate(NpyArray arr)
        {
            return (null != arr && npy_defs.NPY_VALID_MAGIC == arr.nob_magic_number);
        }
        internal static bool ValidateBaseArray(NpyArray arr)
        {
            return (null == arr.base_arr || npy_defs.NPY_VALID_MAGIC == arr.base_arr.nob_magic_number);
        }
        internal static bool Validate(NpyArrayIterObject it)
        {
            return (null != it && npy_defs.NPY_VALID_MAGIC == it.nob_magic_number);
        }
        internal static bool Validate(NpyArrayMultiIterObject multi)
        {
            return (null != multi && npy_defs.NPY_VALID_MAGIC == multi.nob_magic_number);
        }

        internal static bool Validate(NpyArray_Descr descr)
        {
            return (null != descr && npy_defs.NPY_VALID_MAGIC == descr.nob_magic_number);
        }

        internal static bool NpyTypeNum_ISBOOL(NPY_TYPES type)
        {
            return (type == NPY_TYPES.NPY_BOOL);
        }
        internal static bool Validate(NpyUFuncObject ufunc)
        {
            return (null != ufunc && npy_defs.NPY_VALID_MAGIC == ufunc.nob_magic_number);
        }

        internal static bool NpyTypeNum_ISUNSIGNED(NPY_TYPES type)
        {
            switch (type)
            {
                case NPY_TYPES.NPY_UBYTE:
                case NPY_TYPES.NPY_USHORT:
                case NPY_TYPES.NPY_UINT:
                case NPY_TYPES.NPY_ULONG:
                case NPY_TYPES.NPY_ULONGLONG:
                    return true;

                default:
                    return false;
            }
        }
        internal static bool NpyTypeNum_ISSIGNED(NPY_TYPES type)
        {
            switch (type)
            {
                case NPY_TYPES.NPY_BYTE:
                case NPY_TYPES.NPY_SHORT:
                case NPY_TYPES.NPY_INT:
                case NPY_TYPES.NPY_LONG:
                case NPY_TYPES.NPY_LONGLONG:
                    return true;

                default:
                    return false;
            }
        }

        internal static bool NpyTypeNum_ISINTEGER(NPY_TYPES type)
        {
            if ((type >= NPY_TYPES.NPY_BYTE) && (type <= NPY_TYPES.NPY_ULONGLONG))
                return true;

            return false;
        }

        internal static bool NpyTypeNum_ISFLOAT(NPY_TYPES type)
        {
            if ((type >= NPY_TYPES.NPY_FLOAT) && (type <= NPY_TYPES.NPY_LONGDOUBLE))
                return true;

            return false;
        }

        internal static bool NpyTypeNum_ISNUMBER(NPY_TYPES type)
        {
            if (type <= NPY_TYPES.NPY_CLONGDOUBLE)
                return true;

            return false;
        }

        internal static bool NpyTypeNum_ISSTRING(NPY_TYPES type)
        {
            switch (type)
            {
                case NPY_TYPES.NPY_STRING:
                case NPY_TYPES.NPY_UNICODE:
                    return true;

                default:
                    return false;
            }

        }

        internal static bool NpyTypeNum_ISCOMPLEX(NPY_TYPES type)
        {
            if ((type >= NPY_TYPES.NPY_CFLOAT) && (type <= NPY_TYPES.NPY_CLONGDOUBLE))
                return true;

            return false;
        }

        internal static bool NpyTypeNum_ISPYTHON(NPY_TYPES type)
        {
            switch (type)
            {
                case NPY_TYPES.NPY_LONG:
                case NPY_TYPES.NPY_DOUBLE:
                case NPY_TYPES.NPY_CDOUBLE:
                case NPY_TYPES.NPY_BOOL:
                case NPY_TYPES.NPY_OBJECT:
                    return true;

                default:
                    return false;
            }
        }

        internal static bool NpyTypeNum_ISFLEXIBLE(NPY_TYPES type)
        {
            if ((type >= NPY_TYPES.NPY_STRING) && (type <= NPY_TYPES.NPY_VOID))
                return true;

            return false;
        }

        internal static bool NpyTypeNum_ISDATETIME(NPY_TYPES type)
        {
            if ((type >= NPY_TYPES.NPY_DATETIME) && (type <= NPY_TYPES.NPY_TIMEDELTA))
                return true;

            return false;
        }

        internal static bool NpyTypeNum_ISUSERDEF(NPY_TYPES type)
        {
            if ((type >= NPY_TYPES.NPY_USERDEF) && (type < NPY_TYPES.NPY_USERDEF + numpyinternal.NpyArray_GetNumusertypes()))
                return true;

            return false;
        }

        internal static bool NpyTypeNum_ISEXTENDED(NPY_TYPES type)
        {
            return (NpyTypeNum_ISFLEXIBLE(type) || NpyTypeNum_ISUSERDEF(type));
        }

        internal static bool NpyTypeNum_ISOBJECT(NPY_TYPES type)
        {
            return (type == NPY_TYPES.NPY_OBJECT);
        }

   
    }



    internal partial class numpyinternal
    {
        internal static char NPY_LITTLE = '<';
        internal static char NPY_BIG = '>';
        internal static char NPY_NATIVE = '=';
        internal static char NPY_SWAP = 's';
        internal static char NPY_IGNORE = '|';

#if NPY_BIG_ENDIAN
        internal static char NPY_NATBYTE = NPY_BIG;
        internal static char NPY_OPPBYTE = NPY_LITTLE;
#endif
#if NPY_LITTLE_ENDIAN
        internal static char NPY_NATBYTE = NPY_LITTLE;
        internal static char NPY_OPPBYTE = NPY_BIG;
#endif

        internal static bool NpyArray_ISNBO(char arg)
        {
            return (arg != NPY_OPPBYTE);
        }
        internal static bool NpyArray_ISNBO(NpyArray_Descr descr)
        {
            return NpyArray_ISNBO(descr.byteorder);
        }
        internal static bool NpyArray_ISNBO(NpyArray arr)
        {
            return NpyArray_ISNBO(arr.descr);
        }
        internal static bool NpyArray_IsNativeByteOrder(char arg)
        {
            return NpyArray_ISNBO(arg);
        }

        internal static bool NpyArray_EquivByteorders(NpyArray_Descr b1, NpyArray_Descr b2)
        {
            return ((b1 == b2) || (NpyArray_ISNBO(b1) == NpyArray_ISNBO(b2)));
        }

        internal static NPY_SCALARKIND NpyArray_MAX(NPY_SCALARKIND a, NPY_SCALARKIND b)
        {
            return a > b ? a : b;
        }

        internal static NPY_SCALARKIND NpyArray_MIN(NPY_SCALARKIND a, NPY_SCALARKIND b)
        {
            return a < b ? a : b;
        }

    }

    [Flags]
    public enum NPYARRAYFLAGS : int
    {
        /*
        * Means c-style contiguous (last index varies the fastest). The data
        * elements right after each other.
        */
        NPY_CONTIGUOUS = 0x0001,

        /*
         * set if array is a contiguous Fortran array: the first index varies
         * the fastest in memory (strides array is reverse of C-contiguous
         * array)
         */
        NPY_FORTRAN = 0x0002,

        NPY_C_CONTIGUOUS = NPY_CONTIGUOUS,
        NPY_F_CONTIGUOUS = NPY_FORTRAN,

        /*
         * Note: all 0-d arrays are CONTIGUOUS and FORTRAN contiguous. If a
         * 1-d array is CONTIGUOUS it is also FORTRAN contiguous
         */

        /*
         * If set, the array owns the data: it will be free'd when the array
         * is deleted.
         */
        NPY_OWNDATA = 0x0004,

        /*
         * An array never has the next four set; they're only used as parameter
         * flags to the the various FromAny functions
         */

        /* Cause a cast to occur regardless of whether or not it is safe. */
        NPY_FORCECAST = 0x0010,

        /*
         * Always copy the array. Returned arrays are always CONTIGUOUS,
         * ALIGNED, and WRITEABLE.
         */
        NPY_ENSURECOPY = 0x0020,

        /* Make sure the returned array is a base-class ndarray */
        NPY_ENSUREARRAY = 0x0040,

        /*
         * Make sure that the strides are in units of the element size Needed
         * for some operations with record-arrays.
         */
        NPY_ELEMENTSTRIDES = 0x0080,

        /*
         * Array data is aligned on the appropiate memory address for the type
         * stored according to how the compiler would align things (e.g., an
         * array of integers (4 bytes each) starts on a memory address that's
         * a multiple of 4)
         */
        NPY_ALIGNED = 0x0100,

        /* Array data has the native endianness */
        NPY_NOTSWAPPED = 0x0200,

        /* Array data is writeable */
        NPY_WRITEABLE = 0x0400,

        /*
         * If this flag is set, then base contains a pointer to an array of
         * the same size that should be updated with the current contents of
         * this array when this array is deallocated
         */
        NPY_UPDATEIFCOPY = 0x1000,

        /* This flag is for the array interface */
        NPY_ARR_HAS_DESCR = 0x0800,


        NPY_BEHAVED = (NPY_ALIGNED | NPY_WRITEABLE),
        NPY_BEHAVED_NS = (NPY_ALIGNED | NPY_WRITEABLE | NPY_NOTSWAPPED),
        NPY_CARRAY = (NPY_CONTIGUOUS | NPY_BEHAVED),
        NPY_CARRAY_RO = (NPY_CONTIGUOUS | NPY_ALIGNED),
        NPY_FARRAY = (NPY_FORTRAN | NPY_BEHAVED),
        NPY_FARRAY_RO = (NPY_FORTRAN | NPY_ALIGNED),
        NPY_DEFAULT = NPY_CARRAY,
        NPY_IN_ARRAY = NPY_CARRAY_RO,
        NPY_OUT_ARRAY = NPY_CARRAY,
        NPY_INOUT_ARRAY = (NPY_CARRAY | NPY_UPDATEIFCOPY),
        NPY_IN_FARRAY = NPY_FARRAY_RO,
        NPY_OUT_FARRAY = NPY_FARRAY,
        NPY_INOUT_FARRAY = (NPY_FARRAY | NPY_UPDATEIFCOPY),

        NPY_UPDATE_ALL = (NPY_CONTIGUOUS | NPY_FORTRAN | NPY_ALIGNED),

    };


    /* These must deal with unaligned and swapped data if necessary */
    public delegate object NpyArray_GetItemFunc(npy_intp index, NpyArray npa);
    public delegate int NpyArray_SetItemFunc(npy_intp index, object value, NpyArray a);

    public delegate void NpyArray_CopySwapNFunc(VoidPtr Dest, npy_intp dstrides, VoidPtr Src, npy_intp sstrides, npy_intp len, bool swap, NpyArray npa);
    public delegate void NpyArray_CopySwapFunc(VoidPtr Dest, VoidPtr Src, bool swap, NpyArray npa);

    public delegate bool NpyArray_NonzeroFunc(VoidPtr vp, NpyArray npa);

    /*
    * These assume aligned and notswapped data -- a buffer will be used
    * before or contiguous data will be obtained
    */

    public delegate int NpyArray_CompareFunc(VoidPtr parrdp, VoidPtr pkeydp, int elSize, NpyArray npa);
    public delegate int NpyArray_ArgFunc(VoidPtr ip, npy_intp startIndex, npy_intp endIndex, ref npy_intp max_ind, NpyArray npa);

    public delegate void NpyArray_DotFunc(VoidPtr o1, npy_intp i1, VoidPtr o2, npy_intp i2, VoidPtr o3, npy_intp i3, NpyArray npa);
    public delegate void NpyArray_VectorUnaryFunc(VoidPtr Src, VoidPtr Dest, npy_intp srclen, NpyArray srcArray, NpyArray destArray);

    /*
    * XXX the ignore argument should be removed next time the API version
    * is bumped. It used to be the separator.
    */
    public delegate int NpyArray_ScanFunc(FileInfo fp, object dptr, string ignore, NpyArray_Descr a);
    public delegate int NpyArray_FromStrFunc(string s, object dptr, object[] endptr, NpyArray_Descr a);

    public delegate int NpyArray_FillFunc(VoidPtr dest, npy_intp length, NpyArray arr);

    public delegate int NpyArray_SortFunc(object o1, npy_intp i1, NpyArray a);
    public delegate int NpyArray_ArgSortFunc(object o1, VoidPtr i1, npy_intp i2, NpyArray a);
    public delegate int NpyArray_PartitionFunc(VoidPtr v, npy_intp num, npy_intp kth, npy_intp[] pivots, ref npy_intp? npiv, object not_used);
    public delegate int NpyArray_ArgPartitionFunc(VoidPtr v, VoidPtr tosort, npy_intp num, npy_intp kth, npy_intp[] pivots, ref npy_intp? npiv, object not_used);


    public delegate int NpyArray_FillWithScalarFunc(VoidPtr dest, npy_intp length, VoidPtr scalar, NpyArray a);

    public delegate NPY_SCALARKIND NpyArray_ScalarKindFunc(NpyArray a);

    public delegate void NpyArray_FastClipFunc( VoidPtr _in, npy_intp n_in, VoidPtr min, VoidPtr max, VoidPtr _out);
    public delegate void NpyArray_FastPutmaskFunc(VoidPtr _in, VoidPtr mask, npy_intp n_in, VoidPtr values, npy_intp nv);
    public delegate int  NpyArray_FastTakeFunc(VoidPtr dest, VoidPtr src, npy_intp[] indarray, npy_intp nindarray, npy_intp n_outer, npy_intp m_middle, npy_intp nelem, NPY_CLIPMODE clipmode);

}
