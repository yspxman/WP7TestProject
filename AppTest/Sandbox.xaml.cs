﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.Windows.Threading;

namespace AppTest
{
    public partial class Sandbox : PhoneApplicationPage
    {
        public Sandbox()
        {
            InitializeComponent();
            InterlockedTest();
            MultiThreadTest();

        }


        void MultiThreadTest()
        {
            for (int i = 0; i < 20; i++)
            {
                Thread testThread = new Thread(AddFunc);
                testThread.Start(this);
            }
        }

        public static int num1 = 1;
        public static int num2 = 99;
        public static int num3 = 0;
        public static void AddFunc(Object o)
        { 
            Sandbox s = (Sandbox)o;
            int res;
            //Interlocked.Increment(ref num);
            //res = num;
            //Interlocked.
            Thread.Sleep(1000);
            // 
            num3 = num1;
            num1 = num2;
            num2 = num3;

            num3 = num1;
            num1 = num2;
            num2 = num3;


            res = num1;
            StringBuilder s1 = new StringBuilder();

            // cannot access UI resources directly.
            Deployment.Current.Dispatcher.BeginInvoke(() => 
            {
            s.textBlock1.Text += s1.AppendFormat("num1 is :{0} \t\n", res).ToString();
            });
            
            
        }
        void InterlockedTest()
        {

            int x = 0;
            // 迭代次数为500万
            const int iterationNumber = 5000000;
            // 不采用锁的情况
            // StartNew方法 对新的 Stopwatch 实例进行初始化，将运行时间属性设置为零，然后开始测量运行时间。
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < iterationNumber; i++)
            {
                x++;
            }

            StringBuilder s1 = new StringBuilder();
            this.textBlock1.Text += s1.AppendFormat("Common increment,time is :{0} ms \t\n", sw.ElapsedMilliseconds).ToString();


            sw.Reset();
            sw.Start();
            x = 0;
            // 使用锁的情况, Interlocked提供多种线程安全的常用函数，如加减，交换，比较并交换等。
            // 这些操作都是原子操作 atomic operation
            for (int i = 0; i < iterationNumber; i++)
            {
                Interlocked.Increment(ref x);
            }
            s1.Clear();
            this.textBlock1.Text += s1.AppendFormat("Interlocked increment,time is :{0} ms \t\n", sw.ElapsedMilliseconds).ToString();
                 
        }
    }
}