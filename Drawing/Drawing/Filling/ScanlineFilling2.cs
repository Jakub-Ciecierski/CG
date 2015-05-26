using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.Filling
{
    /// <summary>
    ///     Scanline filling with AET
    ///     AET update via ET
    ///     
    ///     y = smallest y coordinate that has an entry in ET
    ///     Initialize AET to empty
    ///     while ( !AET.empty() || !ET.empty()) {
    ///         move bucket ET[y] to AET
    ///         sort AET by x value
    ///         fill pixels between pairs of intersections
    ///         remove from AET edges for which ymax = y
    ///         ++y
    ///         for each edge in AET
    ///             x += 1/m
    /// </summary>
    public class ScanlineFilling2
    {
        List<int> y_mapping = new List<int>();
        int y_total_min;

        Action<int, int> PutPixel;

        public ScanlineFilling2(Action<int, int> putPixel)
        {
            PutPixel = putPixel;
        }

        public void Fill(List<Edge> edges)
        {

            List<Edge> AET = new List<Edge>();
            List<Edge> ETBucket = new List<Edge>();

            buildEdgeTable(ETBucket, edges);

            int y = ETBucket[0].min_y;

            while (AET.Count != 0 || ETBucket.Count != 0)
            {
                addActiveEdgeTable(y, AET, ETBucket);

                AET = sortActiveEdgeTable(AET);

                fillScanline(y, AET);

                updateActiveEdgeTable(y, AET);

                y++;
            }
        }

        private void buildEdgeTable(List<Edge> ETBucket, List<Edge> edges)
        {
            y_total_min = 0;

            while (edges.Count != 0)
            {
                int y_min_index = 0;
                int y_min = edges[y_min_index].min_y;

                // find the minimal y
                for (int i = 0; i < edges.Count; i++)
                {

                    Edge edge = edges[i];
                    if (y_min >= edge.min_y)
                    {
                        y_min_index = i;
                        y_min = edge.min_y;
                    }
                }

                Edge minEdge = edges[y_min_index];

                int bucketIndex = -1;
                for (int i = 0; i < y_mapping.Count; i++)
                {
                    if (y_mapping[i] == minEdge.min_y)
                    {
                        bucketIndex = i;
                        break;
                    }
                }
                if (bucketIndex < 0)
                {
                    ETBucket.Add(minEdge);
                    y_mapping.Add(minEdge.min_y);
                }
                else
                {
                    Edge edgeToAdd = ETBucket[bucketIndex];
                    while (edgeToAdd.next != null)
                    {
                        edgeToAdd = edgeToAdd.next;
                    }
                    edgeToAdd.next = minEdge;
                }

                edges.Remove(minEdge);
            }
        }

        private void addActiveEdgeTable(int y, List<Edge> AET, List<Edge> ETBucket)
        {
            int y_index = -1;
            for (int i = 0; i < y_mapping.Count; i++)
            {
                if (y_mapping[i] == y)
                {
                    y_index = i;
                    break;
                }
            }
            if (y_index >= 0)
            {
                Edge bucketEdge = ETBucket[y_index];
                AET.Add(bucketEdge);

                // remove the bucket
                ETBucket.RemoveAt(y_index);
                y_mapping.RemoveAt(y_index);
            }

        }

        private List<Edge> sortActiveEdgeTable(List<Edge> AET)
        {
            // sort by x
            return AET.OrderBy(e => e.curr_x).ToList(); // todo check
        }

        private void fillScanline(int y, List<Edge> AET)
        {
            // for each pair of intersections

            int j = 0;
            while (j < AET.Count - 1)
            {
                Edge edge1 = AET[j];
                Edge edge2 = AET[j + 1];

                for (double x = edge1.curr_x; x < edge2.curr_x; x++)
                {
                    PutPixel((int)x, y);
                }

                j += 2;
            }
            /*
            for (int i = 0; i < AET.Count - 2; i+=2)
            {
                Edge edge1 = AET[i];
                Edge edge2 = AET[i+1];

                for (double x = edge1.curr_x; x < edge2.curr_x; x++)
                {
                    PutPixel((int)x, y);
                }
            }*/
        }

        private void updateActiveEdgeTable(int y, List<Edge> AET)
        {
            // remove from AET edges for which ymax = y
            for (int i = 0; i < AET.Count; i++)
            {
                // update x
                AET[i].curr_x += AET[i].d_m;

                if (y >= AET[i].max_y)
                    AET.RemoveAt(i);
            }
        }

    }
}
