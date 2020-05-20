using System.Text;
using WaterTrans.GlyphLoader.Geometry;

namespace WaterTrans.GlyphLoader.TestFonts
{
    public class GraphRenderer
    {
        public int GraphOffsetX { get; set; } = 100;
        public int GraphOffsetY { get; set; } = 100;

        public string CreateGraph(
            PathGeometry geometry,
            double baseline,
            double advanceWidth,
            double advanceHeight,
            double leftSideBearing,
            double rightSideBearing,
            double topSideBearing,
            double bottomSideBearing,
            int graphEmSize)
        {
            var miniLanguage = geometry.Figures.ToString();

            var graph = new StringBuilder();
            var glyphPath = string.Format(GraphResources.StrokePath, miniLanguage, GraphOffsetX, GraphOffsetY + baseline * graphEmSize);
            graph.AppendLine(glyphPath);

            // origin point
            graph.AppendLine(GetBaseline(baseline * graphEmSize));
            graph.AppendLine(GetOriginPoint(0, baseline * graphEmSize));

            // advance box
            graph.AppendLine(GetAdvanceBox(0, 0, advanceWidth * graphEmSize, advanceHeight * graphEmSize));

            // black box
            graph.AppendLine(GetBlackBox(leftSideBearing * graphEmSize,
                                                 topSideBearing * graphEmSize,
                                                (advanceWidth - leftSideBearing - rightSideBearing) * graphEmSize,
                                                (advanceHeight - topSideBearing - bottomSideBearing) * graphEmSize));

            foreach (var figure in geometry.Figures)
            {
                graph.AppendLine(GetCurvePoint(figure.StartPoint.X,
                    figure.StartPoint.Y + baseline * graphEmSize,
                    figure.StartPoint.X,
                    figure.StartPoint.Y));

                foreach (var segment in figure.Segments)
                {
                    if (segment is WaterTrans.GlyphLoader.Geometry.LineSegment l)
                    {
                        graph.AppendLine(GetCurvePoint(l.Point.X,
                            l.Point.Y + baseline * graphEmSize,
                            l.Point.X,
                            l.Point.Y));
                    }
                    else if (segment is WaterTrans.GlyphLoader.Geometry.QuadraticBezierSegment q)
                    {
                        graph.AppendLine(GetControlPoint(q.Point1.X,
                            q.Point1.Y + baseline * graphEmSize,
                            q.Point1.X,
                            q.Point1.Y));

                        graph.AppendLine(GetCurvePoint(q.Point2.X,
                            q.Point2.Y + baseline * graphEmSize,
                            q.Point2.X,
                            q.Point2.Y));
                    }
                    else if (segment is WaterTrans.GlyphLoader.Geometry.BezierSegment b)
                    {
                        graph.AppendLine(GetControlPoint(b.Point1.X,
                            b.Point1.Y + baseline * graphEmSize,
                            b.Point1.X,
                            b.Point1.Y));

                        graph.AppendLine(GetControlPoint(b.Point2.X,
                            b.Point2.Y + baseline * graphEmSize,
                            b.Point2.X,
                            b.Point2.Y));

                        graph.AppendLine(GetCurvePoint(b.Point3.X,
                            b.Point3.Y + baseline * graphEmSize,
                            b.Point3.X,
                            b.Point3.Y));
                    }
                }
            }

            return string.Format(GraphResources.Grid, graph.ToString());
        }

        public string GetBlackBox(double x, double y, double width, double height)
        {
            return string.Format(GraphResources.BlackBox, x + GraphOffsetX, y + GraphOffsetY, width, height);
        }

        public string GetAdvanceBox(double x, double y, double width, double height)
        {
            return string.Format(GraphResources.AdvanceBox, x + GraphOffsetX, y + GraphOffsetY, width, height);
        }

        public string GetOriginPoint(double x, double y)
        {
            return string.Format(GraphResources.OriginPoint, x + GraphOffsetX, y + GraphOffsetY, 0, 0);
        }

        public string GetBaseline(double y)
        {
            return string.Format(GraphResources.Baseline, y + GraphOffsetY);
        }

        public string GetCurvePoint(double x, double y, double coodinateX, double doodinateY)
        {
            return string.Format(GraphResources.CurvePoint, x + GraphOffsetX, y + GraphOffsetY, coodinateX, doodinateY);
        }

        public string GetControlPoint(double x, double y, double coodinateX, double doodinateY)
        {
            return string.Format(GraphResources.ControlPoint, x + GraphOffsetX, y + GraphOffsetY, coodinateX, doodinateY);
        }
    }
}
