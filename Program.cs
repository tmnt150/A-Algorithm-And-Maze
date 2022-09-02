Console.WriteLine("請輸入迷宮大小: ");
int mazeSize = Convert.ToInt32(Console.ReadLine());
Point[,] maze =initMaze(mazeSize);
while (true)
{
    Console.WriteLine("請輸入障礙物的座標(格式 4 5表示 x = 4,y = 5 放入障礙物)若放置完畢輸入@結束");
    string input = Console.ReadLine();
    if (input.Equals("@")) break;
    string[] wall = input.Split(' ');
    int x = int.Parse(wall[0]);
    int y = int.Parse(wall[1]);
    if (inMaze(x, y))
    {
        maze[y, x].isRoad = false;
        Console.WriteLine("座標:X = " + x + ", Y = " + y + " 已經設定成障礙物");
    }
    else
    {
        Console.WriteLine("超出迷宮範圍了");
    }
}
Console.WriteLine("請輸入起點(格式 0 0 表示 x = 0 , y = 0 為起始點):");

Point start = null;
while(start == null)
{
    string[] startString = Console.ReadLine().Split(' ');
    int x, y;
    x = int.Parse(startString[0]);
    y = int.Parse(startString[1]);
    if (inMaze(x, y))
    {
        start = maze[y, x];
        Console.WriteLine("起點已經設定為:X = "+ x +" ,Y = "+ y);
    }
    else
    {
        Console.WriteLine("請勿把起點設在迷宮外請再輸入一次");
    }
}
start.table.cost = 0;
Point end = null;
Console.WriteLine("請輸入終點(格式 0 0 表示 x = 0 , y = 0 為終止點):");

while(end == null)
{
    string[] endString = Console.ReadLine().Split(' ');
    int x, y;
    x = int.Parse(endString[0]);
    y = int.Parse(endString[1]);
    if (inMaze(x, y))
    {
        if (maze[y, x].Equals(start))
        {
            Console.WriteLine("請勿把終點設置成起點 請再輸入一次");
        }
        else
        {
            end = maze[y, x];
            Console.WriteLine("終點已經設定為:X = " + x + " ,Y = " + y);
        }
    }
    else
    {
        Console.WriteLine("請勿把終點設在迷宮外請再輸入一次");
    }
}
mazeToString();
PriorityQueue<Point, int> queue = new PriorityQueue<Point, int>();
queue.Enqueue(start, 0);
while (queue.Count > 0)
{
    Point point = queue.Dequeue();
    if (point.Equals(end)) break;
    Point next;
    if (canMove(point.x + 1, point.y))
    {
        next = maze[point.y, point.x + 1];
        if (next.table.cost > point.table.cost + 1)
        {
            next.table.cost = point.table.cost + 1;
            next.table.from = point;
            int g = Math.Abs(end.x-next.x)+Math.Abs(end.y-next.y);
            queue.Enqueue(next, (next.table.cost + g));
        }
    }
    if (canMove(point.x - 1, point.y))
    {
        next = maze[point.y, point.x - 1];
        if (next.table.cost > point.table.cost + 1)
        {
            next.table.cost = point.table.cost + 1;
            next.table.from = point;
            int g = Math.Abs(end.x - next.x) + Math.Abs(end.y - next.y);
            queue.Enqueue(next, (next.table.cost + g));
        }
    }
    if (canMove(point.x, point.y + 1))
    {
        next = maze[point.y + 1, point.x];
        if (next.table.cost > point.table.cost + 1)
        {
            next.table.cost = point.table.cost + 1;
            next.table.from = point;
            int g = Math.Abs(end.x - next.x) + Math.Abs(end.y - next.y);
            queue.Enqueue(next, (next.table.cost + g));
        }
    }
    if (canMove(point.x, point.y - 1))
    {
        next = maze[point.y - 1, point.x];
        if (next.table.cost > point.table.cost + 1)
        {
            next.table.cost = point.table.cost + 1;
            next.table.from = point;
            int g = Math.Abs(end.x - next.x) + Math.Abs(end.y - next.y);
            queue.Enqueue(next, (next.table.cost + g));
        }
    }
}
showPath();
void showPath()
{
    if (hasPath())
    {
        Console.WriteLine("找到路徑了");
        Point now = end;
        string[,] map = new string[mazeSize, mazeSize];
        for (int y = 0; y < mazeSize; y++)
        {
            for (int x = 0; x < mazeSize; x++)
            {
                Point point = maze[y, x];
                if (point.Equals(start) || point.Equals(end))
                {
                    if (point.Equals(start))
                    {
                        map[y, x] = "1";
                    }
                    else
                    {
                        map[y, x] = "2";
                    }
                }
                else
                {
                    if (point.isRoad)
                    {
                        map[y, x] = "0";
                    }
                    else
                    {
                        map[y, x] = "X";
                    }
                }
            }
        }
        while (!now.Equals(start))
        {
            map[now.y, now.x] = "7";
            now = now.table.from;
        }
        map[now.y, now.x] = "7";
        for(int x = 0; x < mazeSize; x++)
        {
            for(int y = 0; y < mazeSize; y++)
            {
                Console.Write(map[y,x]);
            }
            Console.WriteLine();
        }
    }
    else
    {
        Console.WriteLine("找不到路徑");
    }
}
bool hasPath()
{
    return end.table.from != null;
}
bool isWall(int x,int y)
{
    return !maze[y,x].isRoad;
}
bool inMaze(int x,int y)
{
    return (x<mazeSize && y<mazeSize) &&(x >=0 && y >= 0);
}
bool canMove(int x,int y)
{
    return  inMaze(x,y)&& !isWall(x,y);
}
void mazeToString()
{
for (int y = 0; y < mazeSize; y++)
{
    for(int x = 0; x < mazeSize; x++)
    {
        Point point = maze[y,x];
            if (point.Equals(start) || point.Equals(end))
            {
                if (point.Equals(start))
                {
                    Console.Write("1");
                }
                else
                {
                    Console.Write("2");
                }
            }
            else
            {
                if (point.isRoad)
                {
                    Console.Write("0");
                }
                else
                {
                    Console.Write("X");
                }
            }
    }
    Console.WriteLine();
}
}
Point[,] initMaze(int size)
{
    Point[,] points = new Point[size,size];
    for(int x = 0; x < size; x++)
    {
        for(int y = 0; y < size; y++)
        {
            points[y, x] = new Point(x, y, true);
        }
    }
    return points;
}
class Point
{
    public int x;
    public int y;
    public Table table;
    public bool isRoad;
    public Point(int x, int y, bool isRoad)
    {
        this.x = x;
        this.y = y;
        table = new Table();
        this.isRoad = isRoad;
    }
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if(obj is Point)
        {
            Point p = (Point)obj;
            return x == p.x && y == p.y;
        }
        return false;
    }
}
class Table 
{
    public int cost = int.MaxValue;
    public Point from;
}

