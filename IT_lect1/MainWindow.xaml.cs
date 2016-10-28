using Microsoft.Research.Oslo;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IT_lect1 {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            vm = new ViewModel();
            DataContext = vm;
        }
        public Ball b1 { get; set; } = new Ball(OxyColors.Green,LineStyle.Solid,"euler");
        public Ball b2 { get; set; } = new Ball(OxyColors.Red,LineStyle.Dash,"midpoint");
        public Ball b3{ get; set; } = new Ball(OxyColors.Blue,LineStyle.DashDashDotDot,"rk4");
        public ViewModel vm { get; set; }
        private IEnumerator<SolPoint> sol1;
        private IEnumerator<SolPoint> sol2;
        public double  T { get; set; }
        private void button_Click(object sender,RoutedEventArgs e) {
            b1.ClearData();
            b2.ClearData();
            b3.ClearData();
            T = 0;
            b1.X = 0;
            b1.Y = 0;
            b1.Vx = 10;
            b1.Vy = 50;
            vm.RegisterBall(b1);
            b2.X = 0;
            b2.Y = 0;
            b2.Vx = 10;
            b2.Vy = 50;
           vm.RegisterBall(b2);
            b3.X = 0;
            b3.Y = 0;
            b3.Vx = 10;
            b3.Vy = 50;
            vm.RegisterBall(b3);

            //sol1 = Ode.Euler(0,b1.Vector0,b1.f,0.1).GetEnumerator();
            //sol2 = Ode.RK45(0,b1.Vector0,b1.f,0.1).GetEnumerator();
            //b2.Vy = 50;
            vm.UpdateBall(0,b2);
            vm.UpdateBall(0,b1);
            vm.UpdateBall(0,b3);
        }
        public double dt { get; set; } = 0.05;

        private void button_Copy_Click(object sender,RoutedEventArgs e) {
            b1.EulerStep(dt);
            b2.MidpointStep(dt);
            b3.Rk4(dt);
            T += dt;

            //sol1.MoveNext();
            //sol2.MoveNext();

            //b1.Vector0 = sol1.Current.X;
            //

            //b2.Vector0 = sol2.Current.X;
            //vm.UpdateBall(sol2.Current.T,b2);

            vm.UpdateBall(T,b1);
            vm.UpdateBall(T,b2);
            vm.UpdateBall(T,b3);

        }

        private void button_Copy1_Click(object sender,RoutedEventArgs e) {
            while(T <30) {
                b1.EulerStep(dt);
                b2.MidpointStep(dt);
                b3.Rk4(dt);


                T += dt;
                vm.UpdateBall(T,b1,false);
                vm.UpdateBall(T,b2,false);
                vm.UpdateBall(T,b3,false);
            }
            vm.UpdateBall(T,b1);
            vm.UpdateBall(T,b2);
            vm.UpdateBall(T,b3);
        }

        public DispatcherTimer timer { get; set; } = new DispatcherTimer();
        private void button_Copy2_Click(object sender,RoutedEventArgs e) {
            if(timer.IsEnabled) {
                timer.Stop();
                return;
            }
            timer.Interval = TimeSpan.FromMilliseconds(100);
            var ee = new RoutedEventArgs();
            timer.Tick += (s,ess) => {
                button_Copy_Click(sender,ee);
                if(T > 30)
                    timer.Stop();
            };
            timer.Start();


        }
    }
}
