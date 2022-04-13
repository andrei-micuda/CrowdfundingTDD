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

        [Test]
        public void FundProject_PositiveAmount_CurrentAmountUpdated()
        {
            // Arrange
            Project createdProject = Platform.AddProject(new Project("foo", "bar", DateTimeOffset.UtcNow.AddDays(7)));
            float amountToFund = 200;

            // Act
            createdProject.Fund(amountToFund);

            // Assert
            Assert.That(createdProject.CurrentAmount, Is.EqualTo(amountToFund));
        }

        [Test]
        public void FundProject_NegativeAmount_ThrowsArgumentException()
        {
            // Arrange
            Project createdProject = Platform.AddProject(new Project("foo", "bar", DateTimeOffset.UtcNow.AddDays(7)));
            float amountToFund = -200;

            // Act
            createdProject.Fund(amountToFund);

            Assert.That(
                () => Platform.AddProject(createdProject),
                Throws.InstanceOf<ArgumentException>()
                        .With.Message.EqualTo("Cannot fund with negative amount"));
        }

    }
}