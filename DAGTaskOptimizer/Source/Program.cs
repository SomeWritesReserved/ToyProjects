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
			Activity t0_A = new Activity("t0_A", 5);
			Activity t0_B = new Activity("t0_B", 100);

			Activity t1_A = new Activity("t1_A", 20);
			Activity t1_B = new Activity("t1_B", 10);
			Activity t1_C = new Activity("t1_C", 5);

			Activity t2_A = new Activity("t2_A", 10);
			Activity t2_B = new Activity("t2_B", 30);
			Activity t2_C = new Activity("t2_C", 5);

			Activity t3_A = new Activity("t3_A", 10);
			Activity t3_B = new Activity("t3_B", 15);

			Activity t4_A = new Activity("t4_A", 5);

			t4_A.Requires.AddRange(new[] { t3_A, t2_C });
			t3_B.Requires.AddRange(new[] { t2_C });
			t3_A.Requires.AddRange(new[] { t2_A, t1_B, t2_B });
			t2_C.Requires.AddRange(new[] { t1_C });
			t2_B.Requires.AddRange(new[] { t1_C });
			t2_A.Requires.AddRange(new[] { t0_A, t1_A });
			t1_C.Requires.AddRange(new[] { t0_B });
			t1_B.Requires.AddRange(new[] { t0_B });
			t1_A.Requires.AddRange(new[] { t0_B });

			List<Activity> allActivities = new List<Activity>() { t0_A, t0_B, t1_A, t1_B, t1_C, t2_A, t2_B, t2_C, t3_A, t3_B, t4_A };
			List<Activity> roots = allActivities.Where((a) => !a.Requires.Any()).ToList();
			List<Activity> sinks = new List<Activity>() { t4_A, t3_B };

			Console.WriteLine("Visit_NextMostExpensive");
			printActivities(GraphTraverser.Visit_NextMostExpensive(allActivities));

			Console.ReadKey(true);
		}

		private static void printActivities(List<Activity> activities)
		{
			activities.ForEach((a) => Console.WriteLine($"{a.Name} ({a.TimeToExecute})"));
		}

		#endregion Methods
	}

	[System.Diagnostics.DebuggerDisplay("{Name} ({TimeToExecute})")]
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

		public List<Activity> Requires { get; } = new List<Activity>();

		#endregion Properties

		#region Methods

		public override string ToString() => $"{Name} ({TimeToExecute})";

		#endregion Methods
	}

	public class GraphTraverser
	{
		#region Methods

		public static List<Activity> Visit_NextMostExpensive(List<Activity> allActivities)
		{
			List<Activity> notYetActivities = new List<Activity>(allActivities);
			List<Activity> visitOrder = new List<Activity>();

			while (notYetActivities.Any())
			{
				List<Activity> availableActivities = notYetActivities.Where((a) => a.Requires.All((ar) => visitOrder.Contains(ar))).ToList();
				Activity nextActivity = availableActivities.OrderByDescending((a) => a.TimeToExecute).First();
				visitOrder.Add(nextActivity);
				notYetActivities.Remove(nextActivity);
			}

			return visitOrder;
		}

		#endregion Methods
	}
}
