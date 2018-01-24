using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TechJobsConsole
{
    class JobData
    {
        //each dictionary in AllJobs has the format: 
        //name: Social Media Manager
        //employer: Excy
        //location: Seattle
        //position type: Technical Writer
        //core competency: Non-coding

        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();
        static bool IsDataLoaded = false;

        public static List<Dictionary<string, string>> FindAll()
        {
            LoadData();
            return AllJobs;
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        public static List<string> FindAll(string column)
        {
            LoadData();

            List<string> values = new List<string>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                string aValue = job[column];

                if (!values.Contains(aValue))
                {
                    values.Add(aValue);
                }
            }
            return values;
        }

        /*
         * TOOD: these are informational at this point, but are actually bad reference docs.
         * param string column: will be a key in each dictionary describing the job (eg "position type")
         * param string value: a search term the user has entered
         */
        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            LoadData();

            value = value.ToLower();
            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> row in AllJobs)
            {
                string aValue = row[column].ToLower();

                if (aValue.Contains(value))
                {
                    jobs.Add(row);
                }
            }

            return jobs;
        }
        
        /*
         * @param string value: a search term the user has entered
         * @returns a list of jobs(dictionaries) that contain the param value
         */
        public static List<Dictionary<string, string>> FindByValue(string value)
        {
            // load data, if not already loaded
            LoadData();

            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                foreach (KeyValuePair<string, string> kv in job)
                {
                    if (kv.Value.ToLower().Contains(value.ToLower()) && ! result.Contains(job))
                        {
                        result.Add(job);
                        continue;
                        }
                }
            }

            return result;
        }

        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
        {

            if (IsDataLoaded)
            {
                return;
            }

            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] rowArrray = CSVRowToStringArray(line);
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            string[] headers = rows[0];
            rows.Remove(headers);

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                AllJobs.Add(rowDict);
            }

            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }
    }
}
