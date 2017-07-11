using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UsingPointerWithoutUnsafe
{
    class Program
    {
        public class SomethingClass
        {
            public string testString = "Nice";
            public int first;
        }

        public struct Struct_t
        {
            public int value;
            public int value2;
            public Struct_t(int Value, int Value22)
            {
                value = 10;
                value2 = 1122;
            }
        }

        // C#에서 레퍼런스 객체 할당
        static public void Case1()
        {
            var testObject = new SomethingClass();
            testObject.first = 1212;
        }

        // 포인터를 통해 힙에 가변할당 방법
        static public void Case2()
        {
            IntPtr test = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(test, 10);

            int adress = test.ToInt32();

            Marshal.FreeHGlobal(test);
        }

        // Struct(Value) 타입을 힙으로 복사(일종에 박싱)해서 사용하기
        // Unsafe 없이는 실제 스택에 있는 메모리에 접근 할 수 없다.
        static public void Case3()
        {
            IntPtr test = Marshal.AllocHGlobal(sizeof(int));

            Struct_t a = new Struct_t();
            Marshal.StructureToPtr(a, test, true);

            Marshal.WriteInt32(test, 10);

            a = Marshal.PtrToStructure<Struct_t>(test);

            Marshal.FreeHGlobal(test);
        }

        // 포인터를 이용해서 힙에 있는 Reference 객체 수정하기
        static public void Case4()
        {
            var testObject = new SomethingClass();
            testObject.first = 1212;
            
            var gcHandler = GCHandle.Alloc(testObject);
            IntPtr pointer = (IntPtr)gcHandler;

            int valueAdress = Marshal.ReadInt32(pointer);
            IntPtr valuePtr = new IntPtr(valueAdress);
            
            Marshal.WriteInt32(valuePtr, 8, 100);
        }

        static void Main(string[] args)
        {
            Case1();
            Case2();
            Case3();
            Case4();
        }
    }
}
