/**
 * Prvek spojoveho seznamu pro ulozeni sousedu vrcholu grafu
 */
class Link
{
    /** Cislo souseda */
    public int neighbour;
    /** Odkaz na dalsiho souseda */
    public Link next;

    /**
	 * Vytvori novy prvek seznamu pro ulozeni souseda vrcholu grafu
	 */
    public Link(int n, Link next)
    {
        this.neighbour = n;
        this.next = next;
    }
}

/**
 * Graf pro ulozeni mapy
 */
class Graph
{
    /** Sousedi jednotlivych vrcholu (hrany) */
    public Link[] edges;

    /**
	 * Inicializuje sousedy jednotlivych vrcholu (hrany)
	 */
    public void Initialize(int vertexCount)
    {
        edges = new Link[vertexCount];
    }

    /**
	 * Prida do grafu novou obousmernou hranu
	 */
    public void AddEdge(int start, int end)
    {
        edges[start] = new Link(end, edges[start]);
        edges[end] = new Link(start, edges[end]);
    }

    public List<int> Neighbours(int v)
    {
        List<int> neighbours = new List<int>();
        Link link = edges[v];

        while(link != null)
        {
            neighbours.Add(link.neighbour);
            link = link.next;
        }

        return neighbours;
    }

    public int[] BFSTree(int s)
    {
        int[] tree = new int[edges.Length];
        for (int i = 0; i < edges.Length; i++)
            tree[i] = -1;
        int[] mark = new int[edges.Length];
        mark[s] = 1;
        Queue<int> q = new Queue<int>();
        q.Enqueue(s);

        while (q.Count > 0)
        {
            int v = q.Dequeue();
            List<int> nbs = Neighbours(v);
            for (int i = 0; i < nbs.Count; i++)
            {
                int n = nbs[i];
                if (mark[n] == 0)
                {
                    mark[n] = 1;
                    q.Enqueue(n);
                    tree[n] = v;
                }
            }
            mark[v] = 2;
        }
        return tree;
    }



    public List<int> ShortestPath(int start, int end)
    {
        int[] tree = BFSTree(end);

        if (tree[start] < 0)
            return null;

        List<int> result = new List<int>();
        int v = start;
        result.Add(v);

        while (v != end)
        {
            v = tree[v];
            result.Add(v);
        }

        return result;
    }

    public int FarthestVertex(int start)
    {
        int[] tree = BFSTree(start);
        int farthest = 0;
        int fv = 0;
        
        for(int i = 0; i < tree.Length; i++)
        {
            int temp = 0;
            int v = tree[i];

            while(v != -1)
            {
                v = tree[v];
                temp++;
            }

            if (temp > farthest) 
            {
                farthest = temp;
                fv = i;
            }
        }
        Console.WriteLine("Nejvzdálenější vrchol je vzdálený hran: " + farthest);
        return fv;
    }

    public void LoadFromFile(string filename)
    {
        string[] s = File.ReadAllLines(filename);
        char[,] input = new char[s.Length, s[0].Length];
        int countVertex = 0;

        // multidimensional array
        for(int i = 0; i < s.Length; i++)
        {
            for (int j = 0; j < s[i].Length; j++)
            {
                if(s[i][j] == 'a' || s[i][j] == 'u')
                {
                    input[i, j] = s[i][j];
                    countVertex++;
                }
            }
        }
        /* jagged array
        char[][] input = new char[s.Length][];

        for (int i = 0; i < s.Length; i++)
        {
            input[i] = new char[s[i].Length];
            for (int j = 0; j < s[i].Length; j++)
            {
                input[i][j] = s[i][j];
            }
        }
        */

        this.Initialize(countVertex);
        //možná lepší
        //for(int i = 0; i < s.Length ; i += 2)
        for (int i = 0; i < s.Length; i++)
        {
            for (int j = 0; j < s[i].Length; j++)
            {
                if (s[i][j] == 'a')
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0)
                                continue;

                            int ni = i + dx;
                            int nj = j + dy;

                            if (ni >= 0 && ni < s.Length && nj >= 0 && nj < s[i].Length && s[ni][nj] == 'a')
                            {
                                this.AddEdge(j + i * s[i].Length, nj + ni * s[i].Length);
                            }
                        }
                    }
                }
            }
        }
        
    }
}

/**
 * Hledani nejkratsi cesty v grafu
 */
public class ShortestPathSearch
{
    public static void Main(String[] args)
    {
        /*
        Graph g = new Graph();
        g.Initialize(20);
        
        //g.AddEdge(0, 1);
        //g.AddEdge(0, 5);
        //g.AddEdge(1, 7);
        //g.AddEdge(1, 2);
        //g.AddEdge(2, 8);
        //g.AddEdge(4, 9);
        

        g.AddEdge(0, 1);
        g.AddEdge(0, 5);

        g.AddEdge(1, 7);
        g.AddEdge(1, 2);

        g.AddEdge(2, 8);
        g.AddEdge(2, 7);

        g.AddEdge(4, 9);

        g.AddEdge(5, 10);

        g.AddEdge(7, 8);
        g.AddEdge(7, 12);

        g.AddEdge(8, 12);
        g.AddEdge(8, 13);
        g.AddEdge(8, 9);

        g.AddEdge(9, 13);
        g.AddEdge(9, 14);

        g.AddEdge(10, 15);

        g.AddEdge(12, 13);
        g.AddEdge(12, 17);
        g.AddEdge(12, 18);

        g.AddEdge(13, 14);
        g.AddEdge(13, 18);
        g.AddEdge(13, 19);

        g.AddEdge(14, 19);

        g.AddEdge(17, 18);

        g.AddEdge(18, 19);

        Console.WriteLine(g.FarthestVertex(5));
        */
        
        Graph g = new Graph();
        g.LoadFromFile("mapForDebug.txt");

        string input;
        do
        { 
            Console.Write("Zadejte startovní vrchol: ");
            input = Console.ReadLine();

        } while (Int32.Parse(input) < 0 && Int32.Parse(input) > g.edges.Length || g.edges[Int32.Parse(input)] == null);

        int farthest = g.FarthestVertex(Int32.Parse(input));
        Console.WriteLine(farthest);

        StreamWriter sw = new StreamWriter("result.txt");

        for(int i = 0; i < g.edges.Length; i++)
        {
            if (i % 31 == 0 && i != 0)
            {
                sw.Write("\n");
            }

            if (g.edges[i] != null)
            {
                if (i == Int32.Parse(input))
                {
                    sw.Write("s");
                }
                else if (i == farthest)
                {
                    sw.Write("k");
                }
                else
                {
                    sw.Write("a");
                }
            }
            else
            {
                sw.Write("u");
            }
        }

        sw.Flush();
        sw.Close();

        /*
        foreach (int i in g.ShortestPath(15,12))
        {
            Console.WriteLine(i);
        }
        */
        //Console.WriteLine(g.ShortestPathLength(15, 19));
    }
}