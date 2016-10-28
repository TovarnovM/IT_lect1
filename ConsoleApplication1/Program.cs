using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    public class Rocket {
        public double M,M0,Cx,X,Y,V,P,Tm,Sm,mu, Tetta0;
        public int N = 4;
        public double GetP(double t) {
            if(t > Tm)
                return 0;
            else
                return P;
        }


        public double GetTetta(double t) {
            return -t* Tetta0 / 10 + Tetta0; 
        }

        
        public Rocket(double tetta) {
            M0 = 100;
            M = M0;
            Cx = 0.44;
            X = 0;
            Y = 1;
            V = 0;
            P = 3000;
            Tm = 0.5;
            Sm = 122.0 * 122.0 * 3.14 * 0.25 / 1000000;
            mu = 0.5;
            Tetta0 = tetta;
        }
        public double[] GetY() {
            double[] res = new double[4];
            res[0] = M;
            res[1] = X;
            res[2] = Y;
            res[3] = V;
            return res;

        }

        public void SetY(double t, double[] y) {
            M = y[0];
            X = y[1];
            Y = y[2];
            V = y[3];


        }


        public double[] f(double t, double[] y) {
            double[] res = new double[4];

            SetY(t,y);
            //dM/dT
            if(t < Tm)
                res[0] = -M0 * mu / Tm;
            else
                res[0] = 0;

            //dx/dt
            res[1] = V * Math.Cos(GetTetta(t));
            //dy/dt
            res[2] = V * Math.Sin(GetTetta(t));

            //dV/dt
            res[3] = GetP(t) - Cx * 1.204 * V * V * 0.5 * Sm - 9.8 * Math.Sin(GetTetta(t));


            return res;
        }

    }


    class Program {

        static void Main(string[] args) {
            Rocket r;
            r = new Rocket(45*3.14 / 180);

            List<double[]> otvety = new List<double[]>();

            
            double[] yn = r.GetY();
            double dt = 0.01;
            double t = 0;
            otvety.Add(yn);

            while(r.Y>0 && t<1) {
                double[] yn1 = new double[r.N]; //Y_n+1
                double[] dyn = r.f(t,yn);       //dYn/dt
                for(int i = 0; i < r.N; i++) {
                    yn1[i] = yn[i] + dt * dyn[i];
                }
                t = t + dt;
                r.SetY(t,yn1);
                yn = yn1;
                otvety.Add(yn);
            }

            foreach(var item in otvety) {
                Console.WriteLine($"M = {item[0]}; X = {item[1]}; Y = {item[2]}; V = {item[3]}");
            }

            Console.WriteLine($"r.M {r.Tetta0}");
            Console.ReadLine();


        }
    }
}
