using System;
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
using System.ComponentModel;
using System.Threading;

namespace AppTest
{
    public partial class WebService : PhoneApplicationPage
    {
        String addr = "http://www.google.com/ig/api?weather=beijing";

        public class MyEventArgs : EventArgs
        {
            public int num;
            public MyEventArgs(int number)
            {
                this.num = number;
            }
        }

        public delegate void MyEventHandler(object sender, MyEventArgs e);
       
        public event MyEventHandler MyEevent;

        public WebService()
        {
            InitializeComponent();
            textBox1.Text = addr;

 
        }
        private void textBlock_addText(string s)
        {
            this.textBlock1.Text += s;


            double h1 = textBlock1.Height;
            double h2 = scrollViewer1.ActualHeight;
            double h3 = scrollViewer1.Height;
            if (textBlock1.ActualHeight - scrollViewer1.Height > 0)
            {
                textBlock1.Height = textBlock1.ActualHeight + 60;
                this.scrollViewer1.ScrollToVerticalOffset(textBlock1.ActualHeight - scrollViewer1.Height);
            }
            else
            {
                textBlock1.Height = scrollViewer1.Height;
            }
        }


        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                textBlock_addText(e.Result);
                //textBlock1.Text = e.Result;
            }

            // 注册事件，下面的两种方法是一样的
            MyEevent += this.WebService_MyEevent;
            //MyEevent += new MyEventHandler(WebService_MyEevent);

            // something happened
            if (MyEevent != null)
            {
                MyEventArgs arg = new MyEventArgs(36);
                //MyEevent.BeginInvoke(null, null, null, null);
                //compact .Net framework does not support BeginInvoke
                MyEevent(this, arg);
            }
            
        }

        void WebService_MyEevent(object sender, WebService.MyEventArgs e)
        {
            MessageBox.Show("my event is commming!!" + e.num.ToString());
            
        }


        private void buttonCall_Click(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.DownloadStringAsync(new Uri(addr));
            //wc.
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            
        }

        #region delegate 异步调用测试，失败！compact framework不支持Delegate的BeginInvoke 所以 AsyncOperation也没法用

        public delegate string MyDelegate(string s1, string s2);

        private void test_Click(object sender, RoutedEventArgs e)
        {
            MyDelegate d1 = delegateFunc;

            int x = 5 + 6;
            MessageBox.Show(x.ToString());
            //// delegate 的异步调用, ok! delegate 的异步调用winphone 不支持
            /// 
            //IAsyncResult res = d1.BeginInvoke("s1", "s2", null, this);
            //MessageBox.Show(res.ToString());
        }
        public string delegateFunc(string s1, string s2)
        {
            return null;
        }
#endregion


#region 异步调用API设计测试

        //1.这里测试几种异步API的设计，分别用delegate 异步，multi-thread，ThreadPool
        // Background worker, Timer. 基本的要求是最后的回调函数可以访问 UI resouece，  
        // 回调函数的形式上统一使用event, delegate.

        #region 公共定义， 事件， delegate
        // sheep count event和delegate的定义
        public class SheepCountEventArgs : EventArgs
        {
            public int finalNumber;
            public SheepCountEventArgs(int number)
            {
                this.finalNumber = number;
            }
        }

        public delegate void SheepCountEventHandler(object sender, SheepCountEventArgs e);
        public event SheepCountEventHandler SheepCountEevent;

        public int SheepCountFunc(int seed)
        {
            // sleep for 3 sec.
            Thread.Sleep(500);
            return seed * 1108;
        }

        void OnSheepCountEeventCompleted(object sender, SheepCountEventArgs e)
        {
            // 线程池 和新线程方案需要把task dispatch到UI线程上
            //Deployment.Current.Dispatcher.BeginInvoke(()=>
            //    {
            //        MessageBox.Show("OK， now we have " + e.finalNumber.ToString() + " sheeps!");
            //    });

            // backgroundworker 的线程是IO线程

            textBlock_addText("OK, async call completed!  now we have " + e.finalNumber.ToString() + " sheeps! \n");            
        }

        private void DelegateBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isThreadPoolThread = System.Threading.Thread.CurrentThread.IsBackground;
            //InvokeSheepCountFuncAsync1();
            InvokeSheepCountFuncAsync2();
            //InvokeSheepCountFuncAsync3();

            // 注册事件
            

            //FireEvent();
        }

        public void FireEvent()
        {
            if (SheepCountEevent != null)
            {
                Delegate[] delArray = SheepCountEevent.GetInvocationList();
                foreach (var v in delArray)
                {
                    try
                    {
                        SheepCountEventHandler handler = (SheepCountEventHandler)v;
                        handler(this, new SheepCountEventArgs(8));
                    }
                    catch
                    { 
                        // catch exceptions
                    }
                }              
            }
        }


        #endregion


        //////////////////////
        // 1. using new thread
        ///////////////////////

        #region 1. new thread
        // 测试函数
        public void InvokeSheepCountFuncAsync1()
        {
            // 只注册一个回调
            if (SheepCountEevent == null)
            {
                SheepCountEevent += OnSheepCountEeventCompleted;
            }
            
            SheepCountFuncAsync1(3);
            
        }

        //实现函数
        private void SheepCountFuncAsync1(int seed)
        {
            Thread thread = new Thread(ThreadProc);
            thread.IsBackground = true;
            thread.Start(seed);
        }

        private void ThreadProc(object obj)
        {
            int seed = (int)obj;
            try
            {
                // 由于是在后台线程中，这里就直接调用同步方法。
                int res = SheepCountFunc(seed);

                // 这里的回调最终会在非ui线程中执行
                if (SheepCountEevent != null)
                    SheepCountEevent(this, new SheepCountEventArgs(res));
            }
            catch (Exception ex)
            {
                //ShowResult(string.Format("{0} => Error: {1}", str, ex.Message));
                if (SheepCountEevent != null)
                    SheepCountEevent(this, new SheepCountEventArgs(0));
            }
        }

        #endregion

        ////////////////////////////
        // 2. using BackgroundWorker ， 使用BackgroundWorker的好处是回调可以自由的使用UI资源？why
        ///////////////////////////


        #region 2. background worker
        // 测试函数
        public void InvokeSheepCountFuncAsync2()
        {
            if (SheepCountEevent == null)
            {
                SheepCountEevent += OnSheepCountEeventCompleted;            
            }
            SheepCountFuncAsync2(3);
        }

        private BackgroundWorker worker;

        //实现函数
        private void SheepCountFuncAsync2(int seed)
        {
            bool isThreadPoolThread = System.Threading.Thread.CurrentThread.IsBackground;
            
            if (worker == null)
            {
                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);         
            }
         
            if (worker.IsBusy != true)
            {
                worker.RunWorkerAsync(seed);            
            }
            else
                MessageBox.Show("worker is busy!!!!");
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            textBlock_addText("Progress received: \n" + e.ProgressPercentage.ToString() + "\n" );
         
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool isThreadPoolThread = System.Threading.Thread.CurrentThread.IsBackground;

           // throw new NotImplementedException();
            if (e.Error == null)
            {
                if (SheepCountEevent != null)
                {
                    SheepCountEevent(this, new SheepCountEventArgs((int)e.Result));
                }           
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            bool isThreadPoolThread = System.Threading.Thread.CurrentThread.IsBackground;
            int seed = (int)e.Argument;
            int res =0;
            for (int i = 0; i < 10; i++)
            {
                // 如果异步调用已经被 cancel 了，那么直接返回 
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // 执行任务并报告进度
                    res = SheepCountFunc(seed);
                    worker.ReportProgress(i * 10);
                }
            }

            //如果任务顺利执行完成那么就报告结果
            if (e.Cancel != true)
            {
                e.Result = res;
            }
        }

        #endregion

        ////////////////////////////////
        ////3. ThreadPool
        ///////////////

        #region 3. ThreadPool

        // 测试函数
        public void InvokeSheepCountFuncAsync3()
        {
            if (SheepCountEevent == null)
            {
                SheepCountEevent += OnSheepCountEeventCompleted;
            }
            SheepCountFuncAsync3(3);
        }

        //实现函数
        private void SheepCountFuncAsync3(int seed)
        {
            bool isThreadPoolThread = System.Threading.Thread.CurrentThread.IsBackground;
            int nWorkerThreads;
            int nIOThreads;

            ThreadPool.GetMaxThreads(out nWorkerThreads, out nIOThreads);
            ThreadPool.GetMinThreads(out nWorkerThreads, out nIOThreads);
            ThreadPool.QueueUserWorkItem(ThreadPoolExecFunc, (object)4);
        }

        private void ThreadPoolExecFunc(object o)
        {
            int res = SheepCountFunc((int)o);
            if (SheepCountEevent != null)
            {
                SheepCountEventArgs a = new SheepCountEventArgs(res);
                SheepCountEevent(this, a);
            }
        }
        #endregion

        
       


#endregion


    }
}