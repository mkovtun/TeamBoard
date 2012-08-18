using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamBoard.Commands;
using TeamBoard.DomainServices;
using TeamBoard.Events;

namespace TeamBoard.DomainTests
{
    [TestClass]
    public abstract class CommandSpecificationTest<TCommand>
        where TCommand : ICommand
    {
        protected Exception Cought;
        protected InMemoryEventStore eventStore;

        [TestInitialize]
        public virtual void SetUp()
        {
            Cought = null;
            eventStore = new InMemoryEventStore();
        }

        protected void Assert(ICommandSpec<TCommand> specification)
        {
            eventStore.SetupEventsHistory(specification.Given);

            try
            {
                CommandsDispatcher.Dispatch(specification.When, eventStore);
            }
            catch (Exception e)
            {
                Cought = e;
            }

            // if exception is expected
            if (specification is FailingCommandSpecification<TCommand>)
            {
                var expected = (specification as FailingCommandSpecification<TCommand>).ExpectException;
                if (Cought == null)
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Expected exception was not cought");
                    return;
                }
                if ((Cought is IStructuralEquatable) && !Cought.Equals(expected))
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Cought exception is not the one that was expected: \n{0}\n\n{1}", expected, Cought);
                    return;
                }
                if (!(Cought is IStructuralEquatable) && Cought.GetType() != expected.GetType())
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Cought exception is not the one that was expected: {0} {1}", expected, Cought);
                    return;
                }

                return;
            }

            // If there's unexpected exception
            if (Cought != null)
            {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Unexpected exception was thrown: {0}", Cought.Message);
                return;
            }

            var spec = specification as CommandSpecification<TCommand>;
            // if regular command spec
            if (spec != null)
            {
                var producedEvents = eventStore.NewEvents;
                var expectedEvents = spec.Expect;

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(AssertEvents.AreSame(producedEvents, expectedEvents));
                return;
            }
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Unknown specification type");
        }
    }

    public static class AssertEvents
    {
        public static bool AreSame(IEnumerable<IEvent> actual, IEnumerable<IEvent> expected)
        {
            var actualEnumerator = actual.GetEnumerator();
            var expectedEnumerator = expected.GetEnumerator();

            while (true)
            {
                var actualOver = actualEnumerator.MoveNext();
                var expectedOver = expectedEnumerator.MoveNext();
                if (actualOver != expectedOver)
                    return false;
                if (actualOver == false)
                    return true;
                if (!actualEnumerator.Current.Equals(expectedEnumerator.Current))
                    return false;
            }
        }
    }

    public interface ICommandSpec<T>
        where T : ICommand
    {
        List<IEvent> Given { get; }
        T When { get; }
    }

    public class CommandSpecification<T> : ICommandSpec<T>
        where T : ICommand
    {
        public List<IEvent> Given { get; set; }
        public T When { get; set; }
        public List<IEvent> Expect { get; set; }

        public CommandSpecification()
        {
            this.Given = new List<IEvent>();
            this.Expect = new List<IEvent>();
        }
    }

    public class FailingCommandSpecification<T> : ICommandSpec<T>
        where T : ICommand
    {
        public List<IEvent> Given { get; set; }
        public T When { get; set; }
        public Exception ExpectException { get; set; }

        public FailingCommandSpecification()
        {
            this.Given = new List<IEvent>();
        }
    }

}
