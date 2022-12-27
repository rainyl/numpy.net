﻿using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumpyDotNet;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NumpyLib;

#if NPY_INTP_64
using npy_intp = System.Int64;
#else
using npy_intp = System.Int32;
#endif

namespace NumpyDotNetTests
{
    [TestClass]
    public class SerializationTests : TestBaseClass
    {
        [TestMethod]
        public void test_shape_serialization_newtonsoft()
        {
            var a = np.arange(9).reshape(3, 3);
            AssertArray(a, new int [,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });

            var ashapeSerialized = SerializationHelper.SerializeNewtonsoftJSON(a.shape);

            var newShape = SerializationHelper.DeSerializeNewtonsoftJSON<shape>(ashapeSerialized);

            var b = a.reshape(newShape);
            AssertArray(b, new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });

            var c = a.reshape(newShape.iDims);
            AssertArray(c, new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });

        }

        [TestMethod]
        public void test_shape_serialization_XML()
        {
            var a = np.arange(9).reshape(3, 3);
            AssertArray(a, new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });

            var ashapeSerialized = SerializationHelper.SerializeXml(a.shape);

            var newShape = SerializationHelper.DeserializeXml<shape>(ashapeSerialized);

            var b = a.reshape(newShape);
            AssertArray(b, new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });

            var c = a.reshape(newShape.iDims);
            AssertArray(c, new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });

        }

        [TestMethod]
        public void test_dtype_serialization_newtonsoft()
        {
            var a = np.arange(9).reshape(3, 3);
            AssertArray(a, new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });

            var DtypeInSerializedFormat = a.Dtype.ToSerializable();

            var adtypeSerialized = SerializationHelper.SerializeNewtonsoftJSON(DtypeInSerializedFormat);
            var newDtypeSerializedFormat = SerializationHelper.DeSerializeNewtonsoftJSON<dtype_serializable>(adtypeSerialized);

            dtype newDtype = new dtype(newDtypeSerializedFormat);

            Assert.AreEqual(a.Dtype.TypeNum, newDtype.TypeNum);
            Assert.AreEqual(a.Dtype.str, newDtype.str);
            Assert.AreEqual(a.Dtype.alignment, newDtype.alignment);
            Assert.AreEqual(a.Dtype.ElementSize, newDtype.ElementSize);
            Assert.AreEqual(a.Dtype.Kind, newDtype.Kind);

        }

        [TestMethod]
        public void test_dtype_serialization_XML()
        {
            var a = np.arange(9).reshape(3, 3);
            AssertArray(a, new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });

            var DtypeInSerializedFormat = a.Dtype.ToSerializable();

            var adtypeSerialized = SerializationHelper.SerializeXml(DtypeInSerializedFormat);
            var newDtypeSerializedFormat = SerializationHelper.DeserializeXml<dtype_serializable>(adtypeSerialized);

            dtype newDtype = new dtype(newDtypeSerializedFormat);

            Assert.AreEqual(a.Dtype.TypeNum, newDtype.TypeNum);
            Assert.AreEqual(a.Dtype.str, newDtype.str);
            Assert.AreEqual(a.Dtype.alignment, newDtype.alignment);
            Assert.AreEqual(a.Dtype.ElementSize, newDtype.ElementSize);
            Assert.AreEqual(a.Dtype.Kind, newDtype.Kind);

        }


    }


    public static class SerializationHelper
    {
        public static T DeserializeXml<T>(this string toDeserialize)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (System.IO.StringReader textReader = new System.IO.StringReader(toDeserialize))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        public static string SerializeXml<T>(this T toSerialize)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static string SerializeNewtonsoftJSON<T>(this T toSerialize)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(toSerialize);
        }

        public static T DeSerializeNewtonsoftJSON<T>(this string toDeserialize)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(toDeserialize);
        }

    }
}


