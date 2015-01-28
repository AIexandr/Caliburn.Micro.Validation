using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Caliburn.Micro.Validation.Utils
{
  internal class Strings
  {
    internal static string Agregate(string delimiter, params object[] objects)
    {
      if (objects == null) return "";
      objects = objects.Where(x => x != null).ToArray();
      objects = objects.Where(x => !(x is string) || !string.IsNullOrWhiteSpace(x as string)).ToArray();
      if (objects.Count() == 0) return "";

      return objects.Select(x => x.ToString()).Aggregate((c, v) => string.Format("{0}{1}{2}", c, delimiter, v));
    }
  }

  public static class ExpressionExtensions
  {
    internal static string GetPropertyFullName(this Expression propertyExpression)
    {
      if (propertyExpression is MemberExpression)
        return GetPropertyName(propertyExpression as MemberExpression);
      else if (propertyExpression is UnaryExpression)
        return GetPropertyName((propertyExpression as UnaryExpression).Operand as MemberExpression);
      else if (propertyExpression is LambdaExpression)
      {
        return GetPropertyFullName((propertyExpression as LambdaExpression).Body);
      }
      else
        throw new ApplicationException(string.Format("Expression: {0} is not MemberExpression", propertyExpression));
    }
    static string GetPropertyName(MemberExpression me)
    {
      string propertyName = me.Member.Name;

      if (me.Expression.NodeType != ExpressionType.Parameter
          && me.Expression.NodeType != ExpressionType.TypeAs
          && me.Expression.NodeType != ExpressionType.Constant)
      {
        propertyName = GetPropertyName(me.Expression as MemberExpression) + "." + propertyName;
      }

      return propertyName;
    }
  }

  public class DeferredExecutor
  {
    public int ExecutionTimeoutMilliseconds { get; set; }
    System.Action m_ExecutionAction = null;
    bool m_Started = false;
    object m_Locker = new object();
    DateTime? m_TimeToExecute = null;
    public string Name { get; set; }

    public int ExecuteCallsCount { get; private set; }

    public DeferredExecutor(System.Action executionAction, int executionTimeoutMilliseconds = 500)
    {
      Name = "(unnamed)";

      if (executionAction == null)
      {
        throw new ArgumentNullException("executionAction");
      }

      ExecutionTimeoutMilliseconds = executionTimeoutMilliseconds;
      m_ExecutionAction = executionAction;
    }

    bool m_NeedRestart = false;
    public void Execute(int? executionTimeoutMilliseconds = null)
    {
      lock (m_Locker)
      {
        ExecuteCallsCount++;

        if (m_TimeToExecute == null) m_TimeToExecute = DateTime.Now;
        if (executionTimeoutMilliseconds == null) executionTimeoutMilliseconds = ExecutionTimeoutMilliseconds;
        m_TimeToExecute = DateTime.Now.AddMilliseconds(executionTimeoutMilliseconds.Value);
        m_NeedRestart = true;

        if (!m_Started)
        {
          m_Started = true;

          ThreadPool.QueueUserWorkItem(new WaitCallback((object o) =>
          {
            try
            {
              while (DateTime.Now <= m_TimeToExecute.Value)
              {
                Thread.Sleep(1);
              }
              if (m_ExecutionAction == null) throw new ApplicationException(string.Format("m_ExecutionAction == null in deferred executor {0}", Name));

              m_NeedRestart = false;
              try
              {
                m_ExecutionAction();
              }
              catch (Exception ex) { }
              finally
              {
                m_Started = false;
                m_TimeToExecute = null;
                ExecuteCallsCount = 0;
              }

              if (m_NeedRestart) Execute();
            }
            catch (Exception ex) { }
          }));
        }
      }
    }
  }

  public class Deferred
  {
    private Deferred() { }

    private static Dictionary<System.Action, DeferredExecutor> m_ActionToExecutor = new Dictionary<System.Action, DeferredExecutor>();
    public static void Execute(System.Action action, int executionTimeoutMilliseconds = 500)
    {
      if (action == null)
      {
        throw new ArgumentNullException("action");
      }

      DeferredExecutor executor = null;

      lock (m_ActionToExecutor)
      {
        if (!m_ActionToExecutor.ContainsKey(action))
        {
          m_ActionToExecutor.Add(action, new DeferredExecutor(action, executionTimeoutMilliseconds));
        }

        executor = m_ActionToExecutor[action];
      }

      executor.ExecutionTimeoutMilliseconds = executionTimeoutMilliseconds;
      executor.Execute();
    }
  }
}
