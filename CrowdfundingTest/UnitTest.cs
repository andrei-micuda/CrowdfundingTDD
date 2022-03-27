using CrowdfundingLib;
using NUnit.Framework;
using System;

namespace CrowdfundingTest
{
    public class Tests
    {
        public CrowdfundingPlatform Platform { get; set; }
        [SetUp]
        public void Setup()
        {
            Platform = new CrowdfundingPlatform();
        }

        [Test]
        public void AddProject_ValidProject_PresentInPlatform()
        {
            Project project = new Project("foo", "bar", DateTimeOffset.UtcNow.AddDays(7));
            Platform.AddProject(project);
            Assert.That(Platform.Projects, Has.Exactly(1).EqualTo(project));
        }

        [Test]
        public void AddProject_PastDeadline_ThrowsArgumentException()
        {
            Project project = new Project("foo", "bar", DateTimeOffset.UtcNow.AddDays(-7));
            
            Assert.That(
                () => Platform.AddProject(project),
                Throws.InstanceOf<ArgumentException>()
                        .With.Message.EqualTo("Project deadline cannot be in the past"));
        }
    }
}