using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DAGTaskOptimizer
{
	public class Program
	{
		#region Methods

		public static void Main(string[] args)
		{
			// Critical path is 145, linear path is 215
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
			List<Activity> sinks = allActivities.Where((a) => !allActivities.Any((b) => b.Requires.Contains(a))).ToList();
			allActivities.SelectMany((a) => a.Requires.Select((b) => Tuple.Create(a.Name, b.Name))).ToList().ForEach((tuple) => Console.WriteLine($"{tuple.Item2} -> {tuple.Item1}"));

			//Console.WriteLine("NaiveVisitor_NextMostExpensive 1 core");
			//Console.WriteLine(ActivityExecuter.SimulateMultiCoreVisiting(1, new NaiveVisitor_NextMostExpensive(allActivities)));
			//Console.WriteLine(ActivityExecuter.SimulateMultiCoreVisiting(1, new NaiveVisitor_NextMostExpensive(allActivities)));
			Console.WriteLine();
			Console.WriteLine("NaiveVisitor_NextMostExpensive 2 core");
			Console.WriteLine(ActivityExecuter.SimulateMultiCoreVisiting(2, new NaiveVisitor_NextMostExpensive(allActivities)));
			Console.WriteLine(ActivityExecuter.SimulateMultiCoreVisiting(2, new NaiveVisitor_NextMostExpensive(allActivities)));

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

	public class ActivityExecuter
	{
		#region Methods

		public static int SimulateMultiCoreVisiting(int coreCount, IActivityVisitor activityVisitor)
		{
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);
			Task<Activity[]>[] visitorTasks = Enumerable.Range(0, coreCount)
				.Select((i) => Task.Run(() =>
				{
					manualResetEvent.WaitOne();
					List<Activity> visitedActivities = new List<Activity>();
					Activity activity;
					while ((activity = activityVisitor.GetNextActivityToVisit()) != null)
					{
						visitedActivities.Add(activity);
						Thread.Sleep(activity.TimeToExecute * 10);
					}
					return visitedActivities.ToArray();
				}))
				.ToArray();

			Stopwatch stopwatch = Stopwatch.StartNew();
			manualResetEvent.Set();
			Task.WaitAll(visitorTasks);
			stopwatch.Stop();
			return (int)(stopwatch.Elapsed.TotalMilliseconds / 10.0 - 15);
		}

		#endregion Methods
	}

	public interface IActivityVisitor
	{
		#region Methods

		Activity GetNextActivityToVisit();

		#endregion Methods
	}

	public class NaiveVisitor_NextMostExpensive : IActivityVisitor
	{
		#region Fields

		private readonly List<Activity> allActivities = new List<Activity>();
		private readonly List<Activity> notYetVisitedActivities = new List<Activity>();
		private readonly HashSet<Activity> vistedActivities = new HashSet<Activity>();

		#endregion Fields

		#region Constructors

		public NaiveVisitor_NextMostExpensive(IEnumerable<Activity> allActivities)
		{
			this.allActivities = allActivities.ToList();
			this.notYetVisitedActivities = allActivities.ToList();
		}

		#endregion Constructors

		#region Methods

		public Activity GetNextActivityToVisit()
		{
			lock (this.notYetVisitedActivities)
			{
				if (!this.notYetVisitedActivities.Any()) { return null; }

				List<Activity> availableActivities = this.notYetVisitedActivities
					.Where((a) => a.Requires.All((ar) => this.vistedActivities.Contains(ar)))
					.ToList();
				Activity nextActivity = availableActivities.OrderByDescending((a) => a.TimeToExecute).First();
				this.notYetVisitedActivities.Remove(nextActivity);
				this.vistedActivities.Add(nextActivity);
				return nextActivity;
			}
		}

		#endregion Methods
	}
}
