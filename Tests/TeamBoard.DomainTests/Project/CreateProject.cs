using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamBoard.Commands;
using TeamBoard.Domain;
using TeamBoard.Events;

namespace TeamBoard.DomainTests.Project
{
    [TestClass]
    public class CreateProjectTests : CommandSpecificationTest<CreateProject>
    {
        [TestMethod]
        public void create_project()
        {
            var projectId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var caption = "New project";

            Assert(new CommandSpecification<CreateProject>()
                {
                    Given =
                    {
                        new UserCreated(creatorId, "", "", "")
                    },
                    When = new CreateProject(projectId, creatorId, caption),
                    Expect = 
                    {
                        new ProjectCreated(projectId, caption),
                        new TeamMemberAdded(projectId, creatorId, TeamMemberRole.Owner)
                    }
                });
        }

        [TestMethod]
        public void create_project_with_not_existing_creator()
        {
            var projectId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var caption = "New project";

            Assert(new FailingCommandSpecification<CreateProject>()
            {
                When = new CreateProject(projectId, creatorId, caption),
                ExpectException = new EntityNotFoundException("User", creatorId)
            });
        }

        [TestMethod]
        public void create_project_with_empty_id()
        {
            var projectId = Guid.Empty;
            var creatorId = Guid.NewGuid();
            var caption = "New project";

            Assert(new FailingCommandSpecification<CreateProject>()
            {
                Given =
                    {
                        new UserCreated(creatorId, "", "", "")
                    },
                When = new CreateProject(projectId, creatorId, caption),
                ExpectException = new ArgumentNullException("id")
            });
        }

        [TestMethod]
        public void create_project_with_empty_caption()
        {
            var projectId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var caption = "";

            Assert(new FailingCommandSpecification<CreateProject>()
            {
                Given =
                    {
                        new UserCreated(creatorId, "", "", "")
                    },
                When = new CreateProject(projectId, creatorId, caption),
                ExpectException = new ArgumentNullException("caption")
            });
        }
    }
}
