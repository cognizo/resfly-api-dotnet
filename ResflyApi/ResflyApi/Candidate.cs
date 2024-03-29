// 
// Candidate.cs
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
    public class Candidate
    {
        public ResflyApi ResflyApi;
        
        [DataMember(Name = "id")]
        public int Id { get; set; }
        
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }
        
        [DataMember(Name = "last_name")]
        public string LastName { get; set; }
        
        [DataMember(Name = "email")]
        public string Email { get; set; }
        
        [DataMember(Name = "source")]
        public string Source { get; set; }
        
        [DataMember(Name = "city")]
        public string City { get; set; }
        
        [DataMember(Name = "state")]
        public string State { get; set; }
        
        [DataMember(Name = "resume_url")]
        public string ResumeUrl { get; set; }
    
        public bool Save()
        {
            if (this.Id < 1)
            {
                return false;
            }
            
            Response response = this.ResflyApi.MakeRequest(
                "candidates/" + this.Id, 
                "PUT", 
                this.ResflyApi.SerializeToJson(this)
            );
            
            if (response.HttpWebResponse.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            
            Candidate candidate = response.Candidate;
            this.Id = candidate.Id;
            this.FirstName = candidate.FirstName;
            this.LastName = candidate.LastName;
            this.Email = candidate.Email;
            this.Source = candidate.Source;
            this.City = candidate.City;
            this.State = candidate.State;
            this.ResumeUrl = candidate.ResumeUrl;
            
            return true;
        }
        
        public List<Job> GetJobs()
        {
            Response response = this.ResflyApi.MakeRequest("/candidates/" + this.Id, "GET");
            
            List<Job> jobs = new List<Job>();
            
            if (response.Jobs != null)
            {
                foreach (Response jobResponse in response.Jobs)
                {
                    jobs.Add(jobResponse.Job);
                }
            }
            
            return jobs;
        }
    }
}

