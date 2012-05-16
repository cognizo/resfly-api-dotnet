// 
// Main.cs
//  
// Author:
//       Graham Floyd <gfloyd@resfly.com>
// 
// Copyright (c) 2012 Resfly, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using Resfly;

namespace ResflyApiExample
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			// Initialize API
			ResflyApi resflyApi = new ResflyApi("https://api.resfly.com", "API-KEY");
			
			// Create a company
			Company company = new Company(resflyApi);
			company.Name = "ACME, Inc.";
			company.Type = "internal";
			company.JobSlots = 5;
			company.Url = "http://www.acmeinc.com";
			
			company.Save();
			
			Console.WriteLine(company.Id);			
			Console.WriteLine(company.DateCreated.ToString());
			
			// Update company
			company.Name = "ACME International, Inc.";
			company.Save();
			
			Console.WriteLine(company.Name);
			
			// Create a job
			Job job = new Job(resflyApi);
			job.CompanyId = company.Id;
			job.Title = "Marketing Specialist";
			job.City = "Minneapolis";
			job.State = "MN";
			job.Category = "marketing_public_relations";
			job.Description = "This is the description for the job.";
			job.Type = "full_time";
			job.Salary.Amount = 50000;
			job.Salary.Type = "yearly";
			
			job.Save();
			
			Console.WriteLine(job.Id);
			Console.WriteLine(job.DateCreated.ToString());
			
			// Publish the job
			job.Publish();
			
			// Get all jobs for the company
			List<Job> companyJobs = company.GetJobs();
			
			foreach (Job companyJob in companyJobs)
			{
				Console.WriteLine(companyJob.Title);
			}
			
			// Get all candidates for the job
			List<Candidate> candidates = job.GetCandidates();
			
			foreach (Candidate candidate in candidates)
			{
				Console.WriteLine(candidate.FirstName + " " + candidate.LastName);
			}
			
			// Close the job
			job.Close();
			
			// Delete the job
			job.Delete();
		}
	}
}
