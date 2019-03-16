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
                }
            }
            return ret;
        }

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
