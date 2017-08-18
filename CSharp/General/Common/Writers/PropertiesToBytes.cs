using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Common.Writers
{
    /// <summary>
    /// Contains the <see cref="WriteClassPropertiesToBinFile{T}(string, T)"/> 
    /// method to write a classes properties to a binary file as bytes
    /// </summary>
    public static class PropertiesToBytes
    {
        /// <summary>
        /// Iterates over an objects properties, writing each property as bytes to a specified bin file
        /// </summary>
        /// <typeparam name="T">The class type</typeparam>
        /// <param name="filePath">File path to .bin file</param>
        /// <param name="obj">Object to iterate over</param>
        public static void WriteClassPropertiesToBinFile<T>(string filePath, T obj) where T : new()
        {
            // get class objects property info
            PropertyInfo[] properties = obj.GetType().GetProperties();

            // create binary writer with file path
            using (var binWriter = new BinaryWriter(new FileStream(filePath, FileMode.Append)))
            {
                // loop over properties
                foreach (var prop in properties)
                {
                    // create new object of type T for reference
                    T newObj = new T();

                    // get the value of the property
                    object value = prop.GetValue(newObj);

                    // get the type of the value
                    Type propType = prop.PropertyType;

                    // get appropriate action
                    Action<BinaryWriter, object> action = _actions[propType];

                    // execute action
                    action(binWriter, value);
                }
            }
        }

        /// <summary>
        /// Holds the appropriate action based on the properties type
        /// </summary>
        private static Dictionary<Type, Action<BinaryWriter, object>> _actions = new Dictionary<Type, Action<BinaryWriter, object>>()
        {
            // primitives
            { typeof(char), (w, v) => w.Write((char)v) },
            { typeof(bool), (w, v) => w.Write((bool)v) },
            { typeof(sbyte), (w, v) => w.Write((sbyte)v) },
            { typeof(byte), (w, v) => w.Write((byte)v) },
            { typeof(short), (w, v) => w.Write((short)v) },
            { typeof(ushort), (w, v) => w.Write((ushort)v) },
            { typeof(int), (w, v) => w.Write((int)v) },
            { typeof(uint), (w, v) => w.Write((uint)v) },
            { typeof(Int64), (w, v) => w.Write((Int64)v) },
            { typeof(UInt64), (w, v) => w.Write((UInt64)v) },
            { typeof(float), (w, v) => w.Write((float)v) },
            { typeof(double), (w, v) => w.Write((double)v) },

            // lists of primitives
            { typeof(List<char>), (w, v) => writeList(w, v as List<char>) },
            { typeof(List<bool>), (w, v) => writeList(w, v as List<bool>) },
            { typeof(List<sbyte>), (w, v) => writeList(w, v as List<sbyte>) },
            { typeof(List<byte>), (w, v) => writeList(w, v as List<byte>) },
            { typeof(List<short>), (w, v) => writeList(w, v as List<short>) },
            { typeof(List<ushort>), (w, v) => writeList(w, v as List<ushort>) },
            { typeof(List<int>), (w, v) => writeList(w, v as List<int>) },
            { typeof(List<uint>), (w, v) => writeList(w, v as List<uint>) },
            { typeof(List<Int64>), (w, v) => writeList(w, v as List<Int64>) },
            { typeof(List<UInt64>), (w, v) => writeList(w, v as List<UInt64>) },
            { typeof(List<float>), (w, v) => writeList(w, v as List<float>) },
            { typeof(List<double>), (w, v) => writeList(w, v as List<double>) },
        };

        /// <summary>
        /// If the property was a list, write each value in the list
        /// </summary>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="w">The <see cref="BinaryWriter"/> that writes the value</param>
        /// <param name="list">The list that holds the values</param>
        private static void writeList<T>(BinaryWriter w, List<T> list)
        {
            // if list is null, something broke!
            if (list == null)
                Debugger.Break();

            // write each value in the list
            foreach (var val in list)
            {
                // get appropriate action
                Action<BinaryWriter, object> action = _actions[val.GetType()];

                // execute action
                action(w, val);
            }
        }
    }
}
