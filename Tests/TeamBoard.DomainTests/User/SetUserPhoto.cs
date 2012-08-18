using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamBoard.Commands;
using TeamBoard.Domain;
using TeamBoard.Events;

namespace TeamBoard.DomainTests.User
{
    [TestClass]
    public class SetUserPhotoTests: CommandSpecificationTest<SetUserPhoto>
    {
        [TestMethod]
        public void set_photo()
        {
            var id = Guid.NewGuid();
            var login = "login1";
            var password = "123456";
            var name = "user name";
            var photo = new byte[] {5,4,3,2,1};

            Assert(new CommandSpecification<SetUserPhoto>()
                {
                    Given =
                    {
                        new UserCreated(id, login, password, name)
                    },
                    When = new SetUserPhoto(id, photo, 1),
                    Expect = 
                    {
                        new UserPhotoSet(id, photo)
                    }
                });
        }

        [TestMethod]
        public void set_photo_for_not_existing_user()
        {
            var id = Guid.NewGuid();
            var photo = new byte[] { 5, 4, 3, 2, 1 };

            Assert(new FailingCommandSpecification<SetUserPhoto>()
            {
                Given =
                    {
                    },
                When = new SetUserPhoto(id, photo, 1),
                ExpectException = new EntityNotFoundException("User", id)
            });
        }
    }
}
