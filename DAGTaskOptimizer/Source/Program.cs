using System;
using System.Collections.Generic;
using System.Linq;

namespace DAGTaskOptimizer
{
	public class Program
	{
		#region Methods

		public static void Main(string[] args)
		{
			Activity rootA = new Activity("rootA", 5);
			Activity rootB = new Activity("rootB", 100);

			Activity rootB_t1A = new Activity("rootB_t1A", 20);
			Activity rootB_t1B = new Activity("rootB_t1B", 10);
			Activity rootB_t1C = new Activity("rootB_t1C", 5);

			Activity rootA_t2A = new Activity("rootA_t2A", 10);
			Activity rootB_t2A = new Activity("rootB_t2A", 30);
			Activity rootB_t2B = new Activity("rootB_t2B", 5);

			Activity rootAB_t3A = new Activity("rootAB_t3A", 10);

			Activity rootB_final = new Activity("rootB_final", 15);
			Activity rootAB_final = new Activity("rootAB_final", 5);

			rootA.Subsequent.AddRange(new[] { rootA_t2A });
			rootB.Subsequent.AddRange(new[] { rootB_t1A, rootB_t1B, rootB_t1C });
			rootB_t1A.Subsequent.AddRange(new[] { rootA_t2A });
			rootB_t1B.Subsequent.AddRange(new[] { rootAB_t3A });
			rootB_t1C.Subsequent.AddRange(new[] { rootB_t2A, rootB_t2B });
			rootA_t2A.Subsequent.AddRange(new[] { rootAB_t3A });
			rootB_t2A.Subsequent.AddRange(new[] { rootAB_t3A });
			rootB_t2B.Subsequent.AddRange(new[] { rootAB_final, rootB_final });
			rootAB_t3A.Subsequent.AddRange(new[] { rootAB_final });

			List<Activity> roots = new List<Activity>() { rootA, rootB };
			List<Activity> sinks = new List<Activity>() { rootAB_final, rootB_final };

			Console.WriteLine("Visit_NextMostExpensive");
			printActivities(GraphTraverser.Visit_NextMostExpensive(roots));

			Console.ReadKey(true);
		}

		private static void printActivities(List<Activity> activities)
		{
			activities.ForEach((a) => Console.WriteLine($"{a.Name} ({a.TimeToExecute})"));
		}

		#endregion Methods
	}

	public class Activity
	{
		#region Constructors

		public Activity(string name, int timeToExecute)
		{
			this.Name = name;
			this.TimeToExecute = timeToExecute;
		}

		#endregion Constructors

		#region Properties

		public string Name { get; }

		public int TimeToExecute { get; }

		public List<Activity> Subsequent { get; } = new List<Activity>();

		#endregion Properties
	}

	public class GraphTraverser
	{
		#region Methods

		public static List<Activity> Visit_NextMostExpensive(List<Activity> roots)
		{
			List<Activity> availableNodes = new List<Activity>(roots);
			List<Activity> visitOrder = new List<Activity>();

			while (availableNodes.Any())
			{
				Activity nextActivity = availableNodes.OrderByDescending((a) => a.TimeToExecute).First();
				availableNodes.Remove(nextActivity);
				availableNodes.AddRange(nextActivity.Subsequent);
				visitOrder.Add(nextActivity);
			}

			return visitOrder;
		}

		#endregion Methods
	}
}
