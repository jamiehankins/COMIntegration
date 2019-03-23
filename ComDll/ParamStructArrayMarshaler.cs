using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ComDll
{
    class ParamStructArrayMarshaler : ICustomMarshaler
    {
        private static ICustomMarshaler _marshaler = new ParamStructArrayMarshaler();
        public static ICustomMarshaler GetInstance(string cookie)
        {
            if(null == _marshaler)
            {
                _marshaler = new ParamStructArrayMarshaler();
            }
            return _marshaler;
        }

        // Try the easy way.
        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            ParamStruct[] array = null;
            if(IntPtr.Zero != pNativeData)
            {
            }
            return array;
        }

        // Here, we'll need to stuff the managed object into native memory.
        // The string will be the hard part because it has to go after the array, and the array elements
        // will need to point to them.
        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            IntPtr ret = IntPtr.Zero;
            if (null != ManagedObj)
            {
                ParamStruct[] array = ManagedObj as ParamStruct[];
                if (array.Length > 0)
                {
                    int elementSize = Marshal.SizeOf<ParamStruct>();
                    int size = 0;
                    foreach (var item in array)
                    {
                        size += GetStructLength(item);
                    }
                    ret = Marshal.AllocHGlobal(size);
                    Marshal.WriteInt32(ret, array.Length);
                    IntPtr objectLocation = ret + sizeof(int);
                    IntPtr stringLocation = IntPtr.Add(objectLocation, elementSize * array.Length);
                    foreach (var item in array)
                    {
                        // First, lay down the structure.
                        Marshal.StructureToPtr(item, objectLocation, false);
                        // Next, write Key.
                        Marshal.StructureToPtr(item.Key, stringLocation, false);
                        // Fix pointer.
                        Marshal.WriteIntPtr(objectLocation, Marshal.OffsetOf(typeof(ParamStruct), "Key").ToInt32(), stringLocation);
                        stringLocation += Marshal.SizeOf(item.Key);
                        // Next, write Value.
                        Marshal.StructureToPtr(item.Value, stringLocation, false);
                        // Fix pointer.
                        Marshal.WriteIntPtr(objectLocation, Marshal.OffsetOf(typeof(ParamStruct), "Key").ToInt32(), stringLocation);
                        stringLocation += Marshal.SizeOf(item.Value);
                    }
                }
            }
            return ret;
        }
        /*
        // This should be the easy part. We're pulling the array from native to managed.
        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            ParamStruct[] array = null;
            if(IntPtr.Zero != pNativeData)
            {
                int length = Marshal.ReadInt32(pNativeData);
                array = new ParamStruct[length];
                int elementSize = Marshal.SizeOf<ParamStruct>();
                for(int i = 0; i < length; ++i)
                {
                    // I'm sure that I'll have to deal with the strings, but I'll do that later.
                    array[i] = Marshal.PtrToStructure<ParamStruct>(pNativeData + sizeof(int) + (elementSize * i));
                }
            }
            return array;
        }

        // Here, we'll need to stuff the managed object into native memory.
        // The string will be the hard part because it has to go after the array, and the array elements
        // will need to point to them.
        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            IntPtr ret = IntPtr.Zero;
            if(null != ManagedObj)
            {
                ParamStruct[] array = ManagedObj as ParamStruct[];
                if (array.Length > 0)
                {
                    int elementSize = Marshal.SizeOf<ParamStruct>();
                    int size = 0;
                    foreach (var item in array)
                    {
                        size += GetStructLength(item);
                    }
                    ret = Marshal.AllocHGlobal(size);
                    Marshal.WriteInt32(ret, array.Length);
                    int stringOffset = elementSize * array.Length;
                    foreach(var item in array)
                    {
                        Marshal.StructureToPtr
                    }
                }
            }
            return ret;
        }
        */

        private int GetStructLength(ParamStruct item)
        {
            int ret = Marshal.SizeOf<ParamStruct>();
            ret += Marshal.SizeOf(item.Key);
            ret += Marshal.SizeOf(item.Value);
            return ret;
        }

        public void CleanUpManagedData(object ManagedObj)
        {
            throw new NotImplementedException();
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            throw new NotImplementedException();
        }

        public int GetNativeDataSize()
        {
            throw new NotImplementedException();
        }
    }
}
