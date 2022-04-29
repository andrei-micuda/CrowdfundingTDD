using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdfundingLib
{
    public class Project
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public float CurrentAmount { get; set; }
        public float GoalAmount { get; set; }
        public int Transactions { get; set; }
        public DateTimeOffset Deadline { get; set; }

        public Project(string title,
                       string description,
                       DateTimeOffset deadline, float GAmount)
        {
            Title = title;
            Description = description;
            Deadline = deadline;
            GoalAmount = GAmount;
            CurrentAmount = 0;
            Transactions = 0;
        }

        public void Fund(float amount, float balance)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Cannot fund with negative amount");
            }

            if(balance < amount)
            {
                throw new ArgumentException("Insufficient funds.");
            }
            
            if(CurrentAmount >= GoalAmount)
            {
                throw new OverflowException("Cannot fund this project, because it has already reached its goal amount of funds!");
            }

            if (DateTimeOffset.Compare(DateTimeOffset.UtcNow, Deadline) > 0)
            {
                throw new ArgumentException("Funding is no longer available. Deadline was exceded");
            }

            CurrentAmount += amount;
            Transactions++;

            if(CurrentAmount >= GoalAmount)
            {
                Console.WriteLine("The project has reached the goal amount of funds!");
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Project project &&
                   Title == project.Title &&
                   Description == project.Description &&
                   Deadline.Equals(project.Deadline) &&
                   GoalAmount == project.GoalAmount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Description, Deadline, GoalAmount);
        }
    }
}
