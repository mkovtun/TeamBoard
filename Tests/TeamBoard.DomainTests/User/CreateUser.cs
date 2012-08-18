using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamBoard.Commands;
using TeamBoard.Domain;
using TeamBoard.Events;

namespace TeamBoard.DomainTests.User
{
    [TestClass]
    public class CreateUserTests: CommandSpecificationTest<CreateUser>
    {
        [TestMethod]
        public void create_user()
        {
            var id = Guid.NewGuid();
            var login = "login1";
            var password = "123456";
            var name = "user name";

            Assert(new CommandSpecification<CreateUser>()
                {
                    When = new CreateUser(id, login, password, name),
                    Expect = 
                    {
                        new UserCreated(id, login, password, name)
                    }
                });
        }

        [TestMethod]
        public void create_user_with_empty_id()
        {
            var id = Guid.Empty;
            var login = "login";
            var password = "123456";
            var name = "user name";

            Assert(new FailingCommandSpecification<CreateUser>()
            {
                When = new CreateUser(id, login, password, name),
                ExpectException = new ArgumentNullException("id")
            });
        }

        [TestMethod]
        public void create_user_with_empty_login()
        {
            var id = Guid.NewGuid();
            var login = "";
            var password = "123456";
            var name = "user name";

            Assert(new FailingCommandSpecification<CreateUser>()
            {
                When = new CreateUser(id, login, password, name),
                ExpectException = new ArgumentNullException("login")
            });
        }

        [TestMethod]
        public void create_user_with_empty_name()
        {
            var id = Guid.NewGuid();
            var login = "login";
            var password = "123456";
            var name = "";

            Assert(new FailingCommandSpecification<CreateUser>()
            {
                When = new CreateUser(id, login, password, name),
                ExpectException = new ArgumentNullException("name")
            });
        }

        //[TestMethod]
        //public void create_user_duplicate_login()
        //{
        //    var id = Guid.NewGuid();
        //    var login = "login1";
        //    var password = "123456";
        //    var name = "user name";

        //    Assert(new FailingCommandSpecification<CreateUser>()
        //    {
        //        Given =
        //        {
        //            new UserCreated(Guid.NewGuid(), login, "123", "abct")
        //        },
        //        When = new CreateUser(id, login, password, name),
        //        ExpectException = new DuplicateLoginException(login)
        //    });
        //}
    }
}
