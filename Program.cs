using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maze81485
{
    class Program
    {
        static void Main(string[] args)
        {
            int[][] maze = { new int[] { 1, 1, 0, 1, 1, 1},
                             new int[] { 1, 2, 0, 0, 1, 1},
                             new int[] { 1, 1, 1, 1, 2, 1},
                             new int[] { 1, 1, 1, 1, 1, 1},
                             new int[] { 1, 0, 0, 1, 1, 1},
                             new int[] { 1, 1, 1, 1, 1, 1} };

            int N = maze.Count();
            
            Point start = new Point();
            start.X = 0;
            start.Y = 0;
            Point end = new Point();
            end.X = 4;
            end.Y = 4;    
                      
            //save the path
            Point [ , ] path = new Point [N, N] ;

            BFS(maze, N, start, end, path);
            //reconstruct the path
            List<Point> res_path = ReconstructPath(maze, N, start, end, path);
            //print the path in the file Path.txt in bin folder of the project
            printPath(res_path);
        }

        private static void printPath( List<Point> res_path)
        {
            if(res_path == null)
            {
                System.IO.File.WriteAllText("Path.txt", "There isn't path!");
                Process.Start("Path.txt");
                return;
            }
            string res = "Lenght of the shortest path: " + res_path.Count.ToString() + '\n' + "The shortest path is:" + '\n';           
            foreach(Point elem in res_path)
            {                
                res += "[" + elem.X.ToString() + ", " + elem.Y.ToString() + "]" + '\n';
                if (elem != res_path.Last())
                {
                    res += "  |" + '\n';
                    res += "  V" + '\n';                    
                }
            }
            
            System.IO.File.WriteAllText("Path.txt", res);
            Process.Start("Path.txt");
        }

        private static List<Point> ReconstructPath(int[][] maze, int N, Point start, Point end, Point[,] path)
        {
            List<Point> result = new List<Point>();
            for(var at = end; at != null; at = path[at.X, at.Y])
            {
                result.Add(at);
            }
            result.Reverse();

            if(result[0] == start)
                return result;
            return null;
        }

        private static void BFS(int[][] maze, int N, Point start, Point end, Point[,] path)
        {
            //bollean array of visited
            bool[,] visited = new bool[N, N];
            InitializeVisitedArray(visited, N);
            /// check if start and end point are acceptable 
            if (! (isFree(maze, N, visited, start.X, start.Y) && isFree(maze, N, visited, end.X, end.Y)) )
                return ;

            //position of 2
            Point first_2 = new Point();
            Point second_2 = new Point();
            getPos(maze, N, first_2, second_2);            

            //Mark the source cell as visited 
            visited[start.X, start.Y] = true;

            //Create a queue for BFS 
            Queue<queueNode> q = new Queue<queueNode>();

            // Distance of start point is 0 
            queueNode s = new queueNode();
            s._Point = start;
            s._Neighbours = getNeighbours(maze, N, visited, s._Point);            
            q.Enqueue(s);  // Enqueue start point 

            while(q.Count > 0)
            {
                queueNode element = q.Dequeue();
                if(maze[element._Point.X][element._Point.Y] == 2)
                {
                    queueNode new_element = Teleport(maze, N, visited, first_2, second_2);
                    path[new_element._Point.X, new_element._Point.Y] = element._Point;
                    element = new_element;
                }
                foreach(Point p in element._Neighbours)
                {
                    if(! visited[p.X, p.Y])
                    {
                        queueNode next = new queueNode();
                        next._Point = p;
                        next._Neighbours = getNeighbours(maze, N, visited, next._Point);                    

                        q.Enqueue(next);
                        visited[next._Point.X, next._Point.Y] = true;
                        path[next._Point.X, next._Point.Y] = element._Point;
                    }
                }
            }

        }

        private static queueNode Teleport(int[][] maze, int N, bool [,] visited, Point first_2, Point second_2)
        {
            queueNode teleport = new queueNode();
            teleport._Point = second_2;
            teleport._Neighbours = getNeighbours(maze, N, visited, second_2);

            visited[first_2.X, first_2.Y] = true;
            visited[second_2.X, second_2.Y] = true;

            return teleport;
        }

        private static IList<Point> getNeighbours(int [][] maze,int N, bool [,] visited, Point point)
        {
            IList<Point> neighbours = new List<Point>();
            Point tmp_point;
            
            if (isFree(maze, N, visited, point.X, point.Y + 1)) //right
            {
                tmp_point = new Point();
                tmp_point.X = point.X;
                tmp_point.Y = point.Y + 1;
                neighbours.Add(tmp_point);
            }
            if (isFree(maze, N, visited,  point.X + 1, point.Y )) //bottom
            {
                tmp_point = new Point();
                tmp_point.X = point.X + 1;
                tmp_point.Y = point.Y ;
                neighbours.Add(tmp_point);
            }
            if (isFree(maze, N, visited, point.X, point.Y - 1)) //left
            {
                tmp_point = new Point();
                tmp_point.X = point.X;
                tmp_point.Y = point.Y - 1;
                neighbours.Add(tmp_point);
            }
            if (isFree(maze, N, visited, point.X - 1, point.Y)) //top
            {
                tmp_point = new Point();
                tmp_point.X = point.X - 1;
                tmp_point.Y = point.Y ;
                neighbours.Add(tmp_point);
            }

            return neighbours;
        }

        private static void InitializeVisitedArray(bool[,] visited, int N)
        {
            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j < N; j++)
                {
                    visited[i, j] = false;
                }
            }
        }

        public static bool isVisited(bool [,] visited, int x, int y)
        {
            return visited[x, y];
        }
        public static bool isFree(int[][] maze, int N, bool[,] visited, int x, int y )
        {
            if ((x >= 0 && x < N) && (y >= 0 && y < N) && (maze[x][y] == 1 || maze[x][y] == 2) && !isVisited(visited, x, y))
            {
                return true;
            }
            return false;
        }

        private static void getPos(int[][] maze, int N, Point first_2, Point second_2)
        {
            int count_of_2 = 0;
            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j < N; j++)
                {
                    if(maze[i][j] == 2 && count_of_2 == 0)
                    {
                        first_2.X = i;
                        first_2.Y = j;
                        count_of_2 ++;
                    }
                    else if(maze[i][j] == 2 && count_of_2 == 1)
                    {
                        second_2.X = i;
                        second_2.Y = j;
                        count_of_2++;
                        break;
                    }
                }
                if (count_of_2 == 2)
                    break;
            }
        }
    }
    public class queueNode
    {
        public Point _Point { get; set; }  // The cordinates of a point
        public IList<Point> _Neighbours { get; set; } // The cordinates of the point's neighbours
    };
    public class Point
    {
        private int x;
        private int y;       
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
    };
}
