using CrowdfundingLib;
using NUnit.Framework;
using System;
using System.IO;

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
            Project project = new Project("foo", "bar", DateTimeOffset.UtcNow.AddDays(7), 10000);
            Platform.AddProject(project);
            Assert.That(Platform.Projects, Has.Exactly(1).EqualTo(project));
        }

        [Test]
        public void AddProject_PastDeadline_ThrowsArgumentException()
        {
            Project project = new Project("foo", "bar", DateTimeOffset.UtcNow.AddDays(-7), 10000);
            
            Assert.That(
                () => Platform.AddProject(project),
                Throws.InstanceOf<ArgumentException>()
                        .With.Message.EqualTo("Project deadline cannot be in the past"));
        }

        [Test]
        public void FundProject_PositiveAmount_CurrentAmountUpdated()
        {
            // Arrange
            Project createdProject = Platform.AddProject(new Project("foo", "bar", DateTimeOffset.UtcNow.AddDays(7), 10000));
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
            Project createdProject = Platform.AddProject(new Project("foo", "bar", DateTimeOffset.UtcNow.AddDays(7), 10000));
            float amountToFund = -200;

            // Act & Assert

            Assert.That(
                () => createdProject.Fund(amountToFund),
                Throws.InstanceOf<ArgumentException>()
                        .With.Message.EqualTo("Cannot fund with negative amount"));
        }

        [Test]
        public void FundProject_GoalAmountReached_DisplayMessage()
        {
            //Arrange
            Project newProject = Platform.AddProject(new Project("foo", "bar", DateTimeOffset.UtcNow.AddDays(5), 540));

            float firstAmount = 100;
            float secondAmount = 440;

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //Act
            newProject.Fund(firstAmount);
            newProject.Fund(secondAmount);

            //Assert 

            Assert.AreEqual("The project has reached the goal amount of funds!\r\n", stringWriter.ToString());           
        }

        [Test]
        public void FundProject_GoalAmountOverfulfilled_ThrowsOverflowException()
        {
            //Arrange
            Project newProject = Platform.AddProject(new Project("foo", "bar", DateTimeOffset.UtcNow.AddDays(9), 810));

            float firstAmount = 300;
            float secondAmount = 510;
            float overflowAmount = 100;

            //Act
            newProject.Fund(firstAmount);
            newProject.Fund(secondAmount);

            //Assert 
            Assert.That(
                () => newProject.Fund(overflowAmount),
                Throws.InstanceOf<OverflowException>()
                        .With.Message.EqualTo("Cannot fund this project, because it has already reached its goal amount of funds!"));
        }

        [Test]
        public void FundProject_TransactionNumber_EqualsExpected()
        {
            //Arrange
            Project newProject = Platform.AddProject(new Project("foo", "bar", DateTimeOffset.UtcNow.AddDays(12), 1010));

            float firstAmount = 300;
            float secondAmount = 510.27f;
            float thirdAmount = 90.80f;
            float fourthAmount = 10.9f;
            float fifthAmount = 50.5f;

            int expectedTransactionsNumber = 5;

            //Act
            newProject.Fund(firstAmount);
            newProject.Fund(secondAmount);
            newProject.Fund(thirdAmount);
            newProject.Fund(fourthAmount);
            newProject.Fund(fifthAmount);

            //Assert 
            Assert.AreEqual(expectedTransactionsNumber, newProject.Transactions);
        }


    }
}