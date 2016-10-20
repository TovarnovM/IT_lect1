using Microsoft.Research.Oslo;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT_lect1 {
    
    public class Ball {
        private double _x;

        public double X {
            get { return _x; }
            set { _x = value; }
        }

        private double _y;

        public double Y {
            get { return _y; }
            set { _y = value; }
        }

        private double _vx;

        public double Vx {
            get { return _vx; }
            set { _vx = value; }
        }

        private double _vy;

        public double Vy {
            get { return _vy; }
            set { _vy = value; }
        }
        public double V { get { return Math.Sqrt(Vx * Vx + Vy * Vy); } }

        public void SynchAnnots() {
            AnnotPos.X = _x;
            AnnotPos.Y = _y;
            var dp = new DataPoint(_x,_y);
            AnnotVel.StartPoint = dp;
            var dp2 = new DataPoint(_x + _vx,_y + _vy);
            AnnotVel.EndPoint = dp2;
            AnnotVel.Text = $"V = {V}";
            AnnotVel.TextPosition = dp2;

        }
        public LineSeries SerX { get; set; }
        public LineSeries SerY { get; set; }
        public LineSeries SerV { get; set; }
        public LineSeries SerP { get; set; }
        public LineSeries SerE { get; set; }
        public LineSeries SerPos { get; set; }
        public IList<LineSeries> SerAll { get; set; }



        public string Name { get; set; }
        public EllipseAnnotation AnnotPos { get; set; }
        public ArrowAnnotation AnnotVel { get; set; }
        public Ball(OxyColor color,LineStyle ls, string name = "ball") {
            Name = name;
            AnnotPos  = new EllipseAnnotation() {
                ToolTip = name,
                Width = 1,
                Height = 1,
                Fill = color

            };
            AnnotVel = new ArrowAnnotation() {
                Color = color,
                Text = "V = 0"
                
            };
            SerPos = new LineSeries() {
                Color = color,
                StrokeThickness = 1,
                Title = $"[{name}]X, m",
                LineStyle = LineStyle.Dash
            };

            SerX = new LineSeries() {
                Color = OxyColors.Green,
                StrokeThickness = 2,
                Title = $"[{name}]X, m"
            };
            SerY = new LineSeries() {
                Color = OxyColors.Blue,
                StrokeThickness = 2,
                Title = $"[{name}]Y, m",
                LineStyle = ls
            };
            SerV = new LineSeries() {
                Color = OxyColors.DarkOrange,
                StrokeThickness = 2,
                Title = $"[{name}]V, m/s",
                LineStyle = ls
            };
            SerP = new LineSeries() {
                Color = OxyColors.DarkViolet,
                StrokeThickness = 2,
                Title = $"[{name}]p, m*s",
                LineStyle = ls
            };
            SerE = new LineSeries() {
                Color = OxyColors.Green,
                StrokeThickness = 2,
                Title = $"[{name}]E, Дж",
                LineStyle = ls
            };
            SerAll = new List<LineSeries>();
            SerAll.Add(SerX);
            SerAll.Add(SerY);
            SerAll.Add(SerV);
            //SerAll.Add(SerP);
            //SerAll.Add(SerE);
        }
        public void ClearData() {
            foreach(var ser in SerAll) {
                ser.Points.Clear();
            }
            SerPos.Points.Clear();
        }
        public void AddData(double t) {
            SerPos.Points.Add(new DataPoint(X,Y));
            SerX.Points.Add(new DataPoint(t,X));
            SerY.Points.Add(new DataPoint(t,Y));
            SerV.Points.Add(new DataPoint(t,V));
            SerP.Points.Add(new DataPoint(t,V*Mass));
            SerE.Points.Add(new DataPoint(t,0.5*Mass*V*V));
        }
        public double g { get; set; } = 10;
        public double Smid { get; set; } = Math.PI * 0.25 * 0.15 * 0.15;
        public double Cx { get; set; } = 1;
        public double Ro { get; set; } = 5;
        public double Mass { get; set; } = 10;

        public Vector Vector0 {
            get {
                return new Vector(
                    X,
                    Y,
                    Vx,
                    Vy
                    );
            }
            set {
                X = value[0];
                Y = value[1];
                Vx = value[2];
                Vy = value[3];

            }
        }

        public Vector f(double t, Vector vec) {
            X = vec[0];
            Y = vec[1];
            Vx = vec[2];
            Vy = vec[3];

            var res = vec.Clone();
            res[0] = Vx;//X'
            res[1] = Vy;//Y'
            res[2] = 0;//Vx'
            res[3] = -g;//Vy'
            return res;
        }

        public void EulerStep(double dt) {
            if(Y <= 0) {
                Vy = Math.Abs(Vy) * 0.8;
            }
            Vector0 = Vector0 + f(0,Vector0) * dt;

            //X += Vx * dt;
            //Y += Vy * dt;
            //Vx += 0;
            //Vy += -g;
        }

        public void MidpointStep(double dt) {
            if(Y <= 0) {
                Vy = Math.Abs(Vy) * 0.8;
            }
            var v1 = Vector0 + f(0,Vector0) * dt*0.5;
            Vector0 = Vector0 + f(0,v1) * dt;

        }

        public void Rk4(double dt) {
            if(Y <= 0) {
                Vy = Math.Abs(Vy) *0.8;
            }
            var yn = Vector0;
            var k1 = f(0,yn);
            Vector k2 = f(0 + dt / 2.0,yn + k1 * (dt / 2.0));          
            Vector k3 = f(0 + dt / 2.0,yn + k2 * (dt / 2.0));     
            Vector k4 = f(0 + dt,yn + k3 * dt);
            Vector0 = yn + (dt / 6.0) * (k1 + 2.0 * k2 + 2.0 * k3 + k4);
            

        }


    }
}
