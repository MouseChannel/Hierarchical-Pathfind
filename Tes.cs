
using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Profiling;


using System.Linq;


public class Testclass : IEquatable<Testclass>
{
    public int a;
    public int b;
    public ManualResetEvent m;
    CustomSampler sampler = CustomSampler.Create("MyCustomSampler");
    public Testclass(int a, int b)
    {
        this.a = a;
        this.b = b;

    }
 

    public bool Equals(Testclass other)
    {
        Debug.Log("quuuu");
        return a+ b == other.a + other.b;
    }

    public void HeavtWork(object aa)
    {
        Profiler.BeginThreadProfiling("1", "2");
        sampler.Begin();
        for (int i = 0; i < 99999; i++)
        {
            float a = math.exp2(math.log2(i + 1));
        }
        sampler.End();
        Profiler.EndThreadProfiling();
        m.Set();
    }

    public override int GetHashCode()
    {
        Debug.Log("hash");
        return a + b;
      
    }
}


public struct TestStruct : IEquatable<TestStruct>
{
    public int e;
    public int w;

    public int time;
    // public NativeList<int> testlist;
    public bool Equals(TestStruct other)
    {
        time++;
        Debug.Log("eq");
        return e+ w == other.e+ other.w;
        // return base.Equals(obj);
    }

    // public override bool Equals(object obj)
    // {
    //      Debug.Log("eq");
    //     return e+ w == other.e+ other.w;
    // }

    // public override bool Equals(object obj)
    // {
    //     return base.Equals(obj);
    // }

    public override int GetHashCode()
    {
        time++;
        Debug.Log("hash");
        return w;
    }

    public override string ToString()
    {
        var i = string.Empty;
        return i + w.ToString() + " " + e.ToString() + " " + time;
    }
}

[AlwaysUpdateSystem]
public partial class Tes : SystemBase
{

    private Heap<Grid> testHeap = new Heap<Grid>(200);
    private NativeList<TestStruct> test = new NativeList<TestStruct>(10, Allocator.Persistent);
    JobHandle jobHandle;
    Dictionary<Edge, int> dic = new Dictionary<Edge, int>();
    Dictionary<testJob, JobHandle> testDic = new Dictionary<testJob, JobHandle>();
    // Hashtable aww = new Hashtable();
    Edge a, b;
    UnsafeHashSet<int> tee;
    UnsafeList<int> tte;
    ManualResetEvent[] threadingpool = new ManualResetEvent[16];
    TestStruct tests;
    NativeHashSet<TestStruct> testHashset = new NativeHashSet<TestStruct>(10, Allocator.Persistent);
    CustomSampler sampler;
    List<Testclass> ts = new List<Testclass>();
    ClassHeap<Testclass> classHeap = new ClassHeap<Testclass>();
    NativeList<int> teee = new NativeList<int>(Allocator.TempJob);
    List<NativeList<int>> llll = new List<NativeList<int>>();



    protected override void OnDestroy()
    {
        testHeap.Release();
    }
    protected override void OnCreate()
    {
        sampler = CustomSampler.Create("MyCustomSampler");
        // a = new Edge { fromIndexInPath = 2 };

        // b = new Edge { fromIndexInPath = 3 };
        // tee = new UnsafeHashSet<int>(StaticData.ClusterWidth * StaticData.ClusterWidth, Allocator.Temp);
        tte = new UnsafeList<int>(StaticData.ClusterWidth * StaticData.ClusterWidth, Allocator.Temp);
        ts.Add(new Testclass(2, 3));
    }


    protected override void OnUpdate()
    {


        if (Input.GetKeyDown(KeyCode.F1))
        {
            NativeList<int> a = NativeAllocatePool.PullNativeList<int>(Allocator.Persistent);

            a.Add(1);
            
            a.Add(1);
            
            a.Add(1);
            
            a.Add(1);
            
            a.Add(1);

            Debug.Log(a.Length);
            llll.Add(a);

            NativeAllocatePool<int>.DisPlay();
            Profiler.BeginSample("myAll");
            var aa = new NativeList<int2>(Allocator.TempJob);
            Profiler.EndSample();
            var teesafd = AllocatePool.PullItem<Edge>();
            teesafd.Init(new PortalNode(), new PortalNode());

        }
        if (Input.GetKeyDown(KeyCode.F2))
        {

            NativeAllocatePool.GiveBackToPool(llll[0], Allocator.Persistent);
            llll.RemoveAt(0);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
           NativeAllocatePool<int>.DisPlay();

        }
    }

    public struct testJob : IJob, IEquatable<testJob>
    {
        public int a;
        public NativeList<int> temp;
        public int b;

        public override int GetHashCode()
        {
            Debug.Log("Hash");
            return a * b;

        }
        public override string ToString()
        {
            var aa = string.Empty;
            return aa + a.ToString() + " " + b.ToString();
        }

        public void Execute()
        {
            temp.Add(2);

        }

        public bool Equals(testJob other)
        {
            return a * b == other.a * other.b;
        }
    }
}

