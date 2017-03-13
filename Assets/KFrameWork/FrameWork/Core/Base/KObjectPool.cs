﻿using System;
using System.Collections;
using System.Collections.Generic;
using KFrameWork;
using UnityEngine;

namespace KUtils
{
    /// <summary>
    /// 对象池接口，尽量不要在外部持有ipool对象的引用，有的话 请手动保证对象在被清楚的时候对象
    /// </summary>
    public interface IPool
    {
        void RemoveToPool();
        void RemovedFromPool();
    }

    /// <summary>
    /// 引用类型对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// 
    [SingleTon]
    public sealed class KObjectPool
    {
        public static KObjectPool mIns;

        public const int EachRemoveCount = 5;

        public const float RmovedDelta = 10f;

        private Dictionary<Type, List<System.Object>> queue = new Dictionary<Type, List<System.Object>>(16);

        private Dictionary<Type,List<float> >  deltalist = new Dictionary<Type,List<float>>(16);


        public void Push(System.Object data)
        {
            Type tp = data.GetType();
            if (!this.queue.ContainsKey(tp))
            {
                this.queue.Add(tp, new List<System.Object>(8));
                this.deltalist.Add(tp, new List<float>(8));
            }

#if UNITY_EDITOR
            if (this.queue[tp].TryAdd(data))
#else
                this.queue[tp].Add(data);
#endif
            {
                if (data is IPool)
                {
                    (data as IPool).RemoveToPool();
                }

                //if (FrameWorkConfig.Open_DEBUG)
                //{
                //    LogMgr.LogFormat("{0}进入缓存池 ", data);
                //}

                this.deltalist[tp].Add(Time.realtimeSinceStartup);
            }
#if UNITY_EDITOR
            else
            {
                LogMgr.LogErrorFormat("重复添加 :{0}",data);
            }
#endif
        }

        [SceneLeave]
        public  static void SceneRemoveUnUsed(int level)
        {
            if(KObjectPool.mIns != null)
            {
                KObjectPool.mIns.RemoveSomeOlded();
            }
        }

        public void Destroy()
        {
            FrameworkAttRegister.DestroyStaticAttEvent(MainLoopEvent.OnLevelLeaved, typeof(KObjectPool), "SceneRemoveUnUsed");
        }

        private void RemoveSomeOlded()
        {
            float now = UnityEngine.Time.realtimeSinceStartup;
 
            int removedCount = 0;

            var en = this.queue.GetEnumerator();
            while (en.MoveNext())
            {
                var kv = en.Current;
                List<float> list = this.deltalist[kv.Key];
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (now - list[i] > RmovedDelta)
                    {
                        list.RemoveAt(i);
                        object kvvalue = kv.Value[i];
                        if (kvvalue is IPool)
                        {
                            (kvvalue as IPool).RemoveToPool();
                        }
                        kv.Value.RemoveAt(i);
                        removedCount++;

                        if (removedCount >= EachRemoveCount)
                            break;
                    }
                }
            }
        }

        public T Seek<T,U>(Func<T,U,int> seekFunc,U userdata,int seekTimes =-1) where T:IPool
        {
            Type tp = typeof(T);
            if (!this.queue.ContainsKey(tp))
            {
                this.queue.Add(tp, new List<System.Object>(8));
                this.deltalist.Add(tp, new List<float>(8));
            }

            List<System.Object> list = this.queue[tp];

            if (list.Count == 0)
            {
                return default(T);
            }
            else
            {
                T nearst = default(T);
                int? worthValue = null;
                for(int i =0; i < list.Count;++i )
                {
                    if(seekTimes == 0)
                    {
                        break;
                    }
                    else
                    {
                        T data =(T)list[i];
                        int currentworth =Math.Abs(seekFunc(data,userdata));
                        if(worthValue.HasValue)
                        {
                            if(currentworth == 0)
                            {
                                nearst = data;
                                break;
                            }
                            else if(worthValue > currentworth)
                            {
                                worthValue  = currentworth;
                                nearst =data;
                            }

                        }else
                        {
                            worthValue = currentworth;
                            nearst = data;
                        }
                        seekTimes--;
                    }
                }

                int index = list.IndexOf(nearst);
                if (index != -1)
                {
                    list.RemoveAt(index);
                    this.deltalist[tp].RemoveAt(index);
                }

                return nearst;
            }
        }

        public void Clear()
        {
            this.queue.Clear();
            this.deltalist.Clear();
        }

        /// <summary>
        /// Pop this instance.(不使用New（） 因为这个会调用反射的Activator.CreateInstance 产生GC)
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Pop<T>()
        {
            Type tp = typeof(T);
            if (!this.queue.ContainsKey(tp))
            {
                this.queue.Add(tp, new List<System.Object>(8));
                this.deltalist.Add(tp, new List<float>(8));
            }

           List<System.Object> list = this.queue[tp];

           if (list.Count == 0)
            {
                return default(T);
            }
            else
            {
                System.Object first = list[0];
                list.RemoveAt(0);
                this.deltalist[tp].RemoveAt(0);
                //if (FrameWorkConfig.Open_DEBUG)
                //{
                //    LogMgr.LogFormat("{0}离开缓存缓存池 ", first);
                //}
                return (T)first;
            }
        }

    }
}

