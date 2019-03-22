using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ComDll
{
    #region Interfaces
    [Guid(ComClass.InterfaceId), ComVisible(true)]
    public interface IComClass
    {
        string MyString { get; set; }

        void SendParameters([MarshalAs(UnmanagedType.LPArray)]ParamStruct[] paramArray);

        void DoSomething(string somethingToDo);
    }

    [Guid(ComClass.EventsId), ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IComClassEvents
    {
        [DispId(1)]
        void SomethingWasDone(string somethingDone);

        [DispId(2)]
        void ParamsWereSent([MarshalAs(UnmanagedType.LPArray)]ParamStruct[] paramArray, int count);
    }
    #endregion

    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(IComClassEvents))]
    [Guid(ClassId), ComVisible(true)]
    public class ComClass : IComClass
    {
        internal const string ClassId = "E65B467F-82FB-434E-9497-5ECFC5A93213";
        internal const string InterfaceId = "6A3DB6E8-8420-4F03-A96D-DA0350AF3929";
        internal const string EventsId = "83FF94F8-BA07-4612-80D6-3D92A237ECF8";
        private string _aDeed;
        private ParamStruct[] _paramArray;

        public string MyString { get; set; }

        public void DoSomething(string somethingToDo)
        {
            _aDeed = somethingToDo;
            if (null != SomethingWasDone)
            {
                SomethingWasDone.Invoke(_aDeed);
            }

            // Try sending an array.
            var paramArray = new ParamStruct[]
            {
                new ParamStruct() { Key = "Key1", Value = "Value1" },
                new ParamStruct() { Key = "Key2", Value = "Value2" }
            };
            if (null != ParamsWereSent)
            {
                ParamsWereSent.Invoke(paramArray, paramArray.Length);
            }
        }

        public void SendParameters(ParamStruct[] paramArray)
        {
            _paramArray = paramArray;
            if(null != ParamsWereSent)
            {
                ParamsWereSent.Invoke(_paramArray, _paramArray.Length);
            }
        }

        [ComVisible(false)]
        public delegate void SomethingDoneHandler(string somethingDone);
        public event SomethingDoneHandler SomethingWasDone;

        [ComVisible(false)]
        public delegate void ParamsWereSentHandler(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ParamStructArrayMarshaler))]
            ParamStruct[] paramArray,
            int count);
        public event ParamsWereSentHandler ParamsWereSent;

    }
}
