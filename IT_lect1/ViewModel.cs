using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT_lect1 {
    public class ViewModel {
        public ViewModel() {
            BallPlot = GetNewModel("Jumping ball");
            BallPlot.PlotType = PlotType.Cartesian;

            GraphPlot = GetNewModel("Graphs");

        }
        public PlotModel GetNewModel(string title = "") {
            var m = new PlotModel { Title = title };
            var linearAxis1 = new LinearAxis();
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            linearAxis1.MaximumPadding = 0;
            linearAxis1.MinimumPadding = 0;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            linearAxis1.Position = AxisPosition.Bottom;
            linearAxis1.PositionAtZeroCrossing = true;
            m.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MajorGridlineStyle = LineStyle.Solid;
            linearAxis2.MaximumPadding = 0;
            linearAxis2.MinimumPadding = 0;
            
            linearAxis2.MinorGridlineStyle = LineStyle.Dot;
            m.Axes.Add(linearAxis2);

            var lineAnnotation4 = new LineAnnotation();
            lineAnnotation4.Type = LineAnnotationType.Horizontal;
            lineAnnotation4.Y = 0;
            lineAnnotation4.Color = OxyColors.Black;
            lineAnnotation4.LineStyle = LineStyle.Solid;
            m.Annotations.Add(lineAnnotation4);

            return m;
        }
        public PlotModel BallPlot { get; set; }
        public PlotModel GraphPlot { get; set; }
        public void RegisterBall(Ball ball) {
            if(!BallPlot.Annotations.Contains(ball.AnnotPos))
                BallPlot.Annotations.Add(ball.AnnotPos);
            if(!BallPlot.Annotations.Contains(ball.AnnotVel))
                BallPlot.Annotations.Add(ball.AnnotVel);
            if(!BallPlot.Series.Contains(ball.SerPos))
                BallPlot.Series.Add(ball.SerPos);
            foreach(var ser in ball.SerAll) {
                if(!GraphPlot.Series.Contains(ser))
                    GraphPlot.Series.Add(ser);
            }
        }
        public void UpdateBall(double t,Ball ball,bool redraw = true) {
            ball.SynchAnnots();
            ball.AddData(t);
            if(redraw) {
                BallPlot.InvalidatePlot(true);
                GraphPlot.InvalidatePlot(true);
            }

        }
        public void ClearData(Ball ball) {
            ball.ClearData();
        }
    }
}
