using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyOrganization
{
    internal abstract class Organization
    {
        // class and variables
        private Position root;
        private int personId = 1;

        public Organization()
        {
            root = CreateOrganization();
        }

        protected abstract Position CreateOrganization();

        /**
         * hire the given person as an employee in the position that has that title
         * 
         * @param person
         * @param title
         * @return the newly filled position or empty if no position has that title
         */
        public void Hire(Name person, string title)
        {

            // is title empty?
            if (title == null)
                throw new Exception("title cannot be null");
            /* 
             * method to create employee
             * returns bool
            */
            AssignEmployee(root, title, person);

        }

        /**
         * method to create employee if position exists
         * @param enumerable
         * @param title
         * @param person
         * @return true if title exists and employee assigned or false if no title exists and not assigned
         */
        public bool AssignEmployee(Position enumerable, string title, Name person)
        {

            // checks if first position in list exists and not filled
            if (title == enumerable.GetTitle() && !enumerable.IsFilled())
            {
                //create employee because position exists
                Employee emp = new Employee(personId, person);
                // increment id
                personId++;
                // assign employee to position
                enumerable.SetEmployee(emp);
                return true;
            }
            // if first position in list exists but position is already filled
            if (title == enumerable.GetTitle() && enumerable.IsFilled())
            {
                return false;
            }

            // loop through all positions and nested positions
            foreach (Position item in enumerable.GetDirectReports())
            {

                if (item is Position nestedSet)
                {
                    // if the title exists and position is not filled
                    if (item.GetTitle() == title && !item.IsFilled())
                    {
                        // create employee because position exists
                        Employee emp = new Employee(personId, person);
                        // increase person id
                        personId++;
                        // assign employee to position
                        item.SetEmployee(emp);
                        return true;

                    }
                    // if title exists and position is filled
                    if (item.GetTitle() == title && item.IsFilled())
                    {
                        return false;
                    }

                    // iterate through nested HashSet using recursion
                    else if (AssignEmployee(nestedSet, title, person))
                    {

                        // If an employee is assigned in the recursive call, stop the recursion
                        return true;
                    }

                }

            }
            return false;

        }


        override public string ToString()
        {
            return PrintOrganization(root, "");
        }

        private string PrintOrganization(Position pos, string prefix)
        {
            StringBuilder sb = new StringBuilder(prefix + "+-" + pos.ToString() + "\n");
            foreach (Position p in pos.GetDirectReports())
            {
                sb.Append(PrintOrganization(p, prefix + "  "));
            }
            return sb.ToString();
        }
    }
}
