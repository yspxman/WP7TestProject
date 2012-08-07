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
            InterLockedTest interlockedTest = new InterLockedTest();
            interlockedTest.InterlockedTest(this);
            interlockedTest.MultiThreadTest(this);
 
            DataContext = this;


            this.textBlock1.Text += AppDomain.CurrentDomain.FriendlyName;

            //WP 或者说silverlight不支持创建新的AppDomain, AppDomain.CreateDomain
            //AppDomain.
        }
        

        /// <summary>
        /// 控件的command属性需要和Icommand的子类绑定
        /// </summary>
        private ICommand _testCommand;
        public ICommand TestCommand
        {
            get
            {
                //或者执行使用delegate方式写方法代码，不用传方法名
                return _testCommand = new RelayCommand(TestMethod);
            }
        }

        public void TestMethod()
        {
            MessageBox.Show("Msg from Command Binding");
        }
    }


    #region ICommand Test
    public class RelayCommand : ICommand
    {
        private Action _handler;

        //Action是一个委托方法，所有的放到都可以传递进来，在Execute中执行。
        public RelayCommand(Action handler)
        {
            this._handler = handler;
        }
        //如果返回false，绑定的控件就会呈现不可使用的效果
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _handler();
        }
    }

    #endregion

    #region 多线程安全类 interlocked 类测试
    public class InterLockedTest
    {
        
       public void MultiThreadTest(Sandbox sb)
        {
            for (int i = 0; i < 20; i++)
            {
                Thread testThread = new Thread(ThreadAddFunc);
                testThread.Start(sb);
            }
        }

        public static int num1 = 1;
        public static int num2 = 99;
        public static int num3 = 0;
        public static void ThreadAddFunc(Object o)
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

      public  void InterlockedTest(Sandbox sb)
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
            sb.textBlock1.Text += s1.AppendFormat("Common increment,time is :{0} ms \t\n", sw.ElapsedMilliseconds).ToString();


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
            sb.textBlock1.Text += s1.AppendFormat("Interlocked increment,time is :{0} ms \t\n", sw.ElapsedMilliseconds).ToString();

        }
       
    }

     #endregion
}