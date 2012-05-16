// 
// Job.cs
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
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Resfly
{
	[DataContract]
	public class Job
	{
		public ResflyApi ResflyApi { get; set; }
		
		[DataMember(Name = "id")]
		public int Id { get; set; }
				
		[DataMember(Name = "company_id")]
		public int CompanyId { get; set; }
		
		[DataMember(Name = "date_created")]
		public string DateCreatedString { get; set; }
		
		public DateTime DateCreated { get; set; }
			
		[DataMember(Name = "title")]
		public string Title { get; set; }
		
		[DataMember(Name = "city")]
		public string City { get; set; }
		
		[DataMember(Name = "state")]
		public string State { get; set; }		
		
		[DataMember(Name = "category")]
		public string Category { get; set; }
		
		[DataMember(Name = "description")]
		public string Description { get; set; }
		
		[DataMember(Name = "type")]
		public string Type { get; set; }
		
		[DataMember(Name = "salary")]
		public Salary Salary { get; set; }
	
		[DataMember(Name = "detail_url")]
		public string DetailUrl { get; set; }
		
		[DataMember(Name = "application_url")]
		public string ApplicationUrl { get; set; }
		
		[DataMember(Name = "status")]
		public string Status { get; set; }
		
		public Job(ResflyApi resflyApi)
		{
			this.ResflyApi = resflyApi;
			this.Salary = new Salary();
		}
		
		public bool Save()
		{
			Response response;
			
			if (this.Id < 1)
			{
				response = this.ResflyApi.MakeRequest("/jobs", "POST", this.ResflyApi.SerializeToJson(this));
				
				if (response.HttpWebResponse.StatusCode != HttpStatusCode.Created)
				{
					return false;
				}
			}
			else
			{
				response = this.ResflyApi.MakeRequest("/jobs/" + this.Id, "PUT", this.ResflyApi.SerializeToJson(this));
				
				if (response.HttpWebResponse.StatusCode != HttpStatusCode.OK)
				{
					return false;
				}
			}
			
			Job job = response.Job;
			this.Id = job.Id;
			this.DateCreatedString = job.DateCreatedString;
			this.DateCreated = DateTime.Parse(job.DateCreatedString);
			this.Title = job.Title;
			this.City = job.City;
			this.State = job.State;
			this.Category = job.Category;
			this.Description = job.Description;
			this.Type = job.Type;
			this.Salary = job.Salary;
			this.DetailUrl = job.DetailUrl;
			this.ApplicationUrl = job.ApplicationUrl;
			this.Status = job.Status;
			
			return true;
		}
		
		public bool Delete()
		{
			Response response = this.ResflyApi.MakeRequest("/jobs/" + this.Id, "DELETE");
			
			if (response.HttpWebResponse.StatusCode != HttpStatusCode.NoContent)
			{
				return false;
			}
			
			return true;
		}
				
		public bool Publish()
		{
			Response response = this.ResflyApi.MakeRequest("/jobs/" + this.Id + "/publish", "PUT");
			
			if (response.HttpWebResponse.StatusCode != HttpStatusCode.OK)
			{
				return false;				
			}
			
			this.Status = "published";
			
			return true;
		}
		
		public bool Close()
		{
			Response response = this.ResflyApi.MakeRequest("/jobs/" + this.Id + "/close", "PUT");
			
			if (response.HttpWebResponse.StatusCode != HttpStatusCode.OK)
			{				
				return false;
			}
			
			this.Status = "closed";
			
			return true;
		}
		
		public List<Candidate> GetCandidates()
		{
			Response response = this.ResflyApi.MakeRequest("/jobs/" + this.Id + "/candidates", "GET");
			
			List<Candidate> candidates = new List<Candidate>();
			
			if (response.Candidates != null)
			{				
				foreach (Response candidateResponse in response.Candidates)
				{
					candidates.Add(candidateResponse.Candidate);
				}
			}
			
			return candidates;
		}
	}
}

